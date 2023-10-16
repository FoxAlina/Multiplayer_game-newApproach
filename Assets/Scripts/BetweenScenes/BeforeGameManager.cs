using TMPro;
using UnityEngine;

public class BeforeGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI clientsNumberText;
    [SerializeField] TextMeshProUGUI joinCodeText;

    [SerializeField] GameObject beforeGameMenu;


    private void Start()
    {
        beforeGameMenu.SetActive(true);

        StartGame();

        joinCodeText.text = GameCodeHolder.gameCode;
    }

    void Update()
    {
        

    }

    public void StartGame()
    {
        switch (GameCodeHolder.playerType)
        {
            case PlayerType.HOST:
                
                break;
            case PlayerType.CLIENT:
                
                break;
        }
    }

    public void HideMenu()
    {
        beforeGameMenu.SetActive(false);
        Player.IsRunGame = true;
    }
}
