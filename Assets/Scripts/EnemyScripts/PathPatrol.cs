/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPatrol : MonoBehaviour
{
    public Transform[] waypoints; // Array de puntos de patrulla
    public float speed = 2f; // Velocidad de movimiento
    private int currentWaypointIndex = 0; // Índice del punto de patrulla actual
    public int targetPoint; // Punto de patrulla al que se dirige
    public float rotationSpeed = 5f; // Velocidad de rotación

    void Start()
    {
        targetPoint = 0; // Inicializa el punto de patrulla al que se dirige

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == waypoints[targetPoint].position) // Si el objeto llega al punto de patrulla
        {
            increaseTargetPoint(); // Incrementa el punto de patrulla al que se dirige
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetPoint].position, speed * Time.deltaTime); // Mueve el objeto hacia el punto de patrulla
    
        MoveTowardsTarget();
    }


    void MoveTowardsTarget()
    {
        // Mueve el objeto hacia el punto de patrulla
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetPoint].position, speed * Time.deltaTime);

        // Rota el objeto para que mire hacia el punto de patrulla
        Vector3 direction = (waypoints[targetPoint].position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Si el objeto llega al punto de patrulla
        if (Vector3.Distance(transform.position, waypoints[targetPoint].position) < 0.1f)
        {
            increaseTargetPoint(); // Incrementa el punto de patrulla al que se dirige
        }
    }

    private void increaseTargetPoint()
    {
        targetPoint = Random.Range(0, waypoints.Length);
        if (targetPoint >= waypoints.Length) // Si el punto de patrulla al que se dirige es mayor o igual al número de puntos de patrulla
        {
            targetPoint = 0; // Reinicia el punto de patrulla al que se dirige
        }
    }

}

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathPatrol : MonoBehaviour
{
    public Transform[] waypoints; // Array de puntos de patrulla
    public float speed = 2f; // Velocidad de movimiento
    //private int currentWaypointIndex = 0; // Índice del punto de patrulla actual
    public int targetPoint; // Punto de patrulla al que se dirige
    public float rotationSpeed = 5f; // Velocidad de rotación
    private NavMeshAgent agent;

    void Start()
    {
        targetPoint = 0; // Inicializa el punto de patrulla al que se dirige
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        MoveToNextWaypoint();
    }

    public void Patrol()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned for patrol.");
            return;
        }

        // Si el agente ha llegado al punto de patrulla actual
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        // Selecciona un punto de patrulla aleatorio
        targetPoint = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[targetPoint].position);
    }
}