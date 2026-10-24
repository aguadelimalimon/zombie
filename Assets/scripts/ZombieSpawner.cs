using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Arrastra aquí tu prefab del zombie
    public float spawnInterval = 3f; // Tiempo entre cada zombie (en segundos)
    private float spawnTimer;

    void Update()
    {
        // Solo genera zombies si el GameManager existe y aún quedan por salir en esta oleada
        if (GameManager.Instance != null && GameManager.Instance.zombiesToSpawnThisWave > 0)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnZombie();
                spawnTimer = 0f;
            }
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab != null && GameManager.Instance != null)
        {
            // Instancia el zombie en la posición del spawner
            Instantiate(zombiePrefab, transform.position, transform.rotation);

            // Registra el nuevo zombie en el GameManager
            GameManager.Instance.zombiesToSpawnThisWave--;
            GameManager.Instance.zombiesAlive++;
        }
    }
}