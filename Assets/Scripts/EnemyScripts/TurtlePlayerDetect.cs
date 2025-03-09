using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TurtlePlayerDetect : MonoBehaviour
{
    private Animator animator;
    private TurtleEnemyHealth enemyHealth; // Referencia al script de salud del enemigo

    public bool isAttackable = true;

    [Header("Enemy Interaction")]
    public NavMeshAgent agent; // Agente de navegación
    public LayerMask Terrain, Player; // Capas de terreno y jugador

    [Header("Patrol")]
    public Transform[] waypoints; // Array de puntos de patrulla
    private int currentWaypointIndex = 0; // Índice del punto de patrulla actual
    public float walkPointRange; // Rango de caminata

    [Header("Attack Stats")]
    public Transform player; // Jugador
    public float timeBetweenAttacks; // Tiempo entre ataques
    private bool alreadyAttacked; // Para que el enemigo no ataque constantemente
    public GameObject projectile; // Proyectil que lanzará el enemigo
    public Transform spawnProjectile; // Punto de spawn del proyectil
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

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
        // Verificar si el jugador está en el rango de visión y ataque
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (waypoints.Length == 0) return;

        // Establecer el destino al siguiente punto de patrulla
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Actualizar el estado de animación
        animator.SetBool("isPatrolling", true);
        animator.SetBool("isChasing", false);
        animator.ResetTrigger("isAttacking1");
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        // Incrementar el índice del punto de patrulla actual
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    private void ChasePlayer()
    {
        if (enemyHealth.IsDead) return; // No perseguir si el enemigo está muerto

        agent.SetDestination(player.position);

        // Actualizar el estado de animación
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", true);
        animator.ResetTrigger("isAttacking1");
    }

    private void AttackPlayer()
    {
        if (enemyHealth.IsDead) return; // No atacar si el enemigo está muerto

        // Verificar si el enemigo no se mueve
        agent.SetDestination(transform.position);

        // Asegurarse de que solo se rota en el eje Y para mirar al jugador
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // Mantener la rotación solo en el eje Y
        transform.rotation = Quaternion.LookRotation(lookDirection);

        if (!alreadyAttacked)
        {
            // Instanciar el proyectil en el punto de spawn
            Rigidbody rb = Instantiate(projectile, spawnProjectile.position, Quaternion.identity).GetComponent<Rigidbody>();
            // Aplica fuerzas al proyectil para lanzarlo.
            rb.AddForce(spawnProjectile.forward * 32f, ForceMode.Impulse);
            print("Proyectil lanzado");

            // Atacar al jugador
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            // Reproducir la animación de ataque
            animator.SetTrigger("isAttacking1");
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}