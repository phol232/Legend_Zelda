using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public DoorType doorType;
    public string message = "Presiona E para entrar";
    public string sceneToLoad = "Taller"; // Nombre de la escena a cargar

    [Header("Key Requirements")]
    public Item requiredKey; // El item de llave requerido para abrir la puerta

    [Header("References")]
    public GameObject messagePanel;

    private bool playerInRange = false;
    private TextMeshProUGUI messageText;
    private InventorySystem inventory;

    public enum DoorType
    {
        Forge,         
        FirstTrial,    
        SecondTrial,   
        ThirdTrial,    
        CronalGuard    
    }

    void Start()
    {
        if (messagePanel != null)
        {
            messageText = messagePanel.GetComponentInChildren<TextMeshProUGUI>();
            messagePanel.SetActive(false);
        }
        
        // Buscar el sistema de inventario (puede ser null en algunas escenas)
        inventory = FindFirstObjectByType<InventorySystem>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.X))
        {
            TryInteractWithDoor();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowDoorMessage();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideDoorMessage();
        }
    }

    void ShowDoorMessage()
    {
        if (messagePanel != null && messageText != null)
        {
            string finalMessage = GetDoorMessage();
            messageText.text = finalMessage;
            messagePanel.SetActive(true);
        }
    }

    void HideDoorMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    void TryInteractWithDoor()
    {
        if (doorType == DoorType.Forge)
        {
            // Verificar si el jugador tiene la llave requerida
            if (HasRequiredKey())
            {
                Debug.Log("¡Tienes la llave! Entrando al taller de forja...");
                LoadScene();
            }
            else
            {
                Debug.Log("Necesitas la llave para entrar al taller de forja");
                if (messageText != null)
                {
                    messageText.text = "Taller de Forja: Necesitas la llave...";
                }
            }
        }
        else if (doorType == DoorType.FirstTrial)
        {
            // Primera prueba - sin requisitos
            Debug.Log("Entrando a la primera prueba...");
            LoadScene();
        }
        else if (doorType == DoorType.SecondTrial)
        {
            // Segunda prueba - requiere completar PRUEBA 1
            if (GameProgress.PruebaCompletada(1))
            {
                Debug.Log("PRUEBA 1 completada. Entrando a la segunda prueba...");
                LoadScene();
            }
            else
            {
                Debug.Log("Debes completar PRUEBA 1 primero");
                if (messageText != null)
                {
                    messageText.text = "Debes completar la PRUEBA 1 primero";
                }
            }
        }
        else if (doorType == DoorType.ThirdTrial)
        {
            // Tercera prueba (ZonaFinal) - requiere completar PRUEBA 2
            if (GameProgress.PruebaCompletada(2))
            {
                Debug.Log("PRUEBA 2 completada. Entrando a la zona final...");
                LoadScene();
            }
            else
            {
                Debug.Log("Debes completar PRUEBA 2 primero");
                if (messageText != null)
                {
                    messageText.text = "Debes completar la PRUEBA 2 primero";
                }
            }
        }
        else
        {
            // Otras puertas
            Debug.Log("Entrando...");
            LoadScene();
        }
    }

    bool HasRequiredKey()
    {
        if (requiredKey == null)
        {
            Debug.LogWarning("DoorInteraction: No se ha asignado una llave requerida - permitiendo pasar");
            return true; // Si no hay llave requerida, permitir pasar
        }

        if (inventory == null)
        {
            inventory = FindFirstObjectByType<InventorySystem>();
            if (inventory == null)
            {
                Debug.LogError("DoorInteraction: No se encontró InventorySystem");
                return false;
            }
        }

        // Mostrar qué items tiene el jugador para debug
        Debug.Log("=== VERIFICANDO LLAVE ===");
        Debug.Log("Llave requerida: " + requiredKey.itemName);
        
        List<Item> items = inventory.GetCollectedItems();
        Debug.Log("Items en inventario: " + items.Count);
        foreach (Item item in items)
        {
            Debug.Log("- " + item.itemName);
        }

        bool hasKey = inventory.HasItem(requiredKey);
        Debug.Log("¿Tiene la llave?: " + hasKey);
        
        return hasKey;
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("Cargando escena: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("DoorInteraction: No se ha especificado una escena a cargar");
        }
    }

    string GetDoorMessage()
    {
        switch (doorType)
        {
            case DoorType.Forge:
                return "Taller de Forja: " + message;
            case DoorType.FirstTrial:
                return "Primera Prueba: " + message;
            case DoorType.SecondTrial:
                return "Segunda Prueba: " + message;
            case DoorType.ThirdTrial:
                return "Tercera Prueba: " + message;
            case DoorType.CronalGuard:
                return "Guardia Cronal: " + message;
            default:
                return message;
        }
    }
}