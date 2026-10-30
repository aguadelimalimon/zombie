using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Interfaz y Menús")]
    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel; // Arrastra aquí tu GameOverPanel desde la jerarquía

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Asegura que empiece oculto
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
    }

    void GameOver()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("¡GAME OVER ACTIVADO!");

        if (gameOverPanel != null)
        {
            // 1. Mostramos el panel de Game Over
            gameOverPanel.SetActive(true);

            // 2. Extraemos el puntaje y la oleada del GameManager
            int finalScore = 0;
            int finalWave = 1;

            if (GameManager.instance != null)
            {
                finalScore = GameManager.instance.score;
                finalWave = GameManager.instance.currentWave;
            }

            // 3. Pasamos los datos al script GameOverUI para que los pinte en pantalla
            GameOverUI gameOverScript = gameOverPanel.GetComponent<GameOverUI>();
            if (gameOverScript != null)
            {
                gameOverScript.SetupGameOver(finalScore, finalWave);
            }
        }

        // 4. Congelamos el juego por completo y liberamos el cursor para los botones
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}