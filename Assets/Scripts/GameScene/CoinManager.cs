using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoinManager : MonoBehaviour
{
    public List<GameObject> coins;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float numberOfCoins;

    [SerializeField] private float horizontalRadius;
    [SerializeField] private float verticalRadius;
    [SerializeField] private float coinZComponent;
    [SerializeField] private float coinSpawnRate;

    bool reload = true;

    void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            enabled = false;
        }

        coins = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < numberOfCoins; i++)
        {
            tmp = Instantiate(coinPrefab);
            tmp.SetActive(false);
            coins.Add(tmp);
        }
    }

    private void Update()
    {
        if (Player.IsRunGame)
            if (ActiveCoinsAmount() < numberOfCoins / 2)
            {
                if (reload) StartReloadCoroutine();
            }
    }

    GameObject GetCoin()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            if (!coins[i].activeInHierarchy)
                return coins[i];
        }
        return null;
    }

    int ActiveCoinsAmount()
    {
        int k = 0;
        for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i].activeInHierarchy)
                k++;
        }
        return k;
    }

    public void StartReloadCoroutine()
    {
        reload = false;
        StartCoroutine(reloadCoroutine());
    }

    IEnumerator reloadCoroutine()
    {
        yield return new WaitForSeconds(coinSpawnRate);

        for (int i = 0; i < 3; i++)
        {
            float y = Random.Range(-verticalRadius, verticalRadius);
            float x = Random.Range(-horizontalRadius, horizontalRadius);

            //setActiveCoinClientRpc(x, y);
            GetComponent<PhotonView>().RPC("setActiveCoinClientRpc", RpcTarget.All, x, y);
        }
        reload = true;
    }

    [PunRPC]
    public void setActiveCoinClientRpc(float x, float y)
    {
        GameObject tmp = GetCoin();
        if (tmp)
        {
            tmp.transform.position = new Vector3(x, y, coinZComponent);
            tmp.SetActive(true);
        }
    }
}
