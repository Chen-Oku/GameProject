using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerSak : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Vector3 offset = new Vector3(0, 2, -4); // Desplazamiento de la cámara respecto al jugador
    public float sensitivity = 5.0f; // Sensibilidad del ratón
    public float yMinLimit = -20f; // Límite mínimo de rotación vertical
    public float yMaxLimit = 80f; // Límite máximo de rotación vertical

    private float x;
    private float y;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!PauseMenu.isPaused)
        {
            x += Input.GetAxis("Mouse X") * sensitivity;
            y -= Input.GetAxis("Mouse Y") * sensitivity;
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            transform.position = player.position + rotation * offset;
            transform.rotation = rotation;

            // Actualizar la rotación del jugador para que mire en la misma dirección que la cámara
            player.rotation = Quaternion.Euler(0, x, 0);
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}

