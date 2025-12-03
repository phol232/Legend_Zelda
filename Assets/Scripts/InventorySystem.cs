using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    [Header("Inventory Settings")]
    public int inventorySize = 5;

    [Header("UI References")]
    public GameObject inventoryPanel;
    public GameObject slotPrefab;

    [Header("Item References")]
    public List<Item> availableItems = new List<Item>();

    private List<InventorySlot> slots = new List<InventorySlot>();
    private List<Item> collectedItems = new List<Item>();

    void Awake()
    {
        // Singleton pattern - persistir entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeInventory();
    }

    void InitializeInventory()
    {
        // Solo inicializar si no hay slots creados
        if (slots.Count > 0) return;
        
        if (inventoryPanel == null || slotPrefab == null) return;
        
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, inventoryPanel.transform);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slots.Add(slot);
            slot.ClearSlot();
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        if (collectedItems.Count >= inventorySize)
        {
            Debug.Log("Inventario lleno!");
            return false;
        }

        collectedItems.Add(itemToAdd);

        UpdateInventoryUI();

        Debug.Log("Item agregado: " + itemToAdd.itemName);
        return true;
    }

    void UpdateInventoryUI()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.ClearSlot();
        }

        for (int i = 0; i < collectedItems.Count; i++)
        {
            if (i < slots.Count)
            {
                slots[i].SetItem(collectedItems[i]);
            }
        }
    }

    public bool AddCraftedItem(Item craftedItem)
    {
        if (craftedItem != null)
        {
            if (AddItem(craftedItem))
            {
                Debug.Log("Item crafteado agregado al inventario: " + craftedItem.name);
                return true;
            }
            else
            {
                Debug.Log("No hay espacio para el item crafteado: " + craftedItem.name);
                return false;
            }
        }
        return false;
    }

    public bool HasItem(Item item)
    {
        if (item == null) return false;
        
        // Primero intentar comparación directa
        if (collectedItems.Contains(item))
        {
            return true;
        }
        
        // Si no funciona, buscar por nombre del item
        foreach (Item collectedItem in collectedItems)
        {
            if (collectedItem != null && collectedItem.itemName == item.itemName)
            {
                return true;
            }
        }
        
        return false;
    }

    // Nueva función para verificar si tiene un item por nombre
    public bool HasItemByName(string itemName)
    {
        foreach (Item collectedItem in collectedItems)
        {
            if (collectedItem != null && collectedItem.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public List<Item> GetCollectedItems()
    {
        return new List<Item>(collectedItems);
    }

    public bool RemoveItem(Item itemToRemove)
    {
        if (collectedItems.Contains(itemToRemove))
        {
            collectedItems.Remove(itemToRemove);
            UpdateInventoryUI();
            Debug.Log("Item removido: " + itemToRemove.itemName);
            return true;
        }
        return false;
    }
}