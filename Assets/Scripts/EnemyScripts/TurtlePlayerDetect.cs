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
    public MultiProjectilePool projectilePool; // Pool de proyectiles
    public string projectileType; // Tipo de proyectil
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
        if (enemyHealth.IsDead) return; // No realizar ninguna acción si el enemigo está muerto

        // Verificar si el jugador está en el rango de visión y ataque
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        else if (playerInSightRange && playerInAttackRange) AttackPlayer();

        // Verificar si el enemigo ha alcanzado su punto de caminata
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isChasing", false);
            animator.ResetTrigger("isAttacking1");
            MoveToNextWaypoint();
        }
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
       agent.SetDestination(player.position);

        // Actualizar el estado de animación
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", true);
        animator.ResetTrigger("isAttacking1");
    }

    private void AttackPlayer()
    {
       // Verificar si el enemigo no se mueve
        agent.SetDestination(transform.position);

        // Asegurarse de que solo se rota en el eje Y para mirar al jugador
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // Mantener la rotación solo en el eje Y
        transform.rotation = Quaternion.LookRotation(lookDirection);

        if (!alreadyAttacked)
        {
            // Obtener un proyectil del pool
            GameObject instantiatedProjectile = projectilePool.GetProjectile(projectileType);
            instantiatedProjectile.transform.position = spawnProjectile.position;
            instantiatedProjectile.transform.rotation = Quaternion.identity;
            Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Calcular la dirección hacia el jugador
                Vector3 direction = (player.position - spawnProjectile.position).normalized;

                // Aplica fuerzas al proyectil para lanzarlo hacia el jugador
                rb.velocity = direction * 32f; // Usar velocity en lugar de AddForce para un control más directo
                print("Proyectil lanzado");
            }

            // Inicializar el proyectil con el pool y el tipo
            ProjectileBh projectileBh = instantiatedProjectile.GetComponent<ProjectileBh>();
            if (projectileBh != null)
            {
                projectileBh.Initialize(projectilePool, projectileType);
            }

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