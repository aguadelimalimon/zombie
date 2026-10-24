using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Necesario para usar TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI healthText; // Arrastra aquí tu texto de la vida

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("¡Te han atacado! Vida actual: " + currentHealth);
        
        UpdateHealthUI(); // Actualiza el texto cada vez que recibes daño

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
        Debug.Log("¡GAME OVER!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}