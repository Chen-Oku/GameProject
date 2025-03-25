using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private WayPointPathSQP1 _waypointPath; // Reference to the waypoint path

    [SerializeField]
    private float _speed; // Speed of the platform

    private int _targetWaypointIndex; // Index of the target waypoint

    private Transform _previusWaypoint; // Reference to the previous waypoint
    private Transform _targetWaypoint; // Reference to the target waypoint

    private float _timeToWaypoint; // Time to reach the target waypoint
    private float _elapseTime; // Elapsed time

        // Start is called before the first frame update
    void Start()
    {
        TargetNextWaypoint(); // Target the next waypoint
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _elapseTime += Time.deltaTime; // Increase the elapsed time

        float elapsedPercentage =  _elapseTime / (float) _timeToWaypoint; // Calculate the percentage of time that has passed
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);  // Smooth the percentage of time that has passed
        transform.position = Vector3.Lerp(_previusWaypoint.position, _targetWaypoint.position, elapsedPercentage); // Move the platform
        transform.rotation = Quaternion.Lerp(_previusWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage); // Rotate the platform

        if (elapsedPercentage >= 1) // If the platform has reached the target waypoint
        {
            TargetNextWaypoint(); // Target the next waypoint
        }
    }

    private void TargetNextWaypoint()
    {
        _previusWaypoint = _waypointPath.GetWayPointSQP(_targetWaypointIndex); // Get the previous waypoint
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex); // Get the next waypoint
        _targetWaypoint = _waypointPath.GetWayPointSQP(_targetWaypointIndex); // Get the target waypoint

        _elapseTime = 0;

        // Calculate the distance between the previous and the target waypoint
        float distanceToWaypoint = Vector3.Distance(_previusWaypoint.position, _targetWaypoint.position); 
        _timeToWaypoint = distanceToWaypoint / _speed;// Calculate the time to reach the target waypoint
    }

    private void OnTriggerEnter(Collider other) // When the player enters the platform
    {
        other.transform.SetParent(transform); // Set the player as a child of the platform

        // Desactivar el script de control de la cámara
        Camera playerCamera = other.GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            MonoBehaviour cameraControllerSak = playerCamera.GetComponent<MonoBehaviour>(); // Reemplaza con el nombre del script de control de la cámara
            if (cameraControllerSak != null)
            {
                cameraControllerSak.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other) // When the player exits the platform
    {
        other.transform.SetParent(null); // Remove the player as a child of the platform

        // Reactivar el script de control de la cámara
        Camera playerCamera = other.GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            MonoBehaviour cameraControllerSak = playerCamera.GetComponent<MonoBehaviour>(); // Reemplaza con el nombre del script de control de la cámara
            if (cameraControllerSak != null)
            {
                cameraControllerSak.enabled = true;
            }
        }
    }
}
