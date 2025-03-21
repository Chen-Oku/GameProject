using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GolemBoss : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float detectionRange;
    public float attackRange;
    [SerializeField] private LayerMask attackLayers;
    public float attackCooldown;
    public float respawnTime;
    public GameObject door;
    public int maxHealth;
    public Transform bossSpawnPoint;
    public int attackDamage = 20; // Da�o del ataque

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
        // L�gica para el ataque de un solo pu�o
        animator.SetTrigger("SinglePunch");
    }

    private void PerformDoublePunchAttack()
    {
        // L�gica para el ataque de dos pu�os
        animator.SetTrigger("DoublePunch");
    }

    // M�todo llamado por el evento de animaci�n para aplicar da�o al jugador
    public void ApplyDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackRange, attackRange, attackLayers);
        if(hitColliders.Length != 0)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                if(hitCollider.TryGetComponent(out PlayerHealth playerScript))
                {
                    playerScript.TakeDamage(attackDamage);
                }
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
        // Activar la animaci�n de recibir da�o
        if (animator != null)
        {
            animator.SetTrigger("GetHit");
            StartCoroutine(ResetHitTrigger());
        }
    }

    IEnumerator ResetHitTrigger()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo seg�n sea necesario
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
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}


