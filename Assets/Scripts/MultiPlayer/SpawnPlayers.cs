using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] float leftBorder, rightBorder, topBorder, bottomBorder;

    void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(leftBorder, rightBorder), Random.Range(topBorder, bottomBorder));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

        //SetPlayerIcon();
    }

    public void SetPlayerIcon()
    {
        NetworkPlayer myPlayer;
        NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();
        foreach (var player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                myPlayer = player;
                myPlayer.SetPlayerIcon((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]);
                break;
            }
        }

    }
}
