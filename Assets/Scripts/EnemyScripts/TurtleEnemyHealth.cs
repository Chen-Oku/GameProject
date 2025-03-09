using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurtleEnemyHealth : MonoBehaviour, IEnemy
{
    public float maxHealth = 100f; // Salud m�xima del enemigo
    private float currentHealth; // Salud actual del enemigo
    private Animator animator; // Componente Animator
    public TextMeshProUGUI healthUI;
    public bool isAttackable = true;
    private bool isDead;

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

        animator = GetComponent<Animator>(); // Obtener el componente Animator
        if (animator == null)
        {
            Debug.LogError("No se encontr� el componente Animator en " + gameObject.name);
        }
    }

    // M�todo para recibir da�o
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
        if (animator != null)
        {
            animator.SetTrigger("GetHit");
            StartCoroutine(ResetHitTrigger());
        }

        // Verificar si el enemigo ha muerto
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Corrutina para desactivar el Trigger de GetHit despu�s de un breve per�odo de tiempo
    IEnumerator ResetHitTrigger()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo seg�n sea necesario
        if (animator != null)
        {
            animator.ResetTrigger("GetHit");
        }
    }

    public bool IsDead => isDead;

    // M�todo para manejar la muerte del enemigo
    void Die()
    {
        if (isDead) return;

        // Activar la animaci�n de muerte
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Establecer el enemigo como no atacable
        isAttackable = false;

        isDead = true;
        Destroy(gameObject, 5f);
    }

    public void RefreshUI()
    {
        healthUI.text = currentHealth.ToString();
        transform.position = startPos;
        isDead = false;
    }
}