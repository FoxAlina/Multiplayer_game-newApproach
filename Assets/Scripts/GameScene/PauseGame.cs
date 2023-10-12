using UnityEngine;
using Unity.Netcode;

public class PauseGame : NetworkBehaviour
{
    public GameObject pausePanel;

    private GameObject playerGO;
    private int playerId = 0;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    public void pauseGame()
    {
        if (IsOwner)
        {

            pausePanel.SetActive(true);
            Time.timeScale = 0.0f;


            Player[] players = FindObjectsOfType<Player>();
            foreach (var player in players)
            {
                if (player.IsOwner)
                {
                    playerId = player.playerId;
                    //playerGO = player.gameObject;
                    break;
                }

            }

            hidePlayerServerRpc();

        }
    }

    [ServerRpc]
    void hidePlayerServerRpc()
    {
        hidePlayerClientRpc();
    }

    [ClientRpc]
    void hidePlayerClientRpc()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (var player in players)
        {
            if (player.playerId == playerId)
            {
                playerGO = player.gameObject;
                break;
            }
        }
        Debug.Log(playerId);
        playerGO.SetActive(false);
    }

    public void resumeGame()
    {
        if (IsOwner)
        {

            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;

            dispkayPlayerServerRpc();

        }
    }

    [ServerRpc]
    void dispkayPlayerServerRpc()
    {
        dispkayPlayerClientRpc();
    }

    [ClientRpc]
    void dispkayPlayerClientRpc()
    {
        playerGO.SetActive(true);
    }
}
