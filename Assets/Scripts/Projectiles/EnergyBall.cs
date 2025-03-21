using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public float speed = 20f; // Velocidad del proyectil
    public int damage = 10; // Da�o del proyectil
    public float lifeTime = 5f; // Tiempo de vida del proyectil

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontr� el componente Rigidbody en " + gameObject.name);
        }

        // Aplicar una velocidad inicial al proyectil
        rb.velocity = transform.forward * speed;

        // Destruir el proyectil despu�s de un tiempo para evitar que quede en la escena indefinidamente
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Mover el proyectil hacia adelante
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el proyectil colisiona con un enemigo
        if (other.gameObject.CompareTag("Enemy"))
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage); // Aplicar daño al enemigo
            }

            // Destruir el proyectil despues de impactar
            Destroy(gameObject);
        }
    }
}