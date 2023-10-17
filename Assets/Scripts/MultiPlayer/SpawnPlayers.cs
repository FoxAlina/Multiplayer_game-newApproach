using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField]  float leftBorder, rightBorder, topBorder, bottomBorder;

    void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(leftBorder, rightBorder), Random.Range(topBorder, bottomBorder));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
