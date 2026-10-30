using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Interfaz y Menús")]
    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel; // Arrastra aquí tu GameOverPanel de la jerarquía

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
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
        Debug.Log("💀 ¡El jugador ha muerto!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Obtenemos los datos del GameManager asegurándonos de que exista
            int finalScore = 0;
            int finalWave = 1;

            if (GameManager.instance != null)
            {
                finalScore = GameManager.instance.score;
                finalWave = GameManager.instance.currentWave;
                Debug.Log("📊 Datos extraídos del GameManager -> Puntos: " + finalScore + " | Oleada: " + finalWave);
            }
            else
            {
                Debug.LogError("❌ ¡ERROR CRÍTICO: GameManager.instance es NULL! No hay GameManager en esta escena.");
            }

            // Buscamos el script GameOverUI en el panel
            GameOverUI gameOverScript = gameOverPanel.GetComponent<GameOverUI>();
            if (gameOverScript != null)
            {
                gameOverScript.SetupGameOver(finalScore, finalWave);
            }
            else
            {
                Debug.LogError("❌ ¡El GameOverPanel NO tiene adjunto el script GameOverUI!");
            }
        }
        else
        {
            Debug.LogError("❌ ¡El GameOverPanel está vacío en el Inspector del Player!");
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}