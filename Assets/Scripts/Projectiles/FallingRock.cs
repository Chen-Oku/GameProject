using UnityEngine;

public class FallingRock : MonoBehaviour
{
    public int damage = 5; // Daño que causa la roca al jugador

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        // Opcional: destruye la roca al impactar
        Destroy(gameObject);
    }
}