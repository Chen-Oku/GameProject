using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireMEnemyHealth : MonoBehaviour, IEnemy
{
    public float maxHealth = 100f; // Salud m�xima del enemigo
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
            Debug.LogError("No se encontr� el componente TextMeshProUGUI en " + gameObject.name);
        }

        animationComponent = GetComponent<Animation>(); // Obtener el componente Animation
        if (animationComponent == null)
        {
            Debug.LogError("No se encontr� el componente Animation en " + gameObject.name);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // No recibir da�o si ya est� muerto

        currentHealth -= damage; // Reducir la salud actual
        if (currentHealth < 0) currentHealth = 0; // Asegurarse de que la salud no sea negativa

        // Imprimir mensaje de depuraci�n
        Debug.Log("Enemigo recibi� da�o: " + damage + ". Salud actual: " + currentHealth);

        // Actualizar la UI de salud
        if (healthUI != null)
        {
            healthUI.text = currentHealth.ToString();
        }

        // Activar la animaci�n de recibir da�o
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

        // Imprimir mensaje de depuraci�n
        Debug.Log("Enemigo ha muerto. Reproduciendo animaci�n de muerte.");

        // Activar la animaci�n de muerte
        if (animationComponent != null)
        {
            animationComponent.CrossFade("Anim_Death", 0.2f);
        }
        else
        {
            Debug.LogError("No se encontr� el componente Animation en " + gameObject.name);
        }

        // Establecer el enemigo como no atacable
        isAttackable = false;

        isDead = true;
        Destroy(gameObject, 5f);
    }
}