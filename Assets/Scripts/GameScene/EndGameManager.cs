using Unity.Netcode;
using UnityEngine;

public class EndGameManager : NetworkBehaviour
{
    [SerializeField] ScoreAndHealthManager scoreAndHealthManager;

    private int clientsNumber = 0;
    private bool gameover = false;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject winText;

    public override void OnNetworkSpawn()
    {
        HideAll();

        Player.IsRunGame = false;
    }

    void Update()
    {
        if (Player.IsRunGame && !gameover)
        {
            Player[] players = FindObjectsOfType<Player>();
            clientsNumber = players.Length;
            if (clientsNumber == 1)
            {
                ShowWin();
                Player.IsRunGame = false;
            }
        }
    }

    #region UI
    void ShowWin()
    {
        scoreAndHealthManager.ShowFinishScore();
        gameOverPanel.SetActive(true);
        winText.SetActive(true);
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
    public void PlayerGameOver(int playerId)
    {
        ShowGameOver();

        SetFalsePlayerServerRpc(playerId);

        gameover = true;
    }

    [ServerRpc(RequireOwnership = false)]
    void SetFalsePlayerServerRpc(int playerId)
    {
        SetFalsePlayerClientRpc(playerId);
    }

    [ClientRpc]
    void SetFalsePlayerClientRpc(int playerId)
    {
        Player[] players = FindObjectsOfType<Player>();
        Player gameOverPlayer = players[0];
        foreach (var player in players)
        {
            if (player.playerId == playerId)
                gameOverPlayer = player;
        }
        gameOverPlayer.gameObject.SetActive(false);
    }
    #endregion

    #region Bact to main menu
    public void BactToMainMenu()
    {
        Disconnect();
        Cleanup();

        LoadScene.StartGame("MainScene");
    }

    void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }

    void Cleanup()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }
    #endregion
}