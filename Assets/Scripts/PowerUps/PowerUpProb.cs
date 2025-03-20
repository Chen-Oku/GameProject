using System.Collections;
using UnityEngine;

public class PowerUpProb : MonoBehaviour
{
    public static PowerUpProb instance;

    public GameObject minorHealthBuffPrefab;
    public GameObject majorHealthBuffPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
           // Destroy(gameObject);
        }
    }

    public void DropPowerUp(Vector3 position)
    {
        float dropChance = Random.Range(0f, 1f); // Probabilidad de soltar un PowerUP

        if (dropChance <= 0.35f) // 50% de probabilidad de soltar un PowerUP
        {
            GameObject powerUpPrefab = Random.Range(0, 2) == 0 ? minorHealthBuffPrefab : majorHealthBuffPrefab;
            Instantiate(powerUpPrefab, position, Quaternion.identity);
        }
    }
}

