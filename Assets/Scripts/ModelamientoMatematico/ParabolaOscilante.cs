using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaOscilante : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    public float alturaMaxima = 2.0f;     // Altura máxima del salto
    public float distanciaHorizontal = 5.0f;  // Distancia horizontal total del movimiento (ida)
    public float velocidad = 2.0f;        // Velocidad del movimiento completo
    public Vector3 direccionMovimiento = Vector3.right; // Dirección del movimiento

    [Header("Configuración de Oscilación")]
    public bool iniciarMovimiento = true;
    public bool moverIzquierdaDerecha = true; // True: inicia de izquierda a derecha, False: inicia de derecha a izquierda

    // Variables internas
    private Vector3 posicionInicial;
    private Vector3 posicionFinal;
    private float tiempo = 0f;
    private bool moviendoHaciaDelante = true;
    private float duracionMovimiento;

    void Start()
    {
        // Normalizar la dirección del movimiento
        // Fórmula: v̂ = v/|v| (vector unitario)
        direccionMovimiento.Normalize();
        
        // Guardar la posición inicial
        posicionInicial = transform.position;
        
        // Calcular la posición final
        // Fórmula: PosiciónFinal = PosiciónInicial + Dirección * Distancia
        posicionFinal = posicionInicial + direccionMovimiento * distanciaHorizontal;
        
        // Calcular la duración del movimiento completo basado en la velocidad
        // Fórmula: tiempo = distancia/velocidad
        duracionMovimiento = distanciaHorizontal / velocidad;
        
        // Establecer la dirección inicial del movimiento usando trigonometría
        // La fase inicial determina el punto de inicio en el ciclo:
        // phase = 0: inicia en la posición izquierda (t=0)
        // phase = π: inicia en la posición derecha (t=1)
        float phaseInicial = moverIzquierdaDerecha ? 0f : Mathf.PI;
        tiempo = (Mathf.Cos(phaseInicial) + 1f) / 2f; // Normaliza de -1,1 a 0,1
        moviendoHaciaDelante = Mathf.Sin(phaseInicial) >= 0;

        // Si inicia desde la derecha, nos aseguramos que la posición inicial sea correcta
        if (!moverIzquierdaDerecha)
        {
            transform.position = posicionFinal;
        }
    }

    void Update()
    {
        if (!iniciarMovimiento)
            return;

        // Incrementar el tiempo basado en la dirección del movimiento
        // Fórmula: t = t ± Δt/duración (normalización del tiempo entre 0 y 1)
        if (moviendoHaciaDelante)
        {
            tiempo += Time.deltaTime / duracionMovimiento;
            if (tiempo >= 1.0f)
            {
                tiempo = 1.0f;
                moviendoHaciaDelante = false;
            }
        }
        else
        {
            tiempo -= Time.deltaTime / duracionMovimiento;
            if (tiempo <= 0.0f)
            {
                tiempo = 0.0f;
                moviendoHaciaDelante = true;
            }
        }

        // Calcular la posición horizontal interpolando entre la posición inicial y final
        // Fórmula: Lerp(A,B,t) = A + t*(B-A) donde t va de 0 a 1
        Vector3 posicionHorizontal = Vector3.Lerp(posicionInicial, posicionFinal, tiempo);
        
        // Calcular la altura parabólica (máxima en el centro del recorrido)
        // Fórmula: h = hmax * 4 * t * (1-t) 
        // Esta es una función parabólica que vale 0 en t=0 y t=1, y alcanza hmax cuando t=0.5
        float alturaParabola = alturaMaxima * 4.0f * tiempo * (1.0f - tiempo);
        
        // Guardamos posición previa para calcular la dirección de movimiento
        Vector3 posicionPrevia = transform.position;
        
        // Aplicar la nueva posición: movimiento horizontal + altura parabólica
        transform.position = new Vector3(
            posicionHorizontal.x, 
            posicionHorizontal.y + alturaParabola, 
            posicionHorizontal.z
        );
        
        // Calcular la dirección del movimiento
        // Fórmula: dirección = posiciónActual - posiciónAnterior
        Vector3 direccionActual = transform.position - posicionPrevia;
        
        // Solo rotar si hay movimiento significativo
        if (direccionActual.magnitude > 0.001f)
        {
            // Rotar el objeto para mirar en la dirección del movimiento
            // Se utiliza el vector normalizado para orientar el objeto
            transform.forward = direccionActual.normalized;
        }
    }

    // Método opcional para visualizar la trayectoria en el editor
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Vector3 inicio = transform.position;
            Vector3 fin = transform.position + direccionMovimiento.normalized * distanciaHorizontal;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(inicio, fin);
            
            // Dibujar puntos de la parábola para visualizar la trayectoria
            Gizmos.color = Color.cyan;
            int pasos = 20;
            for (int i = 0; i <= pasos; i++)
            {
                // Interpolación lineal para el movimiento horizontal
                // Fórmula: punto = inicio + t*(fin-inicio)
                float t = i / (float)pasos;
                Vector3 punto = Vector3.Lerp(inicio, fin, t);
                
                // Parábola para la altura vertical
                // Fórmula: h = hmax * 4 * t * (1-t)
                float altura = alturaMaxima * 4.0f * t * (1.0f - t);
                punto.y += altura;
                Gizmos.DrawSphere(punto, 0.1f);
            }
        }
    }
}
