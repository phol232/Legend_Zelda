using UnityEngine;
using TMPro;

public class ChestInteraction : MonoBehaviour
{
    [Header("Chest Settings")]
    public string chestName = "Cofre del Forjador";
    public string interactMessage = "Presiona X para abrir";

    [Header("UI References")]
    public GameObject chestPanel;          // Panel que muestra el contenido del cofre
    public GameObject weaponInChest;       // El arma dentro del panel
    public GameObject messagePanel;        // Panel de mensaje
    public TextMeshProUGUI messageText;    // Texto del mensaje

    [Header("Weapon Settings")]
    public GameObject weaponPrefab;        // Prefab del arma para dar al jugador

    private bool playerInRange = false;
    private bool isOpen = false;
    private bool weaponTaken = false;
    private MiraController playerController;

    void Start()
    {
        if (chestPanel != null)
            chestPanel.SetActive(false);
        
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.X))
        {
            if (!isOpen && !weaponTaken)
            {
                OpenChest();
            }
            else if (isOpen)
            {
                CloseChest();
            }
        }

        // Si el panel está abierto y presiona C, toma el arma
        if (isOpen && !weaponTaken && Input.GetKeyDown(KeyCode.C))
        {
            TakeWeapon();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerController = other.GetComponent<MiraController>();
            
            if (!weaponTaken)
            {
                ShowMessage(chestName + ": " + interactMessage);
            }
            else
            {
                ShowMessage(chestName + ": Ya tomaste el arma");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerController = null;
            HideMessage();
            
            if (isOpen)
            {
                CloseChest();
            }
        }
    }

    void OpenChest()
    {
        isOpen = true;
        
        if (chestPanel != null)
        {
            chestPanel.SetActive(true);
        }

        if (weaponInChest != null)
        {
            weaponInChest.SetActive(true);
        }

        // Pausar movimiento del jugador
        if (playerController != null)
        {
            playerController.SetMovement(false);
        }

        ShowMessage("Presiona C para tomar el arma");
        
        Debug.Log("Cofre abierto");
    }

    void CloseChest()
    {
        isOpen = false;
        
        if (chestPanel != null)
        {
            chestPanel.SetActive(false);
        }

        // Reanudar movimiento del jugador
        if (playerController != null)
        {
            playerController.SetMovement(true);
        }

        HideMessage();
        
        Debug.Log("Cofre cerrado");
    }

    void TakeWeapon()
    {
        weaponTaken = true;
        
        // Ocultar el arma del panel
        if (weaponInChest != null)
        {
            weaponInChest.SetActive(false);
        }

        // Dar el arma al jugador
        if (playerController != null)
        {
            // Equipar usando PlayerCombat
            PlayerCombat combat = playerController.GetComponent<PlayerCombat>();
            if (combat != null)
            {
                combat.EquipWeapon(weaponPrefab);
            }

            // También usar PlayerWeaponHolder si existe
            PlayerWeaponHolder weaponHolder = playerController.GetComponent<PlayerWeaponHolder>();
            if (weaponHolder != null)
            {
                weaponHolder.EquipWeapon(weaponPrefab);
            }
        }

        ShowMessage("¡Has obtenido el arma! Presiona C para disparar");
        
        Debug.Log("¡Arma tomada y equipada!");

        // Cerrar el cofre después de tomar el arma
        Invoke("CloseChest", 1.5f);
    }

    void ShowMessage(string message)
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            
            if (messageText != null)
            {
                messageText.text = message;
            }
        }
    }

    void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
}
