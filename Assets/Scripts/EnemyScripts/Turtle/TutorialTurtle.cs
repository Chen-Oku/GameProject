using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TutorialTurtle : MonoBehaviour
{
    private Animator animator;
    private TurtleEnemyHealth enemyHealth; // Referencia al script de salud del enemigo

    public bool isAttackable = true;

    [Header("Enemy Interaction")]
    public NavMeshAgent agent; // Agente de navegación
    public LayerMask Terrain, Player; // Capas de terreno y jugador

    [Header("Attack Stats")]
    private Transform player; // Jugador

    //States
    public float sightRange, stareRange;
    public bool playerInSightRange, playerInAttackRange;

    public event Action OnDestroyed; // Evento que se dispara cuando el enemigo es destruido

    private void Awake()
    {
        player = GameObject.Find("PlayerSak").transform;
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<TurtleEnemyHealth>(); // Obtener la referencia al script de salud
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (enemyHealth.IsDead) return; // No realizar ninguna acción si el enemigo está muerto

        // Verificar si el jugador está en el rango de visión y ataque
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, stareRange, Player);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        else if (playerInAttackRange) AttackPlayer();

        // Verificar si el enemigo ha alcanzado su punto de caminata
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isChasing", false);
        }
    }

    private void Patrolling()
    {
        // Actualizar el estado de animación
        animator.SetBool("isPatrolling", true);
        animator.SetBool("isChasing", false);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        // Actualizar el estado de animación
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", true);
    }

    private void AttackPlayer()
    {
        // Verificar si el enemigo no se mueve
        agent.SetDestination(transform.position);

        // Asegurarse de que solo se rota en el eje Y para mirar al jugador
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // Mantener la rotación solo en el eje Y
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // Aquí puedes agregar la lógica de ataque si es necesario
    }

    private void OnDestroy()
    {
        // Disparar el evento OnDestroyed cuando el enemigo sea destruido
        OnDestroyed?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stareRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

