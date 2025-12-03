using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Puerta que requiere completar una prueba anterior para abrirse.
/// </summary>
public class TrialDoor : MonoBehaviour
{
    [Header("Configuración de Puerta")]
    [Tooltip("Número de prueba requerida para abrir (0 = sin requisito)")]
    public int pruebaRequerida = 0;
    
    [Tooltip("Escena a cargar al entrar")]
    public string escenaDestino;

    [Header("Mensajes")]
    public string mensajeBloqueado = "Debes completar la prueba anterior primero";
    public string mensajeDesbloqueado = "Presiona X para entrar";

    [Header("UI (Opcional)")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.X))
        {
            TryEnterDoor();
        }
    }

    void TryEnterDoor()
    {
        // Si no hay requisito de prueba, o la prueba está completada
        if (pruebaRequerida == 0 || GameProgress.PruebaCompletada(pruebaRequerida))
        {
            // Cargar la escena
            if (!string.IsNullOrEmpty(escenaDestino))
            {
                Debug.Log($"Entrando a {escenaDestino}");
                SceneManager.LoadScene(escenaDestino);
            }
            else
            {
                Debug.LogWarning("No hay escena destino configurada");
            }
        }
        else
        {
            // Mostrar mensaje de bloqueado
            ShowMessage(mensajeBloqueado);
            Debug.Log($"Puerta bloqueada. Requiere completar PRUEBA {pruebaRequerida}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            // Mostrar mensaje apropiado
            if (pruebaRequerida == 0 || GameProgress.PruebaCompletada(pruebaRequerida))
            {
                ShowMessage(mensajeDesbloqueado);
            }
            else
            {
                ShowMessage($"Completa PRUEBA {pruebaRequerida} primero");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideMessage();
        }
    }

    void ShowMessage(string message)
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
        }
        
        if (messageText != null)
        {
            messageText.text = message;
        }
        
        Debug.Log(message);
    }

    void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
}
