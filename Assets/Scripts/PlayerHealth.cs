using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Sistema de vida del jugador.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI (Opcional)")]
    public Slider healthSlider;          // Barra de vida
    public Image healthFill;             // Para cambiar color

    [Header("Invincibility")]
    public float invincibilityTime = 1f; // Tiempo de invulnerabilidad después de recibir daño
    private float lastDamageTime = -10f;

    [Header("Death Settings")]
    public string gameOverScene = "";    // Escena de Game Over (vacío = reiniciar actual)

    private bool isDead = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        // Verificar invulnerabilidad
        if (Time.time < lastDamageTime + invincibilityTime)
        {
            Debug.Log("Jugador invulnerable!");
            return;
        }
        
        lastDamageTime = Time.time;
        currentHealth -= damage;
        
        Debug.Log("Jugador recibió " + damage + " de daño. Vida: " + currentHealth + "/" + maxHealth);
        
        // Efecto visual de daño
        StartCoroutine(DamageFlash());
        
        UpdateHealthUI();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Jugador curado. Vida: " + currentHealth + "/" + maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
        
        if (healthFill != null)
        {
            // Cambiar color según la vida
            float healthPercent = currentHealth / maxHealth;
            if (healthPercent > 0.5f)
            {
                healthFill.color = Color.green;
            }
            else if (healthPercent > 0.25f)
            {
                healthFill.color = Color.yellow;
            }
            else
            {
                healthFill.color = Color.red;
            }
        }
    }

    System.Collections.IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            // Parpadeo rojo
            for (int i = 0; i < 3; i++)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("¡El jugador ha muerto!");
        
        // Reiniciar o ir a Game Over
        if (!string.IsNullOrEmpty(gameOverScene))
        {
            SceneManager.LoadScene(gameOverScene);
        }
        else
        {
            // Reiniciar la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
