using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField]

    public class EnemyPoolItem
    {
        public string EnemyType;
        public GameObject enemyPrefab; // Prefab del enemigo
        public int poolSize = 10; // Tamaño del pool
    }
    public GameObject enemyPrefab; // Prefab del enemigo
    public int poolSize = 10; // Tamaño del pool

    private Queue<GameObject> pool;

    void Start()
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        if (pool.Count > 0)
        {
            GameObject enemy = pool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            GameObject enemy = Instantiate(enemyPrefab);
            return enemy;
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}