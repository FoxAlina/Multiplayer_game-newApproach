using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BeforeGameManager : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI clientsNumberText;
    [SerializeField] TextMeshProUGUI joinCodeText;

    [SerializeField] GameObject beforeGameMenu;

    private NetworkVariable<int> _netClientsNumber = new(writePerm:NetworkVariableWritePermission.Owner);

    private void Start()
    {
        beforeGameMenu.SetActive(true);

        StartGame();

        joinCodeText.text = GameCodeHolder.gameCode;
    }

    void Update()
    {
        if (IsServer)
        {
            _netClientsNumber.Value = NetworkManager.Singleton.ConnectedClientsList.Count;
            ShowClientsNumberClientRpc();
        }

    }

    [ClientRpc]
    void ShowClientsNumberClientRpc()
    {
        clientsNumberText.text = _netClientsNumber.Value.ToString();
    }

    public void StartGame()
    {
        switch (GameCodeHolder.playerType)
        {
            case PlayerType.HOST:
                NetworkManager.Singleton.StartHost();
                break;
            case PlayerType.CLIENT:
                NetworkManager.Singleton.StartClient();
                break;
        }
    }

    public void HideMenu()
    {
        beforeGameMenu.SetActive(false);
        Player.IsRunGame = true;
    }
}
