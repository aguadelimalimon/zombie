using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    [Header("Salud y Dificultad Extrema")]
    public int baseHealth = 60; 
    private int currentHealth;
    public float baseSpeed = 3.5f;

    [Header("Ataque al Player")]
    public float attackRange = 2.0f;
    public int baseAttackDamage = 15; 
    public float attackCooldown = 1.0f;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // ESCALADO IMPLACABLE PARA LA FERIA
        if (GameManager.Instance != null)
        {
            int wave = GameManager.Instance.currentWave;
            
            // La vida sube drásticamente (en la oleada 40 serán bestias imposibles de matar rápido)
            currentHealth = baseHealth + (wave * 50);
            
            // Los zombies se vuelven cada vez más veloces conforme sube la oleada (con un tope para que no se rompan)
            agent.speed = Mathf.Min(baseSpeed + (wave * 0.12f), 7.5f);
        }
        else
        {
            currentHealth = baseHealth;
        }
    }

    void Update()
    {
        if (player != null && agent != null)
        {
            agent.SetDestination(player.position);

            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // El daño de los zombies aumenta en cada oleada; en rondas altas te matarán de 2 o 3 golpes
            int scaledDamage = baseAttackDamage + (GameManager.Instance != null ? GameManager.Instance.currentWave * 2 : 0);
            playerHealth.TakeDamage(scaledDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.zombiesAlive--;
            // Damos 150 puntos por baja para que llegar a 50,000 requiera sobrevivir muchas rondas
            GameManager.Instance.AddScore(150); 
        }
        Destroy(gameObject);
    }
}