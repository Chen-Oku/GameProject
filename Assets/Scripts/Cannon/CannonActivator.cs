using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonActivator : MonoBehaviour
{
    public Transform activator; // The object that moves down when activated
    public float activationDistance = 1f; // Distance the activator moves down
    public float activationSpeed = 2f; // Speed of the activator movement
    public List<CannonManager> cannonManagers; // List of references to CannonManager scripts
    public float cooldownTime = 2f; // Time to wait before the activator can be used again

    private Vector3 originalPosition; // Original position of the activator
    private bool isActivated = false; // Whether the activator is currently activated
    private bool canActivate = true; // Whether the activator can be used

    void Start()
    {
        originalPosition = activator.position; // Store the original position of the activator
    }

    void Update()
    {
        if (isActivated)
        {
            // Move the activator down
            activator.position = Vector3.Lerp(activator.position, originalPosition - new Vector3(0, activationDistance, 0), Time.deltaTime * activationSpeed);
        }
        else
        {
            // Move the activator back to its original position
            activator.position = Vector3.Lerp(activator.position, originalPosition, Time.deltaTime * activationSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canActivate)
        {
            isActivated = true;
            canActivate = false; // Prevent reactivation until released
            foreach (CannonManager cannonManager in cannonManagers)
            {
                cannonManager.FireCannon(); // Trigger the CannonManager to fire the projectile
            }
            print("CannonActivator: Cannon fired");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActivated = false;
            StartCoroutine(CooldownCoroutine()); // Start the cooldown coroutine
        }
    }

    IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime); // Wait for the cooldown time
        canActivate = true; // Allow reactivation after the cooldown
    }
}







