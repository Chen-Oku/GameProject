using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Salud m�xima del jugador
    public float currentHealth; // Salud actual del jugador
    private Animator animator; // Componente Animator
    public TextMeshProUGUI healthUI;
    bool isDead;

    //public GameObject healthBarUI;
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

        //if (currentHealth < maxHealth)
        //{
        //    healthBarUI.SetActive(true);
        //}

        //if (currentHealth <= 0)
        //{
        //    healthBarUI.SetActive(false);
        //}

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

        //if (healthBarUI != null)
        //{
        //    healthBarUI.SetActive(true);
        //    StopCoroutine(HideHealthBar());
        //    StartCoroutine(HideHealthBar());
        //}

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

    /*// Corrutina para ocultar la barra de salud después de un retraso
    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(2f); // Ajusta el tiempo según sea necesario
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }
    }*/

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

    // Método para recoger el power-up de salud
    public void CollectHealthBuff(float healthAmount)
    {
        currentHealth += healthAmount; // Aumentar la salud actual
        if (currentHealth > maxHealth) currentHealth = maxHealth; // Asegurarse de que la salud no exceda el máximo

        // Actualizar la UI de salud
        if (healthUI != null)
        {
            healthUI.text = currentHealth.ToString();
        }

        // Actualizar el slider de salud
        if (slider != null)
        {
            slider.value = CalculateCurrentHealth();
        }

        /*// Mostrar la barra de salud y reiniciar la corrutina para ocultarla
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
            StopCoroutine(HideHealthBar());
            StartCoroutine(HideHealthBar());
        }*/
    }


    public void RefreshUI()
    {
        healthUI.text = currentHealth.ToString();
        transform.position = startPos;
        isDead = false;
    }

}