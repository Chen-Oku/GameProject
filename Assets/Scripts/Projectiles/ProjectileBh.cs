using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBh : MonoBehaviour
{
    public float speed = 10f; // Velocidad del proyectil
    public float lifeTime = 5f; // Tiempo de vida del proyectil antes de destruirse automáticamente
    public float damage = 10f; // Daño que el proyectil inflige
    public LayerMask playerLayer; // Capa del jugador

    private Rigidbody rb;
    private MultiProjectilePool pool;
    private string type;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró el componente Rigidbody en " + gameObject.name);
            return;
        }

        // Destruir el proyectil después de un tiempo de vida
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    public void Initialize(MultiProjectilePool pool, string type)
    {
        this.pool = pool;
        this.type = type;
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el proyectil colisiona con el jugador
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            // Hacer daño al jugador
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        // Devolver el proyectil al pool al colisionar con cualquier objeto
        ReturnToPool();
    }

    void ReturnToPool()
    {
        if (pool != null)
        {
            pool.ReturnProjectile(type, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}