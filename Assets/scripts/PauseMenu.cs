using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance; // Patrón Singleton global

    public GameObject pauseMenuUI; 
    
    [Header("Tecla de Pausa")]
    public KeyCode pauseKey = KeyCode.P;
    
    private bool isPaused = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            if (isPaused) Resume();
            return;
        }

        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenuScene"); 
    }
}