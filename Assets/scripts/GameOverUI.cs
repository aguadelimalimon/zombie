using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Referencias de Texto (UI)")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalWaveText;
    public TextMeshProUGUI highScoreText;

    // Esta función se llama automáticamente cuando el jugador muere
    public void SetupGameOver(int finalScore, int finalWave)
    {
        // 1. Mostrar puntaje y oleada actual
        if (finalScoreText != null) finalScoreText.text = "Puntaje: " + finalScore;
        if (finalWaveText != null) finalWaveText.text = "Oleadas: " + finalWave;

        // 2. Sistema de Récord (High Score)
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScore > highScore)
        {
            highScore = finalScore;
            PlayerPrefs.SetInt("HighScore", highScore); // Guarda el nuevo récord
            PlayerPrefs.Save();
            Debug.Log("🏆 ¡NUEVO RÉCORD DE PUNTAJE!");
        }

        if (highScoreText != null)
        {
            highScoreText.text = "Récord: " + highScore;
        }
    }

    // Botones del Game Over
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); 
    }
}