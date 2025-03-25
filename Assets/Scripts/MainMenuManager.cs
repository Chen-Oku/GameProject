using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "GameProject"; // Nombre de la escena del juego
    public GameObject loadingScreen; // Objeto de UI para la pantalla de carga
    public Slider loadingBar; // Barra de carga
    public float displayTime = 5f; // Tiempo que se mostrará la imagen y la barra de carga antes de iniciar el juego

    void Start()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true); // Asegurarse de que la pantalla de carga esté visible al inicio
        }
        StartCoroutine(DisplayLoadingScreen());
    }

    IEnumerator DisplayLoadingScreen()
    {
        yield return new WaitForSeconds(displayTime); // Esperar el tiempo especificado
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Single);
        operation.allowSceneActivation = false; // No activar la escena automáticamente

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Calcular el progreso de carga
            if (loadingBar != null)
            {
                loadingBar.value = progress; // Actualizar la barra de carga
            }

            // Activar la escena cuando esté completamente cargada
            if (operation.progress >= 0.9f)
            {
                loadingBar.value = 1f; // Asegurarse de que la barra de carga esté llena
                operation.allowSceneActivation = true; // Activar la escena
            }

            yield return null;
        }
    }
}
