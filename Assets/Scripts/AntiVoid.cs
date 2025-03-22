using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AntiVoid : MonoBehaviour
{
    public Transform playerSpawn; // Referencia al punto de spawn del jugador
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DrownPlayer(other.gameObject));
        }
    }

    private IEnumerator DrownPlayer(GameObject player)
    {
        yield return new WaitForSeconds(0.5f); // Esperar un segundo antes de reaparecer

        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Reaparecer al jugador en el punto de spawn
        player.transform.position = playerSpawn.position;
        player.transform.rotation = playerSpawn.rotation;

        // Reactivar el CharacterController si existe
        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }
}



