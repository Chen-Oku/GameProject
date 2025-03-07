using UnityEngine;

public class CanvasLookCamera : MonoBehaviour
{
    public Camera mainCamera; // Referencia a la c�mara principal

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Asigna la c�mara principal si no se ha asignado una espec�fica
        }
    }

    void Update()
    {
        // Hace que el canvas mire hacia la c�mara
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}
