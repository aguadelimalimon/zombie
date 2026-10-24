using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 25; // Daño que hace la bala

    void OnCollisionEnter(Collision collision)
    {
        // 1. Si choca con un zombie, buscamos su script para restarle vida
        if (collision.gameObject.CompareTag("Zombie"))
        {
            ZombieAI zombie = collision.gameObject.GetComponent<ZombieAI>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
        }

        // 2. Independientemente de si es una pared, el suelo o un zombie, la bala se destruye al chocar
        Destroy(gameObject);
    }
}