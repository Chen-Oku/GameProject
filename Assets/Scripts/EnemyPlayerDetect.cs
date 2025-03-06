using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPlayerDetect : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask Terrain, Player;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

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
    }
   
    /*
    private void SearchWalkPoint()
    {
        //calcula un punto random en cierto rango
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }
    */

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //verificar si el enemigo no se mueve
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {



            //atacar al jugador
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
