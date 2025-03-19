using UnityEngine;

public class CanvasLookCamera : MonoBehaviour
{
    private Camera mainCamera; // Referencia a la c�mara principal

    void Start()
    {
        if (mainCamera == null)
        {
            // Buscar la c�mara con el tag "MainCamera"
            GameObject cameraObject = GameObject.FindWithTag("MainCamera");
            if (cameraObject != null)
            {
                mainCamera = cameraObject.GetComponent<Camera>();
            }
        }

        // Verificar si la c�mara principal se ha asignado correctamente
        if (mainCamera == null)
        {
            Debug.LogError("No se encontr� una c�mara principal. Aseg�rate de que haya una c�mara en la escena con el tag 'MainCamera'.");
        }
    }

    void Update()
    {
        if (mainCamera == null) return; // Salir del m�todo si mainCamera es null

        // Hace que el canvas mire hacia la c�mara
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}