/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireMEnemyHealth : MonoBehaviour, IEnemy
{
    public float maxHealth = 100f; // Salud m�xima del enemigo
    private float currentHealth; // Salud actual del enemigo
    public TextMeshProUGUI healthUI;
    public bool isAttackable = true;
    private bool isDead;

    public GameObject healthBarUI;
    public Slider slider;

    private Animation animationComponent; // Componente Animation
    Vector3 startPos;

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

        animationComponent = GetComponent<Animation>(); // Obtener el componente Animation
        if (animationComponent == null)
        {
            Debug.LogError("No se encontr� el componente Animation en " + gameObject.name);
        }

        // Asegurarse de que la barra de salud est� oculta al inicio
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
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
}*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireMEnemyHealth : MonoBehaviour, IEnemy
{
    public float maxHealth = 100f; // Salud m�xima del enemigo
    private float currentHealth; // Salud actual del enemigo
    public TextMeshProUGUI healthUI;
    public bool isAttackable = true;
    private bool isDead;

    public GameObject healthBarUI;
    public Slider slider;

    private Animation animationComponent; // Componente Animation
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

        animationComponent = GetComponent<Animation>(); // Obtener el componente Animation
        if (animationComponent == null)
        {
            Debug.LogError("No se encontr� el componente Animation en " + gameObject.name);
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

        // Actualizar la UI de salud
        RefreshUI();
    }

    float CalculateCurrentHealth()
    {
        return currentHealth / maxHealth;
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

    void Die()
    {
        if (isDead) return;
        slider.value = CalculateCurrentHealth();

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
        OnDestroyed?.Invoke();
        ScoreManager.instance.AddScore(scoreValue); // Otorgar puntaje al jugador
        Destroy(gameObject, 5f);
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        // Implementar si es necesario
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

    private void RefreshUI()
    {
        if (healthUI != null)
        {
            healthUI.text = $"{currentHealth}";
        }
        if (slider != null)
        {
            slider.value = currentHealth / maxHealth;
        }
    }
}
