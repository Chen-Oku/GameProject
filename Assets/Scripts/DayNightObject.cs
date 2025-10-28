using UnityEngine;

public class DayNightObject : MonoBehaviour
{
    public Material dayMaterial;
    public Material nightMaterial;

    private Renderer rend;

    void Awake()
    {
        // Busca el Renderer en este objeto o en hijos
        rend = GetComponent<Renderer>();
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();
    }

    void OnEnable()
    {
        DayNightCycle.OnDayNightChanged += SetMaterial;
        // Aplica el material correcto al habilitar el objeto
        if (DayNightCycle.Instance != null)
            SetMaterial(DayNightCycle.Instance.IsNight);
    }

    void OnDisable()
    {
        DayNightCycle.OnDayNightChanged -= SetMaterial;
    }

    void SetMaterial(bool isNight)
    {
        if (rend != null)
        {
            rend.material = isNight ? nightMaterial : dayMaterial;
        }
    }
}
