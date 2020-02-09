 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
public class ObjectPooler : MonoBehaviour
{
    public GameObject pooledObject;
    [SerializeField][Range(1,99999)] public float pooledAmount;
    public bool willGrow = true;
    
    public List<GameObject> pooledObjects;
    
    void Start ()
    {
        pooledObjects = new List<GameObject>();
        for(int i = 0; i < pooledAmount; i++)
        {
            SpawnObject();
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i< pooledObjects.Count; i++)
        {
            if(pooledObjects[i] == null)
            {
                GameObject obj = SpawnObject(false);
                pooledObjects[i] = obj;
                return pooledObjects[i];
            }
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }    
        }
        
        if (willGrow)
        {
            GameObject obj = SpawnObject();
            return obj;
        }
        
        return null;
    }

    private GameObject SpawnObject(bool appendToList=true)
    {   
        GameObject obj = Instantiate(pooledObject);
        if(appendToList)
        {
            pooledObjects.Add(obj);
        }
        obj.transform.parent = transform;
        obj.SetActive(false);
        return obj;
    }
    
}