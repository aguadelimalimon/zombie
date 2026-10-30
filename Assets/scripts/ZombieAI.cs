using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Estadísticas del Zombie")]
    public int maxHealth = 50;
    private int currentHealth;
    public int damageToPlayer = 10;

    [Header("Configuración de Ataque")]
    public float attackRange = 2.0f;     // Distancia a la que el zombie ataca
    public float attackCooldown = 1.5f;  // Tiempo de espera entre cada golpe
    private float lastAttackTime = 0f;

    private NavMeshAgent agent;
    private Transform player;
    private bool isDead = false;

    // Temporizador para optimizar el NavMesh
    private float updatePathTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

        // Busca automáticamente al jugador por su Tag ("Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        // Calculamos la distancia entre el zombie y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si está en rango de ataque, se detiene y golpea al jugador
        if (distanceToPlayer <= attackRange)
        {
            agent.SetDestination(transform.position); // Detiene el movimiento para golpear

            // Si ya pasó el tiempo de espera, ataca
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            // Si está lejos, lo persigue (optimizando el cálculo de ruta cada 0.2 segundos)
            updatePathTimer += Time.deltaTime;
            if (updatePathTimer >= 0.2f)
            {
                if (agent != null && agent.enabled)
                {
                    agent.SetDestination(player.position);
                }
                updatePathTimer = 0f;
            }
        }
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageToPlayer);
            Debug.Log("💥 ¡El zombie te ha golpeado! Daño: " + damageToPlayer);
        }
    }

    // Método para recibir daño (cuando le disparas)
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Zombie atacado. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Le avisamos al GameManager que este zombie murió
        if (GameManager.instance != null)
        {
            GameManager.instance.ZombieKilled();
        }

        Destroy(gameObject);
    }
}