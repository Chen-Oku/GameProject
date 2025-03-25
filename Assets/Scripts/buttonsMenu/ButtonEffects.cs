using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonEffects : MonoBehaviour
{

    private Vector3 originalScale;
    private TextMeshProUGUI buttonText;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        originalColor = buttonText.color;
    }

    public void OnHoverEnter()
    {
        transform.localScale = originalScale * 1.1f; // Agranda el botón
        buttonText.fontSize += 6;  // Aumenta un poco más el tamaño del texto
        buttonText.color = new Color(1f, 0.8f, 1f); // Cambia a rosado más claro

    }

    public void OnHoverExit()
    {
        transform.localScale = originalScale; // Vuelve al tamaño normal
        buttonText.fontSize -= 6; // Vuelve al tamaño original
        buttonText.color = originalColor; // Vuelve al color original
    }
}
