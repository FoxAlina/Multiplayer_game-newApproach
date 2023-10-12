using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class ObjectPool : MonoBehaviour
{
    public ObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    [SerializeField] private GameObject prefab;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(prefab);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public void SetActiveObjectsFalse()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if (pooledObjects[i].activeInHierarchy)
                pooledObjects[i].SetActive(false);
            else continue;
        }
    }

    public int ActiveObjectsAmount()
    {
        int k = 0;
        for (int i = 0; i < amountToPool; i++)
        {
            if (pooledObjects[i].activeInHierarchy)
                k++;
        }
        return k;
    }
}
