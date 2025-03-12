using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 10f; // Da�o del ataque
    public float attackRange = 1.5f; // Rango del ataque
    public LayerMask enemyLayer; // Capa de los enemigos
    public float attackCooldown = 1f; // Tiempo de espera entre ataques

    private Animator animator;
    private bool canAttack = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontr� el componente Animator en " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack) // Bot�n izquierdo del rat�n para atacar
        {
            StartCoroutine(Attack());
        }
    }
    /*
        void Attack()
        {
            if (animator != null)
            {
                // Reproducir la animaci�n de ataque
                animator.SetTrigger("isAttacking");
            }

            // Detectar enemigos en el rango de ataque
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange, attackRange, enemyLayer);

            // Aplicar da�o a los enemigos detectados
            foreach (Collider enemy in hitEnemies)
            {
                EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage((int)attackDamage); // Convertir attackDamage a int
                }
            }
        }
    */
    IEnumerator Attack()
    {
        if (animator != null)
        {
            // Reproducir la animaci�n de ataque
            animator.SetTrigger("isAttacking");
        }

        // Deshabilitar la capacidad de atacar
        canAttack = false;

        // Esperar hasta que la animaci�n de ataque haya terminado
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Detectar enemigos en el rango de ataque
        PerformAttack();

        // Esperar el tiempo de cooldown adicional
        yield return new WaitForSeconds(attackCooldown);

        // Habilitar la capacidad de atacar nuevamente
        canAttack = true;
    }
        
    public void PerformAttack()
    {
        // Detectar enemigos en el rango de ataque
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange, attackRange, enemyLayer);

        // Aplicar da�o a los enemigos detectados
        foreach (Collider enemy in hitEnemies)
        {
            IEnemy enemyScript = enemy.GetComponent<IEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage((int)attackDamage); // Convertir attackDamage a int
            }
        }

    }

    // Dibujar el rango de ataque en la escena para depuraci�n
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRange);
    }
}