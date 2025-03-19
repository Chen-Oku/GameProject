using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab; // Prefab del enemigo
        public int initialSize; // Tamaño inicial del pool
    }

    [SerializeField] private List<EnemyType> enemyTypes; // Lista de tipos de enemigos
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary; // Pool para cada tipo de enemigo

    private void Awake()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        // Inicializa el pool para cada tipo de enemigo
        foreach (EnemyType enemyType in enemyTypes)
        {
            Queue<GameObject> enemyQueue = new Queue<GameObject>();

            for (int i = 0; i < enemyType.initialSize; i++)
            {
                GameObject enemyInstance = Instantiate(enemyType.prefab);
                enemyInstance.SetActive(false);
                enemyQueue.Enqueue(enemyInstance);
            }

            poolDictionary.Add(enemyType.prefab, enemyQueue);
        }
    }

    // Método para obtener un enemigo del pool
    public GameObject GetEnemy(GameObject enemyPrefab)
    {
        if (!poolDictionary.ContainsKey(enemyPrefab))
        {
            Debug.LogWarning($"No existe el tipo de enemigo para el prefab: {enemyPrefab.name}");
            return null;
        }

        Queue<GameObject> enemyQueue = poolDictionary[enemyPrefab];

        if (enemyQueue.Count > 0)
        {
            GameObject enemy = enemyQueue.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            Debug.LogWarning($"No hay más enemigos disponibles en el pool para el prefab: {enemyPrefab.name}");
            return null;
        }
    }

    // Método para devolver un enemigo al pool
    public void ReturnEnemy(GameObject enemyPrefab, GameObject enemy)
    {
        if (!poolDictionary.ContainsKey(enemyPrefab))
        {
            Debug.LogWarning($"No existe el tipo de enemigo para el prefab: {enemyPrefab.name}");
            return;
        }

        enemy.SetActive(false);
        poolDictionary[enemyPrefab].Enqueue(enemy);
    }
}





