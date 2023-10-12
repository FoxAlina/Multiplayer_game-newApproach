using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float speed = 10f;
    [SerializeField] public Slider healthBar;
    [SerializeField] float playerZComponent = -15;
    [SerializeField] ObjectPool bulletPool;
    [SerializeField] SpriteRenderer playerIcon;

    public float bulletFireRate = 0.25f;
    float timeCount = 0f;
    float shootTime = 0f;

    [HideInInspector] public ScoreAndHealthManager scoreAndHealthManager;

    public int playerId { get; set; }
    public TouchManager TouchManager { get; set; }
    public int Health { get; set; }
    public bool IsGameOver { get; set; }
    public static bool IsRunGame { get; set; } = false;

    public override void OnNetworkSpawn()
    {
        scoreAndHealthManager = FindObjectOfType<ScoreAndHealthManager>();

        //playerId = (int)NetworkObjectId;
        Player[] players = FindObjectsOfType<Player>();
        playerId = players.Length - 1;

        playerIcon.sprite = FindObjectOfType<PlayerIconsList>().GetIcon(playerId);

        if (IsOwner) FindObjectOfType<PlayerIconHolder>().SetPlayerIcon(playerIcon.sprite);

        healthBar.maxValue = scoreAndHealthManager.maxHealth;
        healthBar.value = scoreAndHealthManager.maxHealth;

        if (!IsOwner) enabled = false;
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
        TouchManager = FindObjectOfType<TouchManager>();

        IsGameOver = false;
    }

    void Update()
    {
        if (!IsGameOver && IsRunGame)
        {
            Vector3 v = TouchManager.getTargetVector();
            if (v != Vector3.zero)
            {
                Vector3 scaledMovement = rotationSpeed * Time.deltaTime * new Vector3(v.x, v.y, playerZComponent);
                //transform.LookAt(transform.position + scaledMovement, Vector3.forward);
                transform.rotation = Quaternion.LookRotation(scaledMovement, Vector3.forward);

                transform.Translate(speed * v.magnitude * Vector2.up * Time.deltaTime);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, playerZComponent);
        }
        if (IsGameOver)
            if (!IsOwner)
            {
                gameObject.SetActive(false);
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsOwner)
            if (collision.collider.tag == "Coin")
            {
                scoreAndHealthManager.CollectCoin();
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
        if (IsOwner)
        {
            scoreAndHealthManager.GetDamage();
            healthBar.value = scoreAndHealthManager.health;
        }
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

                RequestFireServerRpc(transform.position, transform.rotation.eulerAngles);

                ExecuteShoot(transform.position, transform.rotation.eulerAngles);
            }
        }
    }

    [ServerRpc]
    private void RequestFireServerRpc(Vector3 pos, Vector3 rot)
    {
        FireClientRpc(pos, rot);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 pos, Vector3 rot)
    {
        if (!IsOwner) ExecuteShoot(pos, rot);
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
