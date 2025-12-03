using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float damage = 10f;
    public float lifetime = 3f;              // Tiempo antes de destruirse
    public LayerMask enemyLayers;
    
    [Header("Hit Detection")]
    public float hitRadius = 0.5f;           // Radio de detección de enemigos

    void Start()
    {
        // Destruir el proyectil después de un tiempo
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Buscar enemigos cercanos constantemente (más fácil de dar)
        CheckNearbyEnemies();
    }

    void CheckNearbyEnemies()
    {
        // Buscar todos los colliders en un radio
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, hitRadius);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player")) continue;
            
            // Verificar si es enemigo por Tag o por componente
            EnemyController enemy = hit.GetComponent<EnemyController>();
            if (enemy != null || hit.CompareTag("Enemy"))
            {
                if (enemy != null)
                {
                    Debug.Log("Proyectil golpeó a enemigo: " + hit.name);
                    enemy.TakeDamage(damage);
                    Debug.Log("Daño aplicado: " + damage);
                }
                Destroy(gameObject);
                return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignorar al jugador
        if (other.CompareTag("Player")) return;

        // Verificar si golpeó a un enemigo (por Tag O por Layer)
        bool isEnemy = other.CompareTag("Enemy") || 
                       (enemyLayers != 0 && ((1 << other.gameObject.layer) & enemyLayers) != 0);

        if (isEnemy)
        {
            Debug.Log("Proyectil golpeó a enemigo: " + other.name);

            // Hacer daño al enemigo
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Daño aplicado: " + damage);
            }

            // Destruir el proyectil
            Destroy(gameObject);
            return;
        }

        // Destruir si golpea cualquier otra cosa que no sea el jugador
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignorar al jugador
        if (collision.gameObject.CompareTag("Player")) return;

        // Si usa Collision en vez de Trigger - detectar por Tag o componente
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null || collision.gameObject.CompareTag("Enemy"))
        {
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Daño aplicado por colisión: " + damage);
            }
        }

        Destroy(gameObject);
    }

    // Visualizar el radio de detección en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
