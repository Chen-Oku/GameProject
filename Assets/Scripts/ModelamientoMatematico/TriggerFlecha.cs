using UnityEngine;
using System.Collections;

public class TriggerFlecha : MonoBehaviour
{
    public arrow_waypoints flecha; // Asigna la referencia en el inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(WaitForSec());
            
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        if (flecha != null)
                flecha.ActivarFlecha();
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (flecha != null)
                flecha.DesactivarFlecha();
        }
    }
}
