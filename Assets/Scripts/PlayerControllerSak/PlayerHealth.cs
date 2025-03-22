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

private float CurrentHealth{
    get => currentHealth;
    set
    {
        if (value <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else if (value >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = value;
        }

        if (healthUI != null)
        {
            healthUI.text = currentHealth.ToString();
        }
        else
        {
            Debug.LogError("No se encontr� el componente TextMeshProUGUI en " + gameObject.name);
        }

        if (slider != null)
        {
            slider.value = CalculateCurrentHealth();
        }
    }
}

    void Start()
    {
        CurrentHealth = maxHealth; // Inicializar la salud actual
       
        anim = GetComponent<Animator>();
        startPos = transform.position;

        animator = GetComponent<Animator>(); // Obtener el componente Animator
        if (animator == null)
        {
            Debug.LogError("No se encontr� el componente Animator en " + gameObject.name);
        }
    }

    void Update()
    {
        
    }

    float CalculateCurrentHealth()
    {
        return currentHealth / maxHealth;
    }

    // M�todo para recibir da�o
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage; // Reducir la salud actual

       // healthBar.value = currentHealth; 

        // Imprimir mensaje de depuraci�n
        //print("Jugador recibi� da�o: " + damage + ". Salud actual: " + currentHealth);

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

    // Metodo para manejar la muerte del jugador
    void Die()
    {
        if(isDead) return; // Verificar si el jugador ya ha muerto

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
        CurrentHealth += healthAmount; // Aumentar la salud actual
       
    }


    public void RefreshUI()
    {
        healthUI.text = currentHealth.ToString();
        transform.position = startPos;
        isDead = false;
    }

}