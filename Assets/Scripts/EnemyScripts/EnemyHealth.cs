using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Salud máxima del enemigo
    private float currentHealth; // Salud actual del enemigo
    private Animator animator; // Componente Animator
    public TextMeshProUGUI healthUI;
    public bool isAttackable = true;
    private bool isDead;

    Animator anim;
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

        animator = GetComponent<Animator>(); // Obtener el componente Animator
        if (animator == null)
        {
            Debug.LogError("No se encontró el componente Animator en " + gameObject.name);
        }
    }

    // Método para recibir daño
    public void TakeDamage(float damage)
    {
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
        if (animator != null)
        {
            animator.SetBool("GetHit", true);
            StartCoroutine(ResetHitAnimation());
        }

        // Verificar si el enemigo ha muerto
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    // Corrutina para desactivar la animación de recibir daño después de un breve período de tiempo
    IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo según sea necesario
        if (animator != null)
        {
            animator.SetBool("GetHit", false);
        }
    }

    // Método para manejar la muerte del enemigo
    void Die()
    {
        if (isDead) return;

        // Activar la animación de muerte
        if (animator != null)
        {
            animator.SetBool("Die", true);
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