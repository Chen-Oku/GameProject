using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GolemBoss : MonoBehaviour, IEnemy
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
    public event System.Action OnDestroyed;

    public int scoreValue = 25; // Valor de puntaje del enemigo

    void Start()
    {
        // Buscar automáticamente los puntos de patrulla con la etiqueta "BossWayPoint"
        GameObject[] wayPointObjects = GameObject.FindGameObjectsWithTag("BossWayPoint");
        patrolPoints = new Transform[wayPointObjects.Length];

        for (int i = 0; i < wayPointObjects.Length; i++)
        {
            patrolPoints[i] = wayPointObjects[i].transform;
        }
        // Buscar automáticamente el objeto puerta con la etiqueta "BossDoor"
        GameObject doorObject = GameObject.FindGameObjectWithTag("BossDoor");
        if (doorObject != null)
        {
            door = doorObject;
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con la etiqueta 'BossDoor'.");
        }

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
        if(animator != null)
        {
            // L�gica para el ataque de un solo pu�o
            animator.SetTrigger("SinglePunch");
            StartCoroutine(ResetAttackTrigger());
        }

    }

    private void PerformDoublePunchAttack()
    {
        if(animator != null)
        {
            // L�gica para el ataque de dos pu�os
            animator.SetTrigger("DoublePunch");
            StartCoroutine(ResetAttackTrigger());
        }
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
        IEnumerator ResetAttackTrigger()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo seg�n sea necesario
        if (animator != null)
        {
            animator.ResetTrigger("SinglePunch");
            animator.SetTrigger("DoublePunch");
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= (int) damage;
        // Activar la animaci�n de recibir da�o
        if (animator != null)
        {
            animator.SetTrigger("GetHit");
            StartCoroutine(ResetHitTrigger());
        }
        if (currentHealth <= 0)
        {
            Die();
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
    public bool IsDead => isDead;

    void Die()
    {
        isDead = true;

        // Activar la animaci�n de muerte
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
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

        isDead = true;
        OnDestroyed?.Invoke();
        Destroy(gameObject, 5f);

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

     public int GetScoreValue()
    {
        return scoreValue;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}


