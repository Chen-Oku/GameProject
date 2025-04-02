using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    public int hitsToDestroy = 3; // Número de impactos necesarios para destruir el muro
    private int currentHits = 0; // Contador de impactos actuales

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonProjectile"))
        {
            currentHits++; // Incrementar el contador de impactos

            if (currentHits >= hitsToDestroy)
            {
                Destroy(gameObject); // Destruir el muro
            }

            Destroy(collision.gameObject); // Destruir el proyectil
        }
    }
}




