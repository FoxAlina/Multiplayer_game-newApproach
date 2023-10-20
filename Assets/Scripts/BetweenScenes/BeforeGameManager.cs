using System.Collections;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BeforeGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI clientsNumberText;
    [SerializeField] TextMeshProUGUI joinCodeText;

    [SerializeField] GameObject beforeGameMenu;

    [SerializeField] SpawnPlayers spawnPlayersScript;

    NetworkPlayer[] players;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    private void Start()
    {
        beforeGameMenu.SetActive(true);

        joinCodeText.text = GameCodeHolder.gameCode;

        StartCoroutine(LookForPlayers());
    }

    private IEnumerator LookForPlayers()
    {
        while (true)
        {
            players = FindObjectsOfType<NetworkPlayer>();
            clientsNumberText.text = players.Length.ToString();

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void HideMenu()
    {
        beforeGameMenu.SetActive(false);
        NetworkPlayer.IsRunGame = true;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("playerAvatar"))
        {
            int index = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];
            Debug.Log(index);

            foreach(var player in players)
            {
                if (player.GetComponent<PhotonView>().IsMine)
                {
                    player.GetComponent<PhotonView>().RPC("SetPlayerIcon", RpcTarget.All);
                    break;
                }
            }
        }
    }
}
