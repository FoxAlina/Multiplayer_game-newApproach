using Photon.Pun;
using UnityEngine;

public class EndGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] ScoreAndHealthManager scoreAndHealthManager;

    public int clientsNumber = 0;

    PhotonView photonView;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject winText;

    void Awake()
    {
        HideAll();

        NetworkPlayer.IsRunGame = false;

        photonView = GetComponent<PhotonView>();
    }

    public override void OnLeftRoom()
    {
        LoadScene.StartScene("LobbyScene");
    }

    #region UI

    void ShowWin()
    {
        scoreAndHealthManager.ShowFinishScore();
        gameOverPanel.SetActive(true);
        winText.SetActive(true);

        NetworkPlayer.IsRunGame = false;
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.SetActive(true);
    }

    void HideAll()
    {
        gameOverPanel.SetActive(false);
        gameOverText.SetActive(false);
        winText.SetActive(false);
    }
    #endregion

    #region End game for player
    public void PlayerGameOver(NetworkPlayer player)
    {
        player.IsGameOver = true;

        ShowGameOver();

        photonView.RPC("GameOverForThisPlayerClientRpc", RpcTarget.All, player.playerId);
    }

    [PunRPC]
    void GameOverForThisPlayerClientRpc(int playerId)
    {
        NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();
        foreach (var player in players)
        {
            if (player.playerId == playerId)
            {
                player.gameObject.SetActive(false);
                clientsNumber--;
                break;
            }
        }

        CheckNumberOfPlayers();
    }

    void CheckNumberOfPlayers()
    {
        NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();
        if (clientsNumber == 1)
        {
            foreach (var playerIter in players)
            {
                if (playerIter.photonView.IsMine)
                {
                    ShowWin();
                    break;
                }
            }
        }
    }
    #endregion

    #region Bact to main menu
    public void BactToMainMenu()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
    }

    #endregion
}