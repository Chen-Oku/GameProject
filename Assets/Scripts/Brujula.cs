using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brujula : MonoBehaviour
{
    [Header("Configuración")]
    public Transform objetivo;            // El objeto hacia el cual la brújula apuntará
    public bool seguirSoloHorizontal = true;  // Si verdadero, la brújula solo rotará en el plano XZ
    public float velocidadRotacion = 5f;      // Velocidad a la que la brújula rota hacia el objetivo
    
    [Header("Depuración")]
    public bool mostrarLineas = true;         // Muestra líneas de debug en el editor
    
    void Update()
    {
        if (objetivo == null)
            return;
            
        // Calcular vector dirección desde la brújula hacia el objetivo
        // Fórmula: dirección = posiciónObjetivo - posiciónActual
        Vector3 direccion = objetivo.position - transform.position;
        
        // Si queremos solo orientación horizontal, eliminamos la componente Y
        if (seguirSoloHorizontal)
        {
            direccion.y = 0f;
        }
        
        // Verificar que el vector dirección no sea nulo (caso en que estemos exactamente en el mismo punto)
        if (direccion.magnitude < 0.001f)
            return;
        
        // Normalizar el vector dirección (convertirlo en vector unitario)
        // Fórmula: v̂ = v/|v|
        direccion.Normalize();
        
        // Calcular la rotación que debe tener la brújula para mirar hacia el objetivo
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
        
        // Aplicar la rotación suavemente usando interpolación esférica (Slerp)
        // Fórmula: Slerp(q1, q2, t) interpola entre dos rotaciones
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            rotacionObjetivo, 
            velocidadRotacion * Time.deltaTime
        );
        
        // Dibujar líneas de debug para visualizar el funcionamiento
        if (mostrarLineas && Application.isEditor)
        {
            Debug.DrawLine(transform.position, objetivo.position, Color.red);
            Debug.DrawRay(transform.position, transform.forward * 2f, Color.blue);
        }
    }
    
    // Método para visualizar en el editor
    private void OnDrawGizmosSelected()
    {
        if (objetivo != null)
        {
            // Dibujar una línea desde la brújula hacia el objetivo
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, objetivo.position);
            
            // Dibujar un área alrededor del objetivo
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(objetivo.position, 0.5f);
        }
    }
}
