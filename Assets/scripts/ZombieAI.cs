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

    // VARIABLE NUEVA PARA EVITAR EL BUG DE MULTIPLE DAÑO
    private float lastDamageTime = 0f; 

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        UpdateZombieSpeed();

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

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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

    public void SetWave(int wave)
    {
        currentWave = Mathf.Max(1, wave);
        UpdateZombieSpeed();
    }

    void UpdateZombieSpeed()
    {
        float newSpeed = baseSpeed + (currentWave - 1) * speedIncreasePerWave;
        if (agent != null) agent.speed = newSpeed;

        float velocidadAnimacion = baseAnimationSpeed + (currentWave - 1) * animationIncreasePerWave;
        if (animator != null) animator.speed = velocidadAnimacion;
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageToPlayer);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // --- PARCHE ANTIBUGS: I-FRAMES ---
        // Si el zombie recibió daño hace menos de 0.1 segundos, ignora los choques extra de la misma bala.
        if (Time.time < lastDamageTime + 0.1f) return;
        
        // Si es un daño válido, actualizamos el cronómetro
        lastDamageTime = Time.time;
        // ---------------------------------

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

        if (GameManager.instance != null)
        {
            GameManager.instance.ZombieKilled();
        }

        Destroy(gameObject);
    }
}