using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Puntuación y Combos")]
    public int score = 0;
    public int comboStreak = 0;
    private float comboTimer = 0f;
    public float comboMaxTime = 3f; // Tiempo límite para mantener el combo

    [Header("Sistema de Oleadas")]
    public int currentWave = 1;
    public int zombiesToSpawnThisWave = 5;
    [HideInInspector] public int zombiesAlive = 0;

    [Header("Referencias de Interfaz (UI)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
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
        // Temporizador para romper el combo si tardas en disparar al siguiente
        if (comboStreak > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                comboStreak = 0;
                UpdateUI();
                Debug.Log("¡Combo perdido!");
            }
        }
    }

    public void AddScore()
    {
        comboStreak++;
        comboTimer = comboMaxTime; // Reinicia el cronómetro

        int pointsToAdd = 100 * comboStreak; // Multiplicador por racha
        score += pointsToAdd;

        UpdateUI();
    }

    public void ZombieKilled()
    {
        zombiesAlive--;
        AddScore();

        if (zombiesToSpawnThisWave <= 0 && zombiesAlive <= 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWave++;
        zombiesToSpawnThisWave = currentWave * 5; 
        UpdateUI();
        Debug.Log($"¡=== OLEADA {currentWave} INICIADA ===!");
    }

    // Método para refrescar los textos en la pantalla
    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Puntaje: " + score;
        
        if (comboText != null) 
        {
            if (comboStreak > 1) comboText.text = "¡Racha x" + comboStreak + "!";
            else comboText.text = ""; // Si no hay racha activa, se oculta
        }

        if (waveText != null) waveText.text = "Oleada: " + currentWave;
    }
}