using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRange = 10f;
    public float chaseSpeed = 3f;
    public float stopDistance = 3f;          // Distancia a la que se detiene para disparar
    public float preferredDistance = 4f;     // Distancia preferida del jugador

    [Header("Separation Settings")]
    public float separationDistance = 2f;    // Distancia mínima entre enemigos
    public float separationForce = 3f;       // Fuerza de separación

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;          // Prefab de la bala enemiga
    public float shootCooldown = 2f;         // Tiempo entre disparos
    public float bulletSpeed = 8f;           // Velocidad de la bala
    public float bulletDamage = 10f;         // Daño de la bala
    private float lastShootTime = 0f;

    [Header("Health Settings")]
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("References")]
    private Transform player;
    private Rigidbody2D rb;

    private bool isChasing = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("EnemyController: No se encontró objeto con tag 'Player'");
        }

        // Obtener Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("EnemyController: Rigidbody2D no encontrado");
        }
    }

    void Update()
    {
        if (player == null || rb == null || isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            
            // Si está en rango de disparo, disparar y mantener distancia
            if (distanceToPlayer <= stopDistance)
            {
                // Detenerse y disparar
                rb.linearVelocity = Vector2.zero;
                TryShoot();
            }
            else if (distanceToPlayer <= preferredDistance)
            {
                // Mantener distancia y disparar
                MaintainDistance();
                TryShoot();
            }
            else
            {
                // Acercarse al jugador
                ChasePlayer();
            }
        }
        else
        {
            // Fuera de rango - dejar de perseguir
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void ChasePlayer()
    {
        // Calcular dirección hacia el jugador
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // Calcular fuerza de separación de otros enemigos
        Vector2 separationVector = CalculateSeparation();

        // Combinar dirección hacia jugador con separación
        Vector2 finalDirection = (directionToPlayer + separationVector).normalized;

        // Mover el enemigo
        rb.linearVelocity = finalDirection * chaseSpeed;
    }

    void MaintainDistance()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        
        // Calcular separación de otros enemigos
        Vector2 separationVector = CalculateSeparation();
        
        // Si está muy cerca, alejarse
        if (distanceToPlayer < stopDistance - 0.5f)
        {
            Vector2 awayFromPlayer = -directionToPlayer;
            rb.linearVelocity = (awayFromPlayer + separationVector).normalized * chaseSpeed * 0.5f;
        }
        else
        {
            // Solo aplicar separación de otros enemigos
            rb.linearVelocity = separationVector.normalized * chaseSpeed * 0.3f;
        }
    }

    void TryShoot()
    {
        if (bulletPrefab == null) return;
        
        // Verificar cooldown
        if (Time.time < lastShootTime + shootCooldown) return;
        
        lastShootTime = Time.time;
        
        // Calcular dirección hacia el jugador
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        
        // Crear la bala
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        
        // Configurar la bala
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = directionToPlayer * bulletSpeed;
        }
        
        // Configurar el daño (usar EnemyBullet si existe, sino Bullet)
        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
        {
            enemyBullet.damage = bulletDamage;
        }
        
        // Rotar la bala hacia el jugador
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        Debug.Log(gameObject.name + " disparó!");
    }

    // Calcula un vector de separación para alejarse de otros enemigos cercanos
    Vector2 CalculateSeparation()
    {
        Vector2 separation = Vector2.zero;
        int nearbyEnemies = 0;

        // Buscar todos los enemigos
        EnemyController[] allEnemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        foreach (EnemyController other in allEnemies)
        {
            // Ignorarse a sí mismo y enemigos muertos
            if (other == this || other.isDead) continue;

            float distance = Vector2.Distance(transform.position, other.transform.position);

            // Si está demasiado cerca
            if (distance < separationDistance && distance > 0.01f)
            {
                // Calcular dirección de alejamiento
                Vector2 awayDirection = (transform.position - other.transform.position).normalized;
                
                // Más fuerza cuanto más cerca esté
                float strength = (separationDistance - distance) / separationDistance;
                separation += awayDirection * strength * separationForce;
                nearbyEnemies++;
            }
        }

        return separation;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " ha muerto!");

        // Notificar al sistema de persistencia
        EnemyPersistence persistence = GetComponent<EnemyPersistence>();
        if (persistence != null)
        {
            persistence.OnEnemyDeath();
        }

        // Detener movimiento
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Destruir el enemigo (o desactivar)
        Destroy(gameObject, 0.5f);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar rango de detección en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualizar distancia de parada
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
