using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Salud m�xima del jugador
    private float currentHealth; // Salud actual del jugador
    private Animator animator; // Componente Animator
    public TextMeshProUGUI healthUI;
    bool isDead;

    public GameObject healthBarUI;
    public Slider slider; 

    Vector3 startPos;
    Animator anim;


    void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud actual
        slider.value = CalculateCurrentHealth();
       
        anim = GetComponent<Animator>();
        startPos = transform.position;

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

    void Update()
    {
        slider.value = CalculateCurrentHealth();

        if (currentHealth < maxHealth)
        {
            healthBarUI.SetActive(true);
        }

        if (currentHealth <= 0)
        {
            healthBarUI.SetActive(false);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    float CalculateCurrentHealth()
    {
        return currentHealth / maxHealth;
    }
    // M�todo para recibir da�o
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Reducir la salud actual
        if (currentHealth < 0) currentHealth = 0; // Asegurarse de que la salud no sea negativa
       // healthBar.value = currentHealth; 

        // Imprimir mensaje de depuraci�n
        print("Jugador recibi� da�o: " + damage + ". Salud actual: " + currentHealth);

        // Actualizar la UI de salud
        if (healthUI != null)
        {
            healthUI.text = currentHealth.ToString();
        }

        // Activar la animaci�n de recibir da�o
        if (animator != null)
        {
            animator.SetBool("isHit", true);
            StartCoroutine(ResetHitAnimation());
        }

        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
            StopCoroutine(HideHealthBar());
            StartCoroutine(HideHealthBar());
        }

        // Verificar si el jugador ha muerto
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    
    // Corrutina para desactivar la animaci�n de recibir da�o despu�s de un breve per�odo de tiempo
    IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo seg�n sea necesario
        if (animator != null)
        {
            animator.SetBool("isHit", false);
        }

    }

    // Corrutina para ocultar la barra de salud después de un retraso
    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(2f); // Ajusta el tiempo según sea necesario
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }
    }

        // Metodo para manejar la muerte del jugador
    void Die()
    {
        isDead = true; // Establecer

        // Activar la animaci�n de muerte
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        // Desactivar el objeto del jugador despu�s de un tiempo
        Destroy(gameObject, 5f);
    }


    public void RefreshUI()
    {
        healthUI.text = currentHealth.ToString();
        transform.position = startPos;
        isDead = false;
    }

}