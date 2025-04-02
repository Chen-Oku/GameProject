using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectiles"))
        {
            Destroy(gameObject); // Destruir el muro
            Destroy(collision.gameObject); // Destruir el proyectil
        }
    }
}



