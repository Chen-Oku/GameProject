using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    [SerializeField] private Animation doorAnimation;

    private bool isOpen = false;

    private void Start()
    {
        if (doorAnimation == null)
        {
            doorAnimation = GetComponent<Animation>();
            if (doorAnimation == null)
            {
                Debug.LogError("No se encontró el componente Animation en " + gameObject.name);
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
        if (!isOpen && doorAnimation != null)
        {
            doorAnimation.CrossFade("DoorOpen", 0.1f);
            isOpen = true;
            Debug.Log("Door opened."); // Log para verificar la apertura de la puerta
        }
    }

    public void Close()
    {
        if (isOpen && doorAnimation != null)
        {
            doorAnimation.Play("Close");
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
