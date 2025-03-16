using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{

    public GameObject tortuga;
    public float spawnTime;
    public float spawnTimeRandom;

    private float spawnTimer;
    private NavMeshAgent nav;
    public Transform target;
    private Vector3 location;

    // Start is called before the first frame update
    void Start()
    {
        tortuga = GameObject.FindGameObjectWithTag("TurtleShellPBR");
        Vector3 location = new Vector3(89,10,9);

        /* ResetSpawnTimer(); */
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        target = GameObject.FindGameObjectWithTag("").transform;
        if (spawnTimer <= 0.0f)
        {
            Instantiate(tortuga, location, Quaternion.identity);

            /* ResetSpawnTimer(); */
        }
    }
}
