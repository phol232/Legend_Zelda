using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("Slot References")]
    public Image itemIcon;
    public TextMeshProUGUI itemCountText;

    private Item currentItem;
    private int itemCount = 0;
    private ItemDragger itemDragger;

    void Start()
    {
        Debug.Log("InventorySlot Start: " + gameObject.name);

        itemDragger = GetComponent<ItemDragger>();
        if (itemDragger == null)
        {
            Debug.LogError("No se encontró ItemDragger en: " + gameObject.name);
        }
        else
        {
            Debug.Log("ItemDragger encontrado en: " + gameObject.name);
        }

        if (itemIcon == null)
        {
            itemIcon = GetComponent<Image>();
            Debug.Log("ItemIcon auto-asignado: " + (itemIcon != null));
        }
    }

    public void SetItem(Item item)
    {
        Debug.Log("=== SET ITEM EN SLOT ===");
        Debug.Log("Slot: " + gameObject.name);
        Debug.Log("Item: " + (item != null ? item.name : "NULL"));

        currentItem = item;
        itemCount = 1;

        if (itemIcon != null && item != null && item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.color = Color.white;
            Debug.Log("Sprite asignado al icono: " + item.itemIcon.name);
        }
        else
        {
            Debug.LogError("Problema con itemIcon o item.itemIcon");
        }

        if (itemCountText != null)
        {
            itemCountText.text = itemCount > 1 ? itemCount.ToString() : "";
        }

        if (itemDragger != null)
        {
            itemDragger.SetItem(item);
            Debug.Log("ItemDragger.SetItem llamado");
        }
        else
        {
            Debug.LogError("ItemDragger es NULL - no se puede arrastrar");
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemCount = 0;

        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.color = new Color(1, 1, 1, 0);
        }

        if (itemCountText != null)
        {
            itemCountText.text = "";
        }

        if (itemDragger != null)
        {
            itemDragger.ClearItem();
        }
    }

    public Item GetItem()
    {
        return currentItem;
    }
}