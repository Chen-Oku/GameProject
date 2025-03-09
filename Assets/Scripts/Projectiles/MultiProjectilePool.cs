using System.Collections.Generic;
using UnityEngine;

public class MultiProjectilePool : MonoBehaviour
{
    [System.Serializable]
    public class ProjectilePoolItem
    {
        public string type; // Tipo o nombre del proyectil
        public GameObject prefab; // Prefab del proyectil
        public int poolSize = 10; // Tamaño del pool
    }

    public List<ProjectilePoolItem> projectilePoolItems; // Lista de items del pool

    private Dictionary<string, Queue<GameObject>> pools;

    void Awake()
    {
        pools = new Dictionary<string, Queue<GameObject>>();

        foreach (var item in projectilePoolItems)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }

            pools[item.type] = pool;
        }
    }

    public GameObject GetProjectile(string type)
    {
        if (pools.ContainsKey(type) && pools[type].Count > 0)
        {
            GameObject obj = pools[type].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else if (pools.ContainsKey(type))
        {
            GameObject obj = Instantiate(projectilePoolItems.Find(item => item.type == type).prefab);
            return obj;
        }
        else
        {
            Debug.LogError("No se encontró el tipo de proyectil: " + type);
            return null;
        }
    }

    public void ReturnProjectile(string type, GameObject obj)
    {
        if (pools.ContainsKey(type))
        {
            obj.SetActive(false);
            pools[type].Enqueue(obj);
        }
        else
        {
            Debug.LogError("No se encontró el tipo de proyectil: " + type);
            Destroy(obj);
        }
    }
}
