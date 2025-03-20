using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Drowned : MonoBehaviour
{
    public Transform playerSpawn; // Referencia al punto de spawn del jugador
    public int damageAmount = 5; // Cantidad de salud a restar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DrownPlayer(other.gameObject));
        }
    }

    private IEnumerator DrownPlayer(GameObject player)
    {
        // Aquí puedes agregar una animación o efecto de ahogamiento si lo deseas
        yield return new WaitForSeconds(1f); // Esperar un segundo antes de reaparecer

        // Restar salud al jugador
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }

        // Desactivar temporalmente el CharacterController si existe
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



