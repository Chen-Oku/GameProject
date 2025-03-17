using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{

    public EnemyPool enemyPool; // Referencia al pool de enemigos
    // public GameObject tortuga; // Se asigna en el Inspector
    public Transform[] spawnPoints; // Array de puntos de spawn


    public float spawnTime; // Tiempo entre spawns

    public float spawnTimeRandom; // Variabilidad en el tiempo entre spawns

    private float spawnTimer;
   // private NavMeshAgent nav;
    //public Transform target;
    private Vector3 location;

    // Start is called before the first frame update
    void Start()
    {
        if (enemyPool == null)
        {
            Debug.LogError("No se ha asignado el prefab de la tortuga en el Inspector.");
        }
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No se han asignado puntos de spawn en el Inspector.");
        }

        ResetSpawnTimer();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
       // target = GameObject.FindGameObjectWithTag("PlayerSak").transform;
        // if (spawnTimer <= 0.0f)
        // {
        //     Instantiate(tortuga, location, Quaternion.identity);

            //ResetSpawnTimer();
        if (spawnTimer <= 0.0f)
        {
           SpawnEnemy();
           ResetSpawnTimer();
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        // Seleccionar un punto de spawn aleatorio
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Instanciar el enemigo en el punto de spawn seleccionado
        Instantiate(enemyPool, spawnPoint.position, spawnPoint.rotation);
    } 


    void ResetSpawnTimer()
    {
        spawnTimer = (float)(spawnTime + Random.Range(0, spawnTimer * 100) / 100.0);
    }
}
