using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;
    public float lifetime = 10f; // Tiempo de vida del PowerUp en segundos

    private void Start()
    {
        // Iniciar la corrutina para destruir el PowerUp después de un tiempo
        StartCoroutine(DestroyAfterTime(lifetime));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider collision)
    {
        Destroy(gameObject);
        powerUpEffect.Apply(collision.gameObject);
    }
}
