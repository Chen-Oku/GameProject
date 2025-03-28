using System;
using System.Collections;
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

    // States
    public float sightRange, stareRange;
    public bool playerInSightRange, playerInAttackRange;

    public event Action OnDestroyed; // Evento que se dispara cuando el enemigo es destruido

    [Header("UI")]
    public GameObject uiComandos; // Objeto de UI para mostrar el mensaje
    private Coroutine uiCoroutine; // Declaración de la variable uiCoroutine
    private bool canShowUI = true; // Variable para controlar la reactivación de la UI

    private void Awake()
    {
        player = GameObject.Find("PlayerSak").transform;
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<TurtleEnemyHealth>(); // Obtener la referencia al script de salud
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (uiComandos != null)
        {
            uiComandos.SetActive(false); // Asegurarse de que el objeto de UI esté oculto al inicio
        }
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

        // Mostrar el mensaje si el jugador está en el rango de visión
        if (playerInSightRange && canShowUI)
        {
            if (uiComandos != null && uiCoroutine == null)
            {
                UIManager.instance.ShowUI(uiComandos);
                uiCoroutine = StartCoroutine(WaitForSec());
            }
        }
        else if (!playerInSightRange)
        {
            canShowUI = true; // Permitir que la UI se muestre nuevamente cuando el jugador vuelva a entrar en el rango
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

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(7);
        if (uiComandos != null)
        {
            UIManager.instance.HideActiveUI();
        }
        uiCoroutine = null; // Resetear la corrutina
        canShowUI = false; // Evitar que la UI se muestre nuevamente hasta que el jugador salga del rango
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