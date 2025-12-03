using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador para cada enemigo individual.
/// Asigna un índice único y se destruye si ya fue derrotado anteriormente.
/// </summary>
public class EnemyPersistence : MonoBehaviour
{
    [Header("Identificación")]
    [Tooltip("Índice único del enemigo en esta escena (0, 1, 2, etc.)")]
    public int enemyIndex = 0;

    private string currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        // Si este enemigo ya fue derrotado, destruirlo
        if (GameProgress.EnemigoDerrotado(currentScene, enemyIndex))
        {
            Debug.Log($"Enemigo {enemyIndex} ya fue derrotado, removiendo...");
            Destroy(gameObject);
        }
    }

    // Llamar esto cuando el enemigo muera
    public void OnEnemyDeath()
    {
        GameProgress.MarcarEnemigoDerrotado(currentScene, enemyIndex);
        
        // Verificar si todos los enemigos de la escena fueron derrotados
        CheckAllEnemiesDefeated();
    }

    void CheckAllEnemiesDefeated()
    {
        // Buscar si quedan enemigos vivos (excluyendo este que está muriendo)
        EnemyPersistence[] allEnemies = FindObjectsByType<EnemyPersistence>(FindObjectsSortMode.None);
        
        int enemiesAlive = 0;
        foreach (var enemy in allEnemies)
        {
            if (enemy != this && enemy.gameObject.activeInHierarchy)
            {
                enemiesAlive++;
            }
        }

        Debug.Log($"Enemigos restantes: {enemiesAlive}");

        if (enemiesAlive == 0)
        {
            // Todos los enemigos derrotados - marcar prueba como completada
            if (currentScene == "PRUEBA 1")
            {
                GameProgress.CompletarPrueba(1);
            }
            else if (currentScene == "PRUEBA 2")
            {
                GameProgress.CompletarPrueba(2);
            }
            else if (currentScene == "ZonaFinal")
            {
                GameProgress.CompletarPrueba(3);
            }
        }
    }
}
