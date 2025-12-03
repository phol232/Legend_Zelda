using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage = 10f;         // Daño del ataque
    public float attackCooldown = 0.5f;      // Tiempo entre ataques
    public LayerMask enemyLayers;            // Capas de enemigos

    [Header("Weapon Visual")]
    public GameObject weaponObject;          // Objeto visual del arma (hijo del jugador)
    public Transform weaponHolder;           // Punto donde se ancla el arma
    public Sprite weaponSprite;              // Sprite del arma
    public GameObject weaponPrefabForScenes; // Prefab del arma para crear en otras escenas

    [Header("Shooting")]
    public GameObject bulletPrefab;          // Prefab del proyectil
    public Transform firePoint;              // Punto desde donde dispara
    public float bulletSpeed = 10f;          // Velocidad del proyectil

    [Header("Animation")]
    public Animator animator;

    private static bool hasWeaponGlobal = false;  // Persistir entre escenas
    private bool hasWeapon = false;
    private float lastAttackTime = 0f;
    private Vector2 lastDirection = Vector2.right;

    void Start()
    {
        // Si no hay weaponHolder asignado, buscar uno o crear uno
        if (weaponHolder == null)
        {
            // Intentar buscar WeaponAnchor
            Transform anchor = transform.Find("WeaponAnchor");
            if (anchor != null)
            {
                weaponHolder = anchor;
                Debug.Log("WeaponHolder encontrado automáticamente: WeaponAnchor");
            }
            else
            {
                // Crear un weaponHolder
                GameObject holder = new GameObject("WeaponHolder");
                holder.transform.SetParent(transform);
                holder.transform.localPosition = new Vector3(0.5f, 0, 0); // A la derecha del jugador
                weaponHolder = holder.transform;
                Debug.Log("WeaponHolder creado automáticamente");
            }
        }
        
        // Verificar si ya tenía el arma (cambio de escena)
        if (hasWeaponGlobal)
        {
            hasWeapon = true;
            
            // Si el weaponObject es null, intentar crearlo
            if (weaponObject == null && weaponPrefabForScenes != null)
            {
                weaponObject = Instantiate(weaponPrefabForScenes, weaponHolder);
                weaponObject.transform.localPosition = Vector3.zero;
                weaponObject.transform.localRotation = Quaternion.identity;
                weaponObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                Debug.Log("Arma recreada en nueva escena");
            }
            
            if (weaponObject != null)
            {
                weaponObject.SetActive(true);
            }
            Debug.Log("Arma restaurada después de cambio de escena");
        }
        else if (weaponObject != null)
        {
            weaponObject.SetActive(false);
        }
    }

    void Update()
    {
        // Actualizar la última dirección del jugador
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        if (horizontal != 0 || vertical != 0)
        {
            lastDirection = new Vector2(horizontal, vertical).normalized;
            UpdateWeaponRotation();
        }

        // Solo puede atacar si tiene el arma - TECLA C
        if (hasWeapon && Input.GetKeyDown(KeyCode.C))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Shoot();
            }
        }
    }

    public void EquipWeapon()
    {
        hasWeapon = true;
        hasWeaponGlobal = true;  // Guardar globalmente
        
        if (weaponObject != null)
        {
            weaponObject.SetActive(true);
        }

        Debug.Log("¡Arma equipada! Presiona C para disparar");
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        hasWeapon = true;
        hasWeaponGlobal = true;  // Guardar globalmente

        // Si hay un prefab, instanciarlo
        if (weaponPrefab != null && weaponHolder != null)
        {
            if (weaponObject != null)
            {
                Destroy(weaponObject);
            }
            
            weaponObject = Instantiate(weaponPrefab, weaponHolder);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            weaponObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);  // Escala pequeña
            weaponObject.SetActive(true);
        }
        else if (weaponObject != null)
        {
            weaponObject.SetActive(true);
        }

        Debug.Log("¡Arma equipada! Presiona C para disparar");
    }

    void UpdateWeaponRotation()
    {
        if (weaponObject == null) return;

        // Rotar el arma según la dirección del jugador
        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        
        if (weaponHolder != null)
        {
            weaponHolder.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Voltear el sprite del arma si mira a la izquierda
        if (lastDirection.x < 0)
        {
            weaponObject.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            weaponObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Shoot()
    {
        lastAttackTime = Time.time;

        // Calcular dirección OPUESTA al movimiento
        Vector2 shootDirection = -lastDirection;  // Opuesta
        
        Debug.Log("¡Disparando en dirección opuesta: " + shootDirection + "!");

        // Reproducir animación de ataque
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Crear el proyectil
        if (bulletPrefab != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            
            // Darle velocidad al proyectil (dirección opuesta al movimiento)
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootDirection * bulletSpeed;
            }

            // Configurar el daño del proyectil
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = attackDamage;
                bulletScript.enemyLayers = enemyLayers;
            }

            // Rotar el proyectil para que apunte en la dirección correcta
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            Debug.LogWarning("No hay bulletPrefab asignado");
        }
    }

    public bool HasWeapon()
    {
        return hasWeapon;
    }

    public void UnequipWeapon()
    {
        hasWeapon = false;
        hasWeaponGlobal = false;
        
        if (weaponObject != null)
        {
            weaponObject.SetActive(false);
        }
    }
}
