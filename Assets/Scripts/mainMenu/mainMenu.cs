using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    public Slider progressBar;
    public float loadTime = 3f; // Tiempo en segundos antes de iniciar el juego

    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        float timer = 0f;
        while (timer < loadTime)
        {
            timer += Time.deltaTime;
            progressBar.value = timer / loadTime; // Actualiza la barra de progreso
            yield return null;
        }

        SceneManager.LoadScene("GameProject"); // Carga el juego después de la espera
    }
}
