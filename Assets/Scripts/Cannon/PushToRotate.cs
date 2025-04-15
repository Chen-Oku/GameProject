using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushToRotate : MonoBehaviour
{
    public float rotateAmount = 15f;  // Grados a rotar cada vez
    public Transform cannon;          // Referencia al Transform del cañón
    public Vector3 rotationAxis = Vector3.up; // Eje de rotación (por defecto, el eje Y)

    private bool canActivate = true;

    private void OnTriggerEnter(Collider other)
    {
        if (canActivate && other.CompareTag("Player"))
        {
            RotateOnce(rotateAmount);
            canActivate = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canActivate = true;
        }
    }

    // Rota el cañón una vez la cantidad indicada
    public void RotateOnce(float degrees)
    {
        if (cannon != null)
        {
            cannon.Rotate(rotationAxis * degrees);
        }
    }
}
