using UnityEngine;

/// <summary>
/// Bala disparada por enemigos. Daña al jugador pero ignora a otros enemigos.
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float damage = 10f;
    public float lifetime = 3f;
    public float hitRadius = 0.3f;

    void Start()
    {
        // Destruir después de un tiempo
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Buscar al jugador cercano
        CheckHitPlayer();
    }

    void CheckHitPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, hitRadius);
        
        foreach (Collider2D hit in hits)
        {
            // Solo dañar al jugador
            if (hit.CompareTag("Player"))
            {
                // Buscar componente de vida del jugador
                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log("Jugador recibió " + damage + " de daño!");
                }
                else
                {
                    Debug.Log("Bala enemiga golpeó al jugador (sin PlayerHealth)");
                }
                
                Destroy(gameObject);
                return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignorar enemigos
        if (other.CompareTag("Enemy")) return;
        
        // Dañar al jugador
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }
        
        // Destruir si golpea paredes u obstáculos
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignorar enemigos
        if (collision.gameObject.CompareTag("Enemy")) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
