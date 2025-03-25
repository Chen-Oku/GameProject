using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public GameObject menuPrincipal;  // Arrastra aquí el GameObject "MenuPrincipal" en el Inspector
    public GameObject menuGanaste;    // Arrastra aquí el GameObject "MenuGanaste" en el Inspector
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " POINTS";
    }

    public void ReturnToMenu()
    {
        menuGanaste.SetActive(false);  // Oculta la pantalla de Victoria
        menuPrincipal.SetActive(true); // Muestra el menú principal
    }
}
