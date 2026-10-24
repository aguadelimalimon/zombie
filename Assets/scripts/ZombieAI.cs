using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    public int health = 50;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // --- VELOCIDAD ALEATORIA ---
        // Cada zombie tendrá una velocidad distinta entre 2.5 y 5.5
        agent.speed = Random.Range(2.5f, 5.5f);

        // Busca al jugador automáticamente por su Tag y guarda su Transform
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform; // <-- Corregido aquí para evitar el error de tipos
        }
    }

    void Update()
    {
        // Si encuentra al jugador, se dirige hacia él usando el NavMesh
        if (player != null && agent != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            // Avisamos al GameManager si ya lo creaste para sumar puntos y racha
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ZombieKilled();
            }
            
            Destroy(gameObject);
        }
    }
}