using System.Collections.Generic;
using UnityEngine;

public class PlayerIconsList : MonoBehaviour
{
    [SerializeField] private List<Sprite> playerIconsList;
    //public Dictionary<int, int> takenSprites = new Dictionary<int, int>();

    public Sprite GetIcon(int k)
    {
        return playerIconsList[k];
    }

    //public Sprite getIcon(int k)
    //{
    //    if (!takenSprites.ContainsKey(k))
    //    {
    //        int v = 0;
    //        while (takenSprites.ContainsValue(v))
    //        {
    //            v = Random.Range(0, 24);
    //        }
    //        takenSprites.Add(k, v);
    //        return playerIconsList[v];
    //    }
    //    return null;
    //}
}
