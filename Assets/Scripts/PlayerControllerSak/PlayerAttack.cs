using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    /* Intentando hacer el ataque de energía del juegador  */
    public GameObject EnergyBall; //Prefab de la bola de energía 
    public Transform spawnPoint; //Punto desde donde se dispara la bola 
    public float projectileSpeed = 10f; //Velocidad del ataque 

    /* Intentando hacer el ataque de energía del juegador  */

    public float attackDamage = 10f; // Daño del ataque
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

    /* Intentando hacer el ataque de energía del juegador  */
        if (Input.GetMouseButtonDown(0) && canAttack) //Click izquierdo para ataque cuerpo a cuerpo
        {
            StartCoroutine(Attack());
        }
        if (Input.GetMouseButtonDown(1)) //Click derecho para disparar bola de energía
        {
            shootEnergyBall();
        }
    /* Intentando hacer el ataque de energía del juegador  */
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

    void shootEnergyBall() 
    {
        if (EnergyBall ==  null || spawnPoint == null)
        {
            Debug.LogError("Falta asignar el prefab de la bola de energía o el punto del spawn");
        }
    
        /* GameObject energyBall = Instantiate(EnergyBall, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = energyBall.GetComponent<Rigidbody>(); */

        GameObject energyBall = Instantiate(EnergyBall, spawnPoint.position, spawnPoint.rotation);
        //Obetner el Rigidbody de la bola y aplicar la fuerza en la direacción en la que mira el jugador 
        Rigidbody rb = energyBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * projectileSpeed;
        }

        Destroy(energyBall, 5f); //Destruir el proyectil después de 5 segundos
    }
}