using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject zombiePrefab;      // Arrastra aquí el Prefab de tu Zombie
    public Transform[] spawnPoints;      // Arrastra aquí los objetos vacíos hijos (puntos de aparición)
    public float spawnInterval = 2.5f;   // Tiempo entre cada aparición

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Si el GameManager existe y aún faltan zombies por generar en esta oleada
            if (GameManager.instance != null && GameManager.instance.zombiesToSpawnThisWave > 0)
            {
                SpawnZombie();
            }
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab == null || spawnPoints.Length == 0) return;

        // Elige un punto de spawn al azar de la lista
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomIndex];

        // Instancia el zombie en el punto elegido
        Instantiate(zombiePrefab, selectedPoint.position, selectedPoint.rotation);

        // ¡AQUÍ VA LA FUNCIÓN CLAVE!: Le avisamos al GameManager que nació un zombie
        if (GameManager.instance != null)
        {
            GameManager.instance.ZombieSpawned();
        }
    }
}