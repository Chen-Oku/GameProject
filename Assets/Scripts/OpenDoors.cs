using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;

    private bool isOpen = false;

    private void Start()
    {
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
            if (doorAnimator == null)
            {
                Debug.LogError("No se encontró el componente Animator en " + gameObject.name);
            }
        }
    }

    public void Interact()
    {
        if (!isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        if (!isOpen && doorAnimator != null)
        {
            doorAnimator.SetTrigger("IsOpen");
            isOpen = true;
            Debug.Log("Door opened."); // Log para verificar la apertura de la puerta
        }
    }

    public void Close()
    {
        if (isOpen && doorAnimator != null)
        {
            doorAnimator.SetTrigger("IsClose");
            isOpen = false;
            Debug.Log("Door closed."); // Log para verificar el cierre de la puerta
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