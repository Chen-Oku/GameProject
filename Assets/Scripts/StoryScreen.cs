using UnityEngine;
using UnityEngine.UI;

public class StoryScreen : MonoBehaviour
{
    private Camera mainCamera; // Referencia a la cámara principal
    public GameObject storyScreen; // Referencia a la pantalla de historia
    public Transform playerSpawn; // Referencia al punto de spawn del jugador
    public GameObject player; // Referencia al jugador
    public EnemySpawner enemySpawner; // Referencia al EnemySpawner
    //public GolemBoss golemBoss; // Referencia al GolemBoss
    public TutorialTurtle tutorialTurtle; // Referencia al TutorialTurtle

    private void Start()
    {
        // Mostrar la pantalla de historia al inicio
        storyScreen.SetActive(true);
        // Desactivar el jugador al inicio
        player.SetActive(false);

        if (mainCamera == null)
        {
            // Buscar la cámara con el tag "MainCamera"
            GameObject cameraObject = GameObject.FindWithTag("MainCamera");
            if (cameraObject != null)
            {
                mainCamera = cameraObject.GetComponent<Camera>();
            }
        }
    }

    public void StartGame()
    {
        // Ocultar la pantalla de historia
        storyScreen.SetActive(false);
        // Activar el jugador
        player.SetActive(true);
        // Mover al jugador al punto de spawn
        player.transform.position = playerSpawn.position;
        player.transform.rotation = playerSpawn.rotation;
      /*  // Iniciar la generación de enemigos
        enemySpawner.StartGame();
        // Iniciar el TutorialTurtle
        tutorialTurtle.StartGame();*/
    }
}