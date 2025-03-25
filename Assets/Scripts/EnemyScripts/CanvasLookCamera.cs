using UnityEngine;

public class CanvasLookCamera : MonoBehaviour
{
    private Camera mainCamera; // Referencia a la cámara principal

    void Start()
    {
        if (mainCamera == null)
        {
            // Buscar la cámara con el tag "MainCamera"
            GameObject cameraObject = GameObject.FindWithTag("MainCamera");
            if (cameraObject != null)
            {
                mainCamera = cameraObject.GetComponent<Camera>();
            }
        }

        // Verificar si la cámara principal se ha asignado correctamente
        if (mainCamera == null)
        {
            Debug.LogError("No se encontró una cámara principal. Asegúrate de que haya una cámara en la escena con el tag 'MainCamera'.");
        }
    }

    void Update()
    {
        if (mainCamera == null) return; // Salir del método si mainCamera es null

        // Hace que el canvas mire hacia la cámara
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}