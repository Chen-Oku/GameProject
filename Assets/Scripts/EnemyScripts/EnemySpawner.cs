using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab; // Prefab del enemigo
    }

    public List<EnemyType> enemyTypes; // Lista de tipos de enemigos
    public Transform[] spawnPoints; // Array de puntos de spawn
    public Transform[] waypoints; // Array de puntos de patrulla

    public float spawnTime; // Tiempo entre spawns
    public float spawnTimeRandom; // Variabilidad en el tiempo entre spawns
    public int maxEnemies; // Número máximo de enemigos permitidos al mismo tiempo

    private float spawnTimer;
    private int currentEnemyCount; // Contador de enemigos activos

    void Start()
    {
        if (enemyTypes.Count == 0)
        {
            Debug.LogError("No se han asignado tipos de enemigos en el Inspector.");
        }
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No se han asignado puntos de spawn en el Inspector.");
        }
        if (waypoints.Length == 0)
        {
            Debug.LogError("No se han asignado puntos de patrulla en el Inspector.");
        }

        ResetSpawnTimer();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0.0f && currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            ResetSpawnTimer();
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyTypes.Count == 0) return;

        // Seleccionar un punto de spawn aleatorio
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Seleccionar un tipo de enemigo aleatorio
        int enemyIndex = Random.Range(0, enemyTypes.Count);
        GameObject enemyPrefab = enemyTypes[enemyIndex].prefab;

        // Instanciar el enemigo en el punto de spawn seleccionado
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log("Enemigo instanciado: " + enemy.name + " en " + spawnPoint.position);

        // Incrementar el contador de enemigos activos
        currentEnemyCount++;

        // Verificar el tipo de enemigo y asignar los waypoints y el evento OnDestroyed
        TurtlePlayerDetect turtleEnemy = enemy.GetComponent<TurtlePlayerDetect>();
        if (turtleEnemy != null)
        {
            turtleEnemy.OnDestroyed += HandleEnemyDestroyed;
            turtleEnemy.SetWaypoints(waypoints); // Asignar los puntos de patrulla correctos
            return;
        }

        FireMPlayerDetect fireEnemy = enemy.GetComponent<FireMPlayerDetect>();
        if (fireEnemy != null)
        {
            fireEnemy.OnDestroyed += HandleEnemyDestroyed;
            fireEnemy.SetWaypoints(waypoints); // Asignar los puntos de patrulla correctos
            return;
        }
    }

    void ResetSpawnTimer()
    {
        spawnTimer = spawnTime + Random.Range(0f, spawnTimeRandom);
    }

    void HandleEnemyDestroyed()
    {
        // Decrementar el contador de enemigos activos
        currentEnemyCount--;
    }
}