using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    [SerializeField] private Animation doorAnimation;

    private bool isOpen = false;
    private bool playerIsNear = false; // Tracks if the player is near the door

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            Debug.Log("Player entered the trigger zone."); // Log for testing
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            Debug.Log("Player exited the trigger zone."); // Log for testing
        }
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed and player is near the door."); // Log for testing

            if (!isOpen)
            {
                doorAnimation.Play("Open");
                isOpen = true;
            }
            else
            {
                doorAnimation.Play("Close");
                isOpen = false;
            }
        }
    }
}