using FreeflowCombatSpace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f;

    private bool isInvincible = false;
    bool isDead;
    public TextMeshProUGUI healthUI;

    Animator anim;

    public enum STATE
    {
        Idle, Hit, Die
    }
    public STATE currState = STATE.Idle;

    public GameObject damageTextPrefab;
    public Transform damageTextPos;

    private void Start()
    {
        // Intentar obtener el componente Animator del objeto actual
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            // Si no se encuentra, intentar obtener el componente Animator de un objeto hijo
            anim = GetComponentInChildren<Animator>();
            if (anim == null)
            {
                Debug.LogError("No se encontr� el componente Animator en " + gameObject.name);
            }
        }

        // Asignar healthUI en el c�digo (opcional)
        if (healthUI == null)
        {
            healthUI = GetComponentInChildren<TextMeshProUGUI>();
            if (healthUI == null)
            {
                Debug.LogError("No se encontr� el componente TextMeshProUGUI en " + gameObject.name);
            }
        }
    }

    public void Hit(int hitPoints)
    {
        health -= hitPoints;
        if (health < 0) health = 0;

        if (healthUI != null)
        {
            healthUI.text = health.ToString(); // L�nea 35
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void ApplyDmg(DmgInfo dmgInfo)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            ChangeState(STATE.Hit);
            GameObject dmgText = Instantiate(damageTextPrefab, damageTextPos.position, Quaternion.identity);
            dmgText.GetComponent<DamagePopup>().SetUp(dmgInfo.dmgValue + Random.Range(-10, 10), dmgInfo.textColor);
            StartCoroutine(ResetStateAfterDelay(1f)); // Volver al estado IDLE despu�s de 1 segundo
        }
    }

    void Die()
    {
        if (isDead) return;

        // Cambiar el estado a Die
        ChangeState(STATE.Die);

        // Reproducir la animaci�n de muerte
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        // Desactivar la capacidad de ser atacado
        GetComponent<FreeflowCombatEnemy>().isAttackable = false;

        // Desactivar el script de rotaci�n
        GetComponent<LookAtPlayer>().enabled = false;

        isDead = true;

        // Destruir el objeto despu�s de 2 segundos
        Destroy(gameObject, 2f);
    }

    void ChangeState(STATE newState)
    {
        currState = newState;
        // Aqu� puedes agregar la l�gica para manejar el cambio de estado
    }

    IEnumerator ResetStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(STATE.Idle);
        isInvincible = false;
    }
}

