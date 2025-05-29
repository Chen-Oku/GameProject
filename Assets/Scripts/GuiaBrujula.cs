/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiaBrujula : MonoBehaviour
{
    public Transform jugador; // Asigna el jugador en el inspector
    public Transform objetivo; // Asigna el objetivo en el inspector
    public GameObject brujulaUI; // Asigna el objeto de la brújula (UI o 3D)
    public float distanciaDesactivacion = 2f; // Distancia para desactivar la brújula

    private bool brujulaActiva = false;
    private float tiempoRestante = 0f; // Tiempo restante de activación

    void Start()
    {
        if (brujulaUI != null)
            brujulaUI.SetActive(false);
    }

    void Update()
    {
        if (brujulaActiva && objetivo != null && jugador != null)
        {
            // Calcula la dirección hacia el objetivo
            Vector3 direccion = objetivo.position - jugador.position;
            direccion.y = 0; // Opcional: ignora la altura

            // Rota la brújula para que apunte hacia el objetivo
            if (brujulaUI != null)
                brujulaUI.transform.forward = direccion.normalized;

            // Desactiva la brújula si el jugador está cerca del objetivo
            if (direccion.magnitude < distanciaDesactivacion)
            {
                brujulaUI.SetActive(false);
                brujulaActiva = false;
                tiempoRestante = 0f;
                return;
            }

            // Desactiva la brújula si se acaba el tiempo
            if (tiempoRestante > 0f)
            {
                tiempoRestante -= Time.deltaTime;
                if (tiempoRestante <= 0f)
                {
                    brujulaUI.SetActive(false);
                    brujulaActiva = false;
                }
            }
        }
    }

    // Activa la brújula por una cantidad de segundos
    public void ActivarBrujula(float segundos)
    {
        if (brujulaUI != null)
            brujulaUI.SetActive(true);
        brujulaActiva = true;
        tiempoRestante = segundos;
    }

    // Método anterior para compatibilidad (activa sin tiempo)
    public void ActivarBrujula()
    {
        ActivarBrujula(Mathf.Infinity);
    }

    // ...puedes agregar OnTriggerEnter en otro script para llamar a ActivarBrujula()...
}
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GuiaBrujula : MonoBehaviour
{
    public Transform target;
    public Camera mainCam; // Asigna la cámara principal en el inspector
    public Canvas canvas; // Si usas UI, asigna el canvas
    public RectTransform flechaUI; // Si usas UI, asigna el RectTransform de la flecha
    public float arrowspeed = 5f;
    public float bordePantalla = 30f; // Margen para el borde

    void Update()
    {
        if (target == null) return;

        // --- Mejora 1: Mostrar/Ocultar flecha solo si el objetivo está fuera de pantalla ---
        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);
        bool isOffscreen = screenPos.z < 0 ||
                           screenPos.x < bordePantalla || screenPos.x > Screen.width - bordePantalla ||
                           screenPos.y < bordePantalla || screenPos.y > Screen.height - bordePantalla;

        if (flechaUI != null)
            flechaUI.gameObject.SetActive(isOffscreen);

        if (isOffscreen)
        {
            // --- Mejora 2: Limitar la flecha al borde de la pantalla ---
            Vector3 dir = (screenPos - new Vector3(Screen.width / 2, Screen.height / 2, screenPos.z)).normalized;
            Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
            Vector2 pos = new Vector2(
                Mathf.Clamp(dir.x * (canvasSize.x / 2 - bordePantalla), -canvasSize.x / 2 + bordePantalla, canvasSize.x / 2 - bordePantalla),
                Mathf.Clamp(dir.y * (canvasSize.y / 2 - bordePantalla), -canvasSize.y / 2 + bordePantalla, canvasSize.y / 2 - bordePantalla)
            );
            flechaUI.anchoredPosition = pos;

            // --- Mejora 3: Rotar la flecha hacia el objetivo ---
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            flechaUI.rotation = Quaternion.Euler(0, 0, angle - 90);

            // --- Mejora 4: Cambiar color/tamaño según distancia ---
            float distancia = Vector3.Distance(transform.position, target.position);
            float scale = Mathf.Clamp(1 + (10f / distancia), 1f, 2f);
            flechaUI.localScale = Vector3.one * scale;
            // Puedes cambiar color usando flechaUI.GetComponent<Image>().color = ...
        }
        else
        {
            // Si el objetivo está en pantalla, puedes ocultar la flecha o hacer otra acción
            if (flechaUI != null)
                flechaUI.gameObject.SetActive(false);
        }
    }

    // --- Mejora 5: Permitir cambiar el objetivo dinámicamente ---
    public void SetTarget(Transform nuevoObjetivo)
    {
        target = nuevoObjetivo;
    }
}