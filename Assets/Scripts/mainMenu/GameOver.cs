using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{

    public GameObject menuPrincipal;  // Arrastra aquí el GameObject "MenuPrincipal" en el Inspector
    public GameObject menuFinDeJuego; // Arrastra aquí el GameObject "MenuFinDeJuego" en el Inspector
    public TextMeshProUGUI pointsText;
    public void Setup(int score){
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " POINTS";
    }
    public void ReturnToMenu()
    {
        menuFinDeJuego.SetActive(false);  // Oculta la pantalla de Game Over
        menuPrincipal.SetActive(true); // Muestra el menú principal
    }
}
