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
        
        // Busca al jugador automáticamente por su Tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
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
            Destroy(gameObject);
        }
    }
}