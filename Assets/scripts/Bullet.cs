using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;    // Velocidad a la que vuela la bala
    public int damage = 25;      // Daño al zombie
    public float lifetime = 3f;  // Segundos antes de destruirse si no choca con nada
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Si tiene Rigidbody, la impulsamos con física
        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }
        
        // Destruir la bala después de un tiempo para no saturar memoria
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Si por alguna razón no tiene Rigidbody, la movemos manualmente hacia adelante
        if (rb == null)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si choca con un Zombie, le hace daño
        if (collision.gameObject.CompareTag("Zombie"))
        {
            ZombieAI zombie = collision.gameObject.GetComponent<ZombieAI>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        // Si choca con otra cosa que no sea el jugador, se destruye
        else if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}