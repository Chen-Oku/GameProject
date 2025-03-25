/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Level Finished");
        }
    }
}*/


/*using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public GameObject winMenuUI; // Objeto de UI para mostrar el menú de "Ganaste"

    void Start()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(false); // Asegurarse de que el menú esté oculto al inicio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowWinMenu();
        }
    }

    void ShowWinMenu()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(true); // Mostrar el menú de "Ganaste"
        }
        Time.timeScale = 0f; // Detener el tiempo del juego
        Debug.Log("Level Finished");
    }
}*/

/*using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public GameObject winMenuUI; // Objeto de UI para mostrar el menú de "Ganaste"

    void Start()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(false); // Asegurarse de que el menú esté oculto al inicio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowWinMenu();
        }
    }

    void ShowWinMenu()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(true); // Mostrar el menú de "Ganaste"
        }
        Time.timeScale = 0f; // Detener el tiempo del juego
        Debug.Log("Level Finished");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Restablecer el tiempo del juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit(); // Salir del juego
    }
}

*/

/*using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public GameObject winMenuUI; // Objeto de UI para mostrar el menú de "Ganaste"
    public float displayTime = 3f; // Tiempo que se mostrará el menú antes de reiniciar

    void Start()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(false); // Asegurarse de que el menú esté oculto al inicio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowWinMenu();
        }
    }

    void ShowWinMenu()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(true); // Mostrar el menú de "Ganaste"
        }
        Time.timeScale = 0f; // Detener el tiempo del juego
        Debug.Log("Level Finished");
        StartCoroutine(RestartLevelAfterDelay());
    }

    IEnumerator RestartLevelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(displayTime); // Esperar el tiempo especificado
        Time.timeScale = 1f; // Restablecer el tiempo del juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1); // Reiniciar la escena actual
    }
}
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public GameObject winMenuUI; // Objeto de UI para mostrar el menú de "Ganaste"
    

    void Start()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(false); // Asegurarse de que el menú esté oculto al inicio

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowWinMenu();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GameProject()
    {
        SceneManager.LoadScene(1);
    }

    void ShowWinMenu()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(true); // Mostrar el menú de "Ganaste"
        }
        Time.timeScale = 0f; // Detener el tiempo del juego
        Debug.Log("Level Finished");
       // StartCoroutine(LoadPreviousLevelAfterDelay());
    }

    /*IEnumerator LoadPreviousLevelAfterDelay()
    {
        Time.timeScale = 1f; // Restablecer el tiempo del juego
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex); // Cargar la escena anterior
        }
        else
        {
            Debug.LogWarning("No previous scene to load.");
        }
    }*/
}