using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Salud máxima del jugador
    private float currentHealth; // Salud actual del jugador
    private Animator animator; // Componente Animator
    public TextMeshProUGUI healthUI;
    bool isDead;

    Vector3 startPos;
    Animator anim;


    void Start()
    {
        healthUI.text = currentHealth.ToString();
        anim = GetComponent<Animator>();
        startPos = transform.position;

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
        print("Jugador recibió daño: " + damage + ". Salud actual: " + currentHealth);

        // Actualizar la UI de salud
        if (healthUI != null)
        {
            healthUI.text = currentHealth.ToString();
        }

        // Activar la animación de recibir daño
        if (animator != null)
        {
            animator.SetBool("isHit", true);
            StartCoroutine(ResetHitAnimation());
        }
        
        // Verificar si el jugador ha muerto
        if (currentHealth <= 0 && !isDead)
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
            animator.SetBool("isHit", false);
        }

    }
        // Método para manejar la muerte del jugador
    void Die()
    {
        isDead = true; // Establecer

        // Activar la animación de muerte
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        // Desactivar el objeto del jugador después de un tiempo
        Destroy(gameObject, 5f);
    }

    public void RefreshUI()
    {
        healthUI.text = currentHealth.ToString();
        transform.position = startPos;
        isDead = false;
    }

}