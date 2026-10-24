using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Arrastra aquí tu prefab del zombie
    public float spawnInterval = 3f; // Tiempo entre cada zombie (en segundos)
    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnZombie();
            spawnTimer = 0f; // Reinicia el cronómetro
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab != null)
        {
            // Instancia el zombie en la posición exacta de este objeto Spawner
            Instantiate(zombiePrefab, transform.position, transform.rotation);
        }
    }
}