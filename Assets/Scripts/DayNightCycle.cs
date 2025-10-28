using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Day/Night Settings")]
    public Light sun; // Asigna tu luz direccional aquí
    public float dayDuration = 60f; // Duración de un día en segundos

    private float timeOfDay = 0f; // 0 a 1, donde 0 es medianoche y 0.5 es mediodía

    [Header("Scene Elements")]
    [Range(0f, 1f)]
    public float nightThreshold = 0.2f; // Cuando la intensidad baja de esto, es noche

    private bool isNight = false;
    public static DayNightCycle Instance { get; private set; }
    public static event Action<bool> OnDayNightChanged;

    public bool IsNight => isNight;

    // Evento para notificar cambio de día/noche
    public static event Action<float> OnDayNightBlend;

    public float CurrentBlend { get; private set; } // 0=dia, 1=noche

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Avanza el tiempo del día
        timeOfDay += (Time.deltaTime / dayDuration);
        if (timeOfDay > 1f) timeOfDay -= 1f;

        // Rota el sol (360 grados en un día)
        float sunAngle = timeOfDay * 360f - 90f; // -90 para que 0 sea medianoche
        if (sun != null)
        {
            sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

            // Ajusta la intensidad para simular día y noche
            sun.intensity = Mathf.Clamp01(Mathf.Cos(timeOfDay * Mathf.PI * 2f) * 0.5f + 0.5f);
        }

        // Calcula el blend (0=dia, 1=noche) usando la intensidad del sol
        float blend = 1f - Mathf.Clamp01(sun != null ? sun.intensity : 1f);
        CurrentBlend = blend;
        OnDayNightBlend?.Invoke(blend);

        // Detecta cambio de día a noche y viceversa
        bool nowNight = sun != null && sun.intensity < nightThreshold;
        if (nowNight != isNight)
        {
            isNight = nowNight;
            OnDayNightChanged?.Invoke(isNight);
        }
    }
}
