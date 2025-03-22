using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GolemBoss : MonoBehaviour, IEnemy
{
    public Transform[] patrolPoints;
    public float detectionRange;
    public float attackRange;
    [SerializeField] private LayerMask attackLayers;
    public float attackCooldown;
    public float respawnTime;
    public GameObject door;
    public Transform bossSpawnPoint;
    public int attackDamage = 20; // Daño del ataque

    public int maxHealth = 500;
    private int currentHealth;
    public TextMeshProUGUI healthUI;
    public GameObject healthBarUI;
    public Slider slider;

    private int currentPatrolIndex;
    private Transform player;
    private bool isDead;
    private bool alreadyAttacked;
    private Animator animator;
    private NavMeshAgent agent;
    public event System.Action OnDestroyed;

    public int scoreValue = 25; // Valor de puntaje del enemigo
    public bool isAttackable = true;

    void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud actual
        UpdateHealthUI();

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

        // Asegurarse de que la barra de salud esté oculta al inicio
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }

        currentPatrolIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if (isDead) return;

        UpdateHealthUI();

        if (currentHealth < maxHealth)
        {
            healthBarUI.SetActive(true);
        }

        if (currentHealth <= 0)
        {
            healthBarUI.SetActive(false);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerInSightRange = distanceToPlayer < detectionRange;
        bool playerInAttackRange = distanceToPlayer < attackRange;

        if (!playerInSightRange && !playerInAttackRange) StartCoroutine(Patrol());
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    void UpdateHealthUI()
    {
        if (slider != null)
        {
            slider.value = (float)currentHealth / maxHealth;
        }

        if (healthUI != null)
        {
            healthUI.text = currentHealth.ToString();
        }
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (isDead) yield break; // No patrullar si está muerto
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
        if (animator != null)
        {
            // Lógica para el ataque de un solo puño
            animator.SetTrigger("SinglePunch");
            StartCoroutine(ResetAttackTrigger());
        }

    }

    private void PerformDoublePunchAttack()
    {
        if (animator != null)
        {
            // Lógica para el ataque de dos puños
            animator.SetTrigger("DoublePunch");
            StartCoroutine(ResetAttackTrigger());
        }
    }

    // Método llamado por el evento de animación para aplicar daño al jugador
    public void ApplyDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackRange, attackRange, attackLayers);
        if (hitColliders.Length != 0)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out PlayerHealth playerScript))
                {
                    playerScript.TakeDamage(attackDamage);
                }
            }
        }
    }
    IEnumerator ResetAttackTrigger()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo según sea necesario
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

        currentHealth -= (int)damage;

        // Actualizar la UI de salud
        UpdateHealthUI();

        // Activar la animación de recibir daño
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
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo según sea necesario
        if (animator != null)
        {
            animator.ResetTrigger("GetHit");
        }
    }
    public bool IsDead => isDead;

    void Die()
    {
        if (isDead) return;
        UpdateHealthUI();

        // Activar la animación de muerte
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Establecer el enemigo como no atacable
        isAttackable = false;

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

        // Reactivar el agente de navegación
        if (agent != null)
        {
            agent.isStopped = false;
        }

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

