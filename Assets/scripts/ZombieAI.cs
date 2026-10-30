
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Estadísticas del Zombie")]
    public int maxHealth = 50;
    private int currentHealth;
    public int damageToPlayer = 10;

    [Header("Configuración de Ataque")]
    public float attackRange = 2.0f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    [Header("Velocidad por Oleadas")]
    public float baseSpeed = 3.5f;
    public float speedIncreasePerWave = 0.3f;

    [Header("Animación")]
    public float baseAnimationSpeed = 1.5f;
    public float animationIncreasePerWave = 0.1f;

    private NavMeshAgent agent;
    private Transform player;
    private bool isDead = false;

    private Animator animator;

    private float updatePathTimer = 0f;

    private int currentWave = 1;

    void Start()
    {
        currentHealth = maxHealth;

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();

        // Velocidad inicial
        UpdateZombieSpeed();

        // Buscar al jugador
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        // Ataque
        if (distanceToPlayer <= attackRange)
        {
            agent.SetDestination(transform.position);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            // Persecución
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

    // ACTUALIZAR VELOCIDAD SEGÚN LA OLEADA
    public void SetWave(int wave)
    {
        currentWave = Mathf.Max(1, wave);

        UpdateZombieSpeed();
    }

    void UpdateZombieSpeed()
    {
        // Aumentar velocidad de movimiento
        float newSpeed =
            baseSpeed + (currentWave - 1) * speedIncreasePerWave;

        if (agent != null)
        {
            agent.speed = newSpeed;
        }

        // Aumentar velocidad de animación
        float velocidadAnimacion =
            baseAnimationSpeed +
            (currentWave - 1) * animationIncreasePerWave;

        if (animator != null)
        {
            animator.speed = velocidadAnimacion;
        }

        Debug.Log(
            "Zombie - Oleada: " + currentWave +
            " | Velocidad: " + newSpeed +
            " | Animación: " + velocidadAnimacion
        );
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageToPlayer);

            Debug.Log(
                "¡El zombie te ha golpeado! Daño: " +
                damageToPlayer
            );
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log(
            "Zombie atacado. Vida restante: " +
            currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.ZombieKilled();
        }

        Destroy(gameObject);
    }
}
