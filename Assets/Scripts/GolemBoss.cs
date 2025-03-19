using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GolemBoss : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed;
    public float detectionRange;
    public float attackRange;
    public float attackCooldown;
    public float respawnTime;
    public GameObject door;
    public int maxHealth;
    public Transform bossSpawnPoint;
    public int attackDamage = 20; // Daño del ataque

    private int currentHealth;
    private int currentPatrolIndex;
    private Transform player;
    private bool isDead;
    private bool alreadyAttacked;
    private Animator animator;
    private NavMeshAgent agent;

    public int scoreValue = 25; // Valor de puntaje del enemigo

    void Start()
    {
        currentPatrolIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerInSightRange = distanceToPlayer < detectionRange;
        bool playerInAttackRange = distanceToPlayer < attackRange;

        if (!playerInSightRange && !playerInAttackRange) StartCoroutine(Patrol());
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (patrolPoints.Length == 0) yield break;

            Transform targetPoint = patrolPoints[currentPatrolIndex];
            agent.SetDestination(targetPoint.position);

            while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);
                yield return null;
            }

            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            yield return new WaitForSeconds(1f);
        }
    }

    private void ChasePlayer()
    {
        if (isDead) return;

        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if (isDead) return;

        agent.SetDestination(transform.position);

        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDirection);

        if (!alreadyAttacked)
        {
            int attackType = Random.Range(0, 2);
            if (attackType == 0)
            {
                PerformSinglePunchAttack();
            }
            else
            {
                PerformDoublePunchAttack();
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void PerformSinglePunchAttack()
    {
        // Lógica para el ataque de un solo puño
        animator.SetTrigger("SinglePunch");
        DealDamageToPlayer();
    }

    private void PerformDoublePunchAttack()
    {
        // Lógica para el ataque de dos puños
        animator.SetTrigger("DoublePunch");
        DealDamageToPlayer();
    }

    private void DealDamageToPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth playerScript = player.GetComponent<PlayerHealth>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        // Activar la animación de recibir daño
        if (animator != null)
        {
            animator.SetTrigger("GetHit");
            StartCoroutine(ResetHitTrigger());
        }
    }

    IEnumerator ResetHitTrigger()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo según sea necesario
        if (animator != null)
        {
            animator.ResetTrigger("GetHit");
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        ScoreManager.instance.AddScore(scoreValue);
        OpenDoors openDoors = door.GetComponent<OpenDoors>();
        if (openDoors != null)
        {
            openDoors.Open();
        }
        else
        {
            Debug.LogError("El componente OpenDoors no se encuentra en el objeto door.");
        }

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        isDead = false;
        currentHealth = maxHealth;
        transform.position = bossSpawnPoint.position;
        StartCoroutine(Patrol());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
