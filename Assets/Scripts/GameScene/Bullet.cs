using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] int timeToDisable = 3;

    public int playerId;

    private void Start()
    {
        StartCoroutine(lifeTime());
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            gameObject.SetActive(false);
        }

        if (collision.tag == "Player" && gameObject.tag == "PlayerBullet")
        {
            if (collision.GetComponent<NetworkPlayer>().playerId != playerId)
            {
                if (collision.GetComponent<NetworkPlayer>().photonView.IsMine)
                    collision.GetComponent<NetworkPlayer>().photonView.RPC("BulletHit", Photon.Pun.RpcTarget.All);
                gameObject.SetActive(false);
            }
        }

    }

    IEnumerator lifeTime()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false);
    }
}
