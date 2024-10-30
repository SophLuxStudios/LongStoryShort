using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    //Singleton
    public static ObjectPooler Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with " + tag + " tag doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = null;

        //Dequeue from pool
        if(poolDictionary[tag].Count > 0)
        {
            //Debug.Log("Remaining " + tag + " count in pool is: " + poolDictionary[tag].Count);
            objectToSpawn = poolDictionary[tag].Dequeue();
        }
        else//if none remaining in the pool instantiate a new pool object
        {
            Debug.Log("New" + tag + " prefab created.");
            foreach(Pool pool in pools)
            {
                if(pool.tag == tag)
                {
                    objectToSpawn = Instantiate(pool.prefab);
                }
            }
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        return objectToSpawn;
    }

    public void ReturnPool(string tag, GameObject objectToReturnPool)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with " + tag + " tag doesn't exist.");
            return;
        }
        
        objectToReturnPool.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturnPool);
    }
}