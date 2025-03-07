using UnityEngine;

public class CanvasLookCamera : MonoBehaviour
{
    public Camera mainCamera; // Referencia a la cámara principal

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Asigna la cámara principal si no se ha asignado una específica
        }
    }

    void Update()
    {
        // Hace que el canvas mire hacia la cámara
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}
