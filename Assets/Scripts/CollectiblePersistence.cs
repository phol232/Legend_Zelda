using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sistema para guardar qué objetos han sido recogidos.
/// Añadir a cualquier objeto recolectable (items, cristales, etc.)
/// </summary>
public class CollectiblePersistence : MonoBehaviour
{
    [Header("Identificación")]
    [Tooltip("ID único del objeto en esta escena (0, 1, 2, etc.)")]
    public int collectibleIndex = 0;

    private string currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        // Si este objeto ya fue recogido, destruirlo
        if (CollectibleProgress.FueRecogido(currentScene, collectibleIndex))
        {
            Debug.Log($"Objeto {collectibleIndex} ya fue recogido, removiendo...");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Llamar este método cuando el objeto sea recogido
    /// </summary>
    public void OnCollected()
    {
        CollectibleProgress.MarcarRecogido(currentScene, collectibleIndex);
        Debug.Log($"Objeto {collectibleIndex} marcado como recogido en {currentScene}");
    }
}
