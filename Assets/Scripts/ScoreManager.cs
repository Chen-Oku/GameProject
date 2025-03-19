using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score;

    public TextMeshProUGUI scoreText; // Referencia al TextMeshProUGUI para mostrar el puntaje

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Asegurarse de que el ScoreManager no se destruya al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText(); // Actualizar el texto del puntaje al inicio
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText(); // Actualizar el texto del puntaje
    }

    public int GetScore()
    {
        return score;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // Mostrar solo el valor del puntaje
        }
        else
        {
            Debug.LogError("No se ha asignado el TextMeshProUGUI para mostrar el puntaje.");
        }
    }
}
