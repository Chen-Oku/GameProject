using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBh : MonoBehaviour
{
    public float speed = 10f; // Velocidad del proyectil
    public float lifeTime = 5f; // Tiempo de vida del proyectil antes de destruirse autom�ticamente
    public float damage = 10f; // Da�o que el proyectil inflige
    public LayerMask playerLayer; // Capa del jugador

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontr� el componente Rigidbody en " + gameObject.name);
            return;
        }

        // Aplicar una fuerza inicial al proyectil
        rb.velocity = transform.forward * speed;

        // Destruir el proyectil despu�s de un tiempo de vida
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el proyectil colisiona con el jugador
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            // Hacer da�o al jugador
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Jugador recibi� da�o: " + damage);
            }
        }

        // Destruir el proyectil al colisionar con cualquier objeto
        Destroy(gameObject);
    }
}
