using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Crafting Slot Settings")]
    public SlotType slotType;
    public Image itemIcon;

    [Header("For Result Slot Only")]
    public Item resultItem;

    private Item currentItem;
    private CraftingSystem craftingSystem;
    private CanvasGroup canvasGroup;
    private Canvas parentCanvas;
    private Vector3 originalPosition;
    private bool isDraggable = false;

    public enum SlotType
    {
        Ingredient,
        Result
    }

    void Start()
    {
        craftingSystem = FindFirstObjectByType<CraftingSystem>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        ClearSlot();
        Debug.Log("CraftingSlot iniciado: " + gameObject.name + " - Tipo: " + slotType);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (slotType == SlotType.Result) return;

        Debug.Log("OnDrop en: " + gameObject.name);

        ItemDragger dragger = eventData.pointerDrag.GetComponent<ItemDragger>();
        if (dragger != null && dragger.currentItem != null)
        {
            Debug.Log("Item arrastrado: " + dragger.currentItem.name);
            AssignItem(dragger.currentItem);
            craftingSystem.CheckCraftingRecipe();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotType != SlotType.Result || !isDraggable || currentItem == null) return;

        Debug.Log("Comenzando arrastre de resultado: " + currentItem.name);

        originalPosition = transform.position;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        if (parentCanvas != null)
        {
            transform.SetParent(parentCanvas.transform);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (slotType != SlotType.Result || !isDraggable || currentItem == null) return;

        if (parentCanvas != null && parentCanvas.worldCamera != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                parentCanvas.transform as RectTransform,
                eventData.position,
                parentCanvas.worldCamera,
                out Vector3 worldPoint);

            transform.position = worldPoint;
        }
        else
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slotType != SlotType.Result || !isDraggable) return;

        Debug.Log("Terminando arrastre de resultado");

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter != null)
        {
            Debug.Log("Soltado sobre: " + eventData.pointerEnter.name);

            InventorySlot inventorySlot = eventData.pointerEnter.GetComponent<InventorySlot>();
            if (inventorySlot == null)
            {
                inventorySlot = eventData.pointerEnter.GetComponentInParent<InventorySlot>();
            }

            if (inventorySlot != null)
            {
                Debug.Log("Soltado en slot de inventario: " + inventorySlot.name);
                craftingSystem.OnResultItemCollected();
                return;
            }
        }

        ReturnToOriginalPosition();
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        Debug.Log("Resultado regresado a posición original");
    }

    public void AssignItem(Item item)
    {
        currentItem = item;
        if (itemIcon != null && item != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.color = Color.white;
        }

        Debug.Log("Item asignado a slot " + gameObject.name + ": " + (item != null ? item.name : "NULL"));
    }

    public void ClearSlot()
    {
        currentItem = null;
        resultItem = null;
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.color = new Color(1, 1, 1, 0);
        }

        SetDraggable(false);
        Debug.Log("Slot limpiado: " + gameObject.name);
    }

    public void SetResultItem(Item item)
    {
        if (slotType == SlotType.Result)
        {
            resultItem = item;
            currentItem = item;

            if (item != null)
            {
                itemIcon.sprite = item.itemIcon;
                itemIcon.color = Color.white;
                Debug.Log("Resultado asignado: " + item.name);
            }
            else
            {
                itemIcon.sprite = null;
                itemIcon.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void SetDraggable(bool draggable)
    {
        isDraggable = draggable;

        if (slotType == SlotType.Result)
        {
            EventTrigger eventTrigger = GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = gameObject.AddComponent<EventTrigger>();
            }
        }
    }

    public Item GetItem()
    {
        return currentItem;
    }
}