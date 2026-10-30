using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Lista de Mapas del Juego")]
    // Escribe aquí los nombres exactos de las escenas de tus mapas
    public string[] mapSceneNames = { "SampleScene", "Map_Warehouse", "Map_Lab" };

    // --- ¡AQUÍ ESTÁ LA MAGIA PARA EL CURSOR! ---
    void Start()
    {
        // Aseguramos que el tiempo corra normal
        Time.timeScale = 1f;

        // Desbloqueamos y mostramos el cursor obligatoriamente al entrar al menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // --------------------------------------------

    // 1. Botón Jugar (Elige un mapa al azar de la lista)
    public void PlayRandomMap()
    {
        if (mapSceneNames.Length == 0)
        {
            Debug.LogWarning("⚠️ No hay mapas configurados en la lista.");
            return;
        }

        // Elige un índice aleatorio basado en cuántos mapas tengas en la lista
        int randomIndex = Random.Range(0, mapSceneNames.Length);
        string selectedMap = mapSceneNames[randomIndex];

        Debug.Log("🎲 [MAPA ALEATORIO] Cargando: " + selectedMap);
        SceneManager.LoadScene(selectedMap);
    }

    // 2. Selector de Mapas (Carga el mapa específico que elijas)
    public void LoadMap(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }

    // 3. Botón Salir del Juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}