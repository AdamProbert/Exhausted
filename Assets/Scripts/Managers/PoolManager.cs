using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PoolManager : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;

    public GameObject poolObjectParent;

    public static PoolManager Instance;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start() 
    {
        poolObjectParent = new GameObject("PooledObjects");
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pool pool in pools)
        {
            GameObject x = new GameObject(pool.tag + " pool");
            x.transform.parent = poolObjectParent.transform;
            
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform.position, Quaternion.identity, x.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool object with tag " + tag + " doesn't exist");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(obj);
        return obj;
    }
}