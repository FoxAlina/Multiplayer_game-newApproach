using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndHealthManager : MonoBehaviour
{
    [SerializeField] private EndGameManager endGameManager;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthCounter;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI coinsCounter;
    [SerializeField] private Slider coinsBar;

    [Header("GameOver Panel UI")]
    [SerializeField] private TextMeshProUGUI finishCoinsScore;

    private int _coins;
    private readonly int _maxHealth = 10;
    private int _health;

    public int coins { get => _coins; }
    public int health { get => _health; }
    public int maxHealth { get => _maxHealth; }

    void Start()
    {
        _coins = 0;
        _health = _maxHealth;

        healthBar.maxValue = _maxHealth;
        healthBar.value = _health;
        coinsBar.maxValue = 10;
        coinsBar.value = _coins;

        healthCounter.text = _health.ToString();
        coinsCounter.text = _coins.ToString();

        finishCoinsScore.text = _coins.ToString();
    }

    public void CollectCoin()
    {
        _coins++;
        coinsCounter.text = _coins.ToString();
        coinsBar.value = _coins;
        if (_coins >= coinsBar.maxValue)
            coinsBar.maxValue += 10;
    }

    public void GetDamage()
    {
        _health--;
        healthBar.value = _health;
        healthCounter.text = _health.ToString();
        if (_health <= 0)
        {
            ShowFinishScore();

            Player[] players = FindObjectsOfType<Player>();
            if(players.Length != 0)
            {
                Player mainPlayer = players[0];
                foreach (var player in players)
                {
                    if (player.IsOwner) { mainPlayer = player; break; }
                }
                mainPlayer.IsGameOver = true;

                endGameManager.PlayerGameOver(mainPlayer.playerId);
            }
        }
    }

    public void ShowFinishScore()
    {
        finishCoinsScore.text = _coins.ToString();
    }

    public void ShowHealth()
    {
        healthBar.value = _health;
        healthCounter.text = _health.ToString();
    }
}
