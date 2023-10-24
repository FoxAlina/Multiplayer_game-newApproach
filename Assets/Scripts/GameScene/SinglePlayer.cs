using UnityEngine;
using UnityEngine.UI;

public class SinglePlayer : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float speed = 10f;
    [SerializeField] float playerZComponent = -15;
    [SerializeField] ObjectPool bulletPool;
    [SerializeField] SpriteRenderer playerIcon;
    [SerializeField] public Slider healthBar;

    public float bulletFireRate = 0.25f;
    float timeCount = 0f;
    float shootTime = 0f;

    [HideInInspector] public ScoreAndHealthManager scoreAndHealthManager;

    public int playerId { get; set; }
    public ExtendedTouchManager TouchManager { get; set; }
    public int Health { get; set; }
    public bool IsGameOver { get; set; }
    public static bool IsRunGame { get; set; } = false;

    void Awake()
    {
        scoreAndHealthManager = FindObjectOfType<ScoreAndHealthManager>();

        SinglePlayer[] players = FindObjectsOfType<SinglePlayer>();
        playerId = players.Length - 1;

        playerIcon.sprite = FindObjectOfType<PlayerIconsList>().GetIcon(playerId);

        FindObjectOfType<PlayerIconHolder>().SetPlayerIcon(playerIcon.sprite);

        healthBar.maxValue = scoreAndHealthManager.maxHealth;
        healthBar.value = scoreAndHealthManager.maxHealth;
    }

    private void OnEnable()
    {
        PlayerShooting.OnPlayerFire += Fire;
    }

    private void OnDisable()
    {
        PlayerShooting.OnPlayerFire -= Fire;
    }

    void Start()
    {
        TouchManager = FindObjectOfType<ExtendedTouchManager>();

        IsGameOver = false;
    }

    void Update()
    {
        if (!IsGameOver && IsRunGame)
        {
            MovePlayer();
        }
        if (IsGameOver)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreAndHealthManager.CollectCoin();
        }
    }

    //private void MovePlayer()
    //{
    //    Vector3 v = TouchManager.getTargetVector();
    //    if (v != Vector3.zero)
    //    {
    //        Vector3 scaledMovement = rotationSpeed * Time.deltaTime * new Vector3(v.x, v.y, playerZComponent);
    //        //transform.LookAt(transform.position + scaledMovement, Vector3.forward);
    //        transform.rotation = Quaternion.LookRotation(scaledMovement, Vector3.forward);


    //        transform.Translate(speed * v.magnitude * Vector2.up * Time.deltaTime);
    //        transform.position = new Vector3(transform.position.x, transform.position.y, playerZComponent);
    //    }
    //}

    private void MovePlayer()
    {
        var (rotationVector, translationVector) = TouchManager.getTargetVector();
        if (rotationVector != Vector3.zero)
        {
            Vector3 scaledMovement = rotationSpeed * Time.deltaTime * new Vector3(rotationVector.x, rotationVector.y, playerZComponent);
            // transform.LookAt(transform.position + scaledMovement, Vector3.forward)
            transform.rotation = Quaternion.LookRotation(scaledMovement, Vector3.forward);

            transform.Translate(speed * translationVector.magnitude * Vector2.up * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, playerZComponent);
        }
    }

    public void SetBulletIds()
    {
        for (int i = 0; i < bulletPool.SharedInstance.amountToPool; i++)
        {
            bulletPool.SharedInstance.pooledObjects[i].GetComponent<Bullet>().playerId = playerId;
        }
    }

    #region GetDamage
    public void BulletHit()
    {
        scoreAndHealthManager.GetDamage();
        healthBar.value = scoreAndHealthManager.health;
    }

    public void ShowHealth()
    {
        healthBar.value = Health;
    }
    #endregion

    #region Shooting
    public void Fire()
    {
        if (!IsGameOver)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= shootTime)
            {
                shootTime = timeCount + bulletFireRate;

                ExecuteShoot(transform.position, transform.rotation.eulerAngles);
            }
        }
    }

    private void ExecuteShoot(Vector3 pos, Vector3 rot)
    {
        GameObject bullet = bulletPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.GetComponent<Bullet>().playerId = playerId;
            bullet.transform.position = pos;
            bullet.transform.rotation = Quaternion.Euler(rot);
            bullet.transform.Rotate(new Vector3(0f, 0f, 90f));
            bullet.SetActive(true);
        }
    }
    #endregion
}
