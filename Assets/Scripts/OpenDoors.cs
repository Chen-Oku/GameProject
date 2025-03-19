using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    [SerializeField] private Animation doorAnimation;

    private bool isOpen = false;

    public void Interact()
    {
        if (!isOpen)
        {
            doorAnimation.CrossFade("OpenDoors",0.1f);
            isOpen = true;
        }
        else
        {
            doorAnimation.Play("Close");
            isOpen = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone."); // Log para verificar la entrada del jugador
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger zone."); // Log para verificar la salida del jugador
        }
    }
}

