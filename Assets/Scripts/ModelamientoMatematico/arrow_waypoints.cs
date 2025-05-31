using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_waypoints : MonoBehaviour
{
    public Transform target;// El objetivo al que apunta la flecha
    public float arrowspeed = 5f;
    private bool flechaActiva = false;

    private int destino = 5; // Distancia a la que se considera que el jugador ha llegado al destino

    void Update()
    {
        if (target == null) return;

        float distancia = Vector3.Distance(transform.position, target.position);

        // Si está cerca del destino, oculta la flecha
        if (distancia < destino)
        {
            if (flechaActiva)
                DesactivarFlecha();
            return;
        }
        else
        {
            // Si se aleja del destino, muestra la flecha
            if (!flechaActiva)
                ActivarFlecha();
        }

        if (!flechaActiva) return;

        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up); //
        transform.rotation = rotation;
    }

    public void ActivarFlecha()
    {
        flechaActiva = true;
        gameObject.SetActive(true);
    }

    public void DesactivarFlecha()
    {
        flechaActiva = false;
        gameObject.SetActive(false);
        
    }
}
