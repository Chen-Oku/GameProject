using System.Collections;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject golemBossPrefab; // Prefab del GolemBoss
    public Transform spawnPoint; // Punto donde se spawneará el GolemBoss
    public float respawnDelay = 10f; // Tiempo de espera antes de spawnear el siguiente GolemBoss
    private GameObject door;

    private GameObject currentBoss; // Referencia al GolemBoss actual
    private bool isSpawning; // Indica si se está esperando para spawnear un nuevo jefe

    void Start()
    {
        GameObject doorObject = GameObject.FindGameObjectWithTag("BossDoor");
        if (doorObject != null)
        {
            door = doorObject;
        }
    }

    void Update()
    {
        // Si no hay un jefe activo y no se está esperando para spawnear, invocar al jefe
        if (currentBoss == null && !isSpawning)
        {
            StartCoroutine(SpawnBoss());
        }
    }

    IEnumerator SpawnBoss()
    {
        isSpawning = true;
        Debug.Log("GolemBoss Spawn Start method called."); // Mensaje de depuración

        // Esperar el tiempo de respawn
        yield return new WaitForSeconds(respawnDelay);

        // Cerrar la puerta al respawnear
        OpenDoors openDoors = door.GetComponent<OpenDoors>();
        if (openDoors != null)
        {
            Debug.Log("Cerrando la puerta.");
            openDoors.Close();
        }

        // Spawnear el GolemBoss
        currentBoss = Instantiate(golemBossPrefab, spawnPoint.position, spawnPoint.rotation);

        isSpawning = false;
    }
}