using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPlayerDetect : MonoBehaviour
{
    Animator animator;
    //bool isDead;
    public bool isAttackable = true;

    public NavMeshAgent agent; // Agente de navegación
    public Transform player; // Jugador
    public LayerMask Terrain, Player; // Capas de terreno y jugador

    //public float health;
    //public TextMeshProUGUI healthUI; 

    //Patroling
    public Vector3 walkPoint; // Punto de caminata
    bool walkPointSet; // Para que el enemigo no camine constantemente
    public float walkPointRange; // Rango de caminata

    //Attacking
    public float timeBetweenAttacks; // Tiempo entre ataques
    bool alreadyAttacked; // Para que el enemigo no ataque constantemente
    public GameObject projectile; // Proyectil que lanzará el enemigo
    public Transform spawnProjectile; // Punto de spawn del proyectil

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private PathPatrol pathPatrol;
    
    private void Awake()
    {
        player = GameObject.Find("PlayerSak").transform;
        agent = GetComponent<NavMeshAgent>();
        pathPatrol = GetComponent<PathPatrol>();
       
    }

    void Start()
    {
        //healthUI.text = health.ToString();
        animator = GetComponent<Animator>();
       
    }

    void Update()
    {
        //verificar si el jugador está en el rango de visión y ataque
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

       
    }

    private void Patrolling()
    {
        if (pathPatrol != null)
        {
            pathPatrol.Patrol();
        }
        else
        {
            Debug.LogWarning("PathPatrol component not found.");
        }

        //if (!walkPointSet) SearchWalkPoint();

        //if (walkPointSet)
        //    agent.SetDestination(walkPoint);

        //Vector3 distanceToWalkPoint = transform.position - walkPoint;

        ////se alcanza el punto de caminata
        //if (distanceToWalkPoint.magnitude < 1f)
        //    walkPointSet = false;
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
        //verificar si el enemigo no se mueve
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Instanciar el proyectil en el punto de spawn
            Rigidbody rb = Instantiate(projectile, spawnProjectile.position, Quaternion.identity).GetComponent<Rigidbody>();
            //Aplica fuerzas al proyectil para lanzarlo.
            rb.AddForce(spawnProjectile.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            print("Proyectil lanzado");

            //atacar al jugador
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            // Actualizar el estado de animación
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isChasing", false);
            animator.SetTrigger("isAttacking1");

        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    //public void TakeDamage(int damage)
    //{
    //    health -= damage;
    //    if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    //    {
    //        animator.SetBool("GetHit", true);
    //        StartCoroutine(ResetHitAnimation());

    //        print("Enemy Health: " + health);
    //        print("Enemy Hit");

    //    }

    //}

    IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo según sea necesario
        if (animator != null)
        {
            animator.SetBool("GetHit", false);
        }

    }

       
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}