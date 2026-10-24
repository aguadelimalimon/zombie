using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Puntuación y Oleadas")]
    public int score = 0;
    public int currentWave = 1;
    public int zombiesToSpawnThisWave = 10; // Empezamos con más presión desde la ronda 1
    public int zombiesAlive = 0;

    [Header("UI Textos")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (zombiesToSpawnThisWave <= 0 && zombiesAlive <= 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWave++;
        
        // ESCALADO BRUTAL DE HORDAS: La cantidad de zombies explota por cada oleada
        zombiesToSpawnThisWave = currentWave * 12; 
        
        Debug.Log("=== ¡OLEADA " + currentWave + " (MODO INFERNAL) INICIADA! ===");
        UpdateUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (waveText != null) waveText.text = "Wave: " + currentWave;
    }
}