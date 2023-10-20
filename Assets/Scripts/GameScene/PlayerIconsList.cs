using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconsList : MonoBehaviour
{
    [SerializeField] private List<Sprite> playerIconsList;
    private List<int> takenSprites = new List<int>();
    public int availableSpriteItem = 0;

    public Sprite GetIcon(int index)
    {
        return playerIconsList[index];
    }

    public int GetIconIndex()
    {
        int result = availableSpriteItem;
        GetComponent<PhotonView>().RPC("SetIconIndexClientRpc", RpcTarget.All);
        return result;
    }

    [PunRPC]
    public void SetIconIndexClientRpc()
    {
        takenSprites.Add(availableSpriteItem);
        availableSpriteItem++;
    }
}
