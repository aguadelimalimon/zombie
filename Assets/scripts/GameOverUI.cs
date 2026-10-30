using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Referencias de Texto (UI)")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalWaveText;
    public TextMeshProUGUI highScoreText;

    public void SetupGameOver(int finalScore, int finalWave)
    {
        Debug.Log("🎯 SetupGameOver llamado con Puntaje: " + finalScore + " y Oleada: " + finalWave);

        // Asignamos el puntaje
        if (finalScoreText != null)
        {
            finalScoreText.text = "Puntaje: " + finalScore;
        }
        else
        {
            Debug.LogError("❌ Falta arrastrar el texto 'FinalScoreText' en el Inspector del GameOverUI.");
        }

        // Asignamos la oleada
        if (finalWaveText != null)
        {
            finalWaveText.text = "Oleadas: " + finalWave;
        }
        else
        {
            Debug.LogError("❌ Falta arrastrar el texto 'FinalWaveText' en el Inspector del GameOverUI.");
        }

        // Sistema de Récord Histórico
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScore > highScore)
        {
            highScore = finalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            Debug.Log("🏆 ¡NUEVO RÉCORD GUARDADO: " + highScore + "!");
        }

        if (highScoreText != null)
        {
            highScoreText.text = "Récord: " + highScore;
        }
    }

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