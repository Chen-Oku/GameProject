using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurtleEnemyHealth : MonoBehaviour, IEnemy
{
    public float maxHealth = 100f; // Salud m�xima del enemigo
    private float currentHealth; // Salud actual del enemigo
    private Animator animator; // Componente Animator
    public TextMeshProUGUI healthUI;
    public bool isAttackable = true;
    private bool isDead;

    public GameObject healthBarUI;
    public Slider slider;

    public int scoreValue = 25; // Valor de puntaje del enemigo
    public event System.Action OnDestroyed;

    void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud actual
        slider.value = CalculateCurrentHealth();
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

        // Asegurarse de que la barra de salud est� oculta al inicio
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isDead) return;

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
        if (isDead) return; // No recibir da�o si ya est� muerto

        currentHealth -= damage; // Reducir la salud actual
        if (currentHealth < 0) currentHealth = 0; // Asegurarse de que la salud no sea negativa

        // Imprimir mensaje de depuraci�n

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

        // Mostrar la barra de salud y reiniciar la corrutina para ocultarla
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
            StopCoroutine(HideHealthBar());
            StartCoroutine(HideHealthBar());
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

    // Corrutina para ocultar la barra de salud despu�s de un retraso
    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(2f); // Ajusta el tiempo seg�n sea necesario
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }
    }

    public bool IsDead => isDead;

    // M�todo para manejar la muerte del enemigo
    void Die()
    {
        if (isDead) return;
        slider.value = CalculateCurrentHealth();

        // Activar la animaci�n de muerte
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Establecer el enemigo como no atacable
        isAttackable = false;

        ScoreManager.instance.AddScore(scoreValue); // Otorgar puntaje al jugador

        // Soltar un PowerUP con una probabilidad
        PowerUpProb.instance.DropPowerUp(transform.position);

        isDead = true;
        OnDestroyed?.Invoke();
        Destroy(gameObject, 3f);
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

    public void RefreshUI()
    {
        healthUI.text = currentHealth.ToString();
    }
}
