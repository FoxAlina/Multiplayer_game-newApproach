using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<PlayerNetworkData> _playerState;

    [SerializeField] private bool _serverAuth;

    private void Awake()
    {
        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerState = new NetworkVariable<PlayerNetworkData>(writePerm: permission);
    }

    private void Update()
    {
        if (IsOwner)
            TransmitState();
        else
        {
            transform.position = _playerState.Value.Position;
            transform.rotation = Quaternion.Euler(_playerState.Value.Rotation.x, _playerState.Value.Rotation.y, _playerState.Value.Rotation.z);

            GetComponent<Player>().Health = _playerState.Value.Health;
            GetComponent<Player>().ShowHealth();
        }
    }

    private void TransmitState()
    {
        var state = new PlayerNetworkData
        {
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles,

            Health = GetComponent<Player>().scoreAndHealthManager.health
        };

        if(IsServer || !_serverAuth)
        {
            _playerState.Value = state;
        }
        else
        {
            TransmitStateServerRpc(state);
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkData state)
    {
        _playerState.Value = state;
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y;
        private short _xRot, _yRot, _zRot;

        private int _health;

        internal Vector3 Position
        {
            get => new Vector3(_x, _y, 0);
            set
            {
                _x = value.x;
                _y = value.y;
            }
        }

        internal Vector3 Rotation
        {
            get => new Vector3(_xRot, _yRot, _zRot);
            set
            {
                _xRot = (short)value.x;
                _yRot = (short)value.y;
                _zRot = (short)value.z;
            }
        }

        internal int Health
        {
            get => _health;
            set => _health = value;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);

            serializer.SerializeValue(ref _xRot);
            serializer.SerializeValue(ref _yRot);
            serializer.SerializeValue(ref _zRot);

            serializer.SerializeValue(ref _health);
        }
    }
}
