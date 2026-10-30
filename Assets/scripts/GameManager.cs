using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Estadísticas de Partida")]
    public int score = 0;
    public int currentWave = 1;
    public int zombiesToSpawnThisWave = 10; // Cantidad de zombies que saldrán en esta oleada
    public int zombiesAlive = 0;           // Contador de zombies vivos actualmente en el mapa

    [Header("Referencias de Interfaz (UI)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;

    void Awake()
    {
        // Patrón Singleton global para que otros scripts lo consulten fácilmente
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Si ya se acabaron de generar los zombies de la oleada y murieron todos los vivos, pasamos a la siguiente ronda
        if (zombiesToSpawnThisWave <= 0 && zombiesAlive <= 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWave++;
        
        // Escalado de dificultad: cada oleada trae más zombies de forma automática
        zombiesToSpawnThisWave = currentWave * 10 + 5; 
        
        Debug.Log("=== ¡OLEADA " + currentWave + " INICIADA! ===");
        UpdateUI();
    }

    // Método para sumar puntos cuando matas a un zombie
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // Llámalo desde tu Spawner cada vez que nazca un zombie nuevo
    public void ZombieSpawned()
    {
        zombiesAlive++;
        zombiesToSpawnThisWave--;
    }

    // Llámalo desde el script de vida del zombie cuando este muera
    public void ZombieKilled()
    {
        zombiesAlive--;
        if (zombiesAlive < 0) zombiesAlive = 0;
        
        // Otorgamos 100 puntos automáticos por cada zombie abatido
        AddScore(100);
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (waveText != null) waveText.text = "Wave: " + currentWave;
    }
}