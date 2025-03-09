using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireMEnemyHealth : MonoBehaviour, IEnemy
{
    public float maxHealth = 100f; // Salud máxima del enemigo
    private float currentHealth; // Salud actual del enemigo
    public TextMeshProUGUI healthUI;
    public bool isAttackable = true;
    private bool isDead;

    private Animation animationComponent; // Componente Animation
    Vector3 startPos;

    void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud actual
        if (healthUI != null)
        {
            healthUI.text = currentHealth.ToString();
        }
        else
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI en " + gameObject.name);
        }

        animationComponent = GetComponent<Animation>(); // Obtener el componente Animation
        if (animationComponent == null)
        {
            Debug.LogError("No se encontró el componente Animation en " + gameObject.name);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // No recibir daño si ya está muerto

        currentHealth -= damage; // Reducir la salud actual
        if (currentHealth < 0) currentHealth = 0; // Asegurarse de que la salud no sea negativa

        // Imprimir mensaje de depuración
        Debug.Log("Enemigo recibió daño: " + damage + ". Salud actual: " + currentHealth);

        // Actualizar la UI de salud
        if (healthUI != null)
        {
            healthUI.text = currentHealth.ToString();
        }

        // Activar la animación de recibir daño
        if (animationComponent != null)
        {
            animationComponent.Play("Anim_Damage");
        }

        // Verificar si el enemigo ha muerto
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool IsDead => isDead;

    void Die()
    {
        if (isDead) return;

        // Imprimir mensaje de depuración
        Debug.Log("Enemigo ha muerto. Reproduciendo animación de muerte.");

        // Activar la animación de muerte
        if (animationComponent != null)
        {
            animationComponent.CrossFade("Anim_Death", 0.2f);
        }
        else
        {
            Debug.LogError("No se encontró el componente Animation en " + gameObject.name);
        }

        // Establecer el enemigo como no atacable
        isAttackable = false;

        isDead = true;
        Destroy(gameObject, 5f);
    }
}