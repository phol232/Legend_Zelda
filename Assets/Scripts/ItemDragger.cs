using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Drag Settings")]
    public Image itemImage;
    public Canvas parentCanvas;

    [HideInInspector]
    public Item currentItem;

    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        Debug.Log("ItemDragger Start en: " + gameObject.name);

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("No hay CanvasGroup en: " + gameObject.name);
        }
        else
        {
            Debug.Log("CanvasGroup encontrado en: " + gameObject.name);
        }

        if (parentCanvas == null)
        {
            parentCanvas = GetComponentInParent<Canvas>();
        }

        if (parentCanvas == null)
        {
            Debug.LogError("No se encontró parentCanvas en: " + gameObject.name);
        }
        else
        {
            Debug.Log("ParentCanvas: " + parentCanvas.name);
        }

        if (itemImage == null)
        {
            itemImage = GetComponent<Image>();
            Debug.Log("ItemImage auto-asignado: " + (itemImage != null));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("=== ON BEGIN DRAG ===");
        Debug.Log("Objeto: " + gameObject.name);
        Debug.Log("CurrentItem: " + (currentItem != null ? currentItem.name : "NULL"));
        Debug.Log("EventData: " + eventData.pointerDrag.name);

        if (currentItem == null)
        {
            Debug.LogError("No se puede arrastrar - currentItem es NULL");
            return;
        }

        if (canvasGroup == null)
        {
            Debug.LogError("No se puede arrastrar - canvasGroup es NULL");
            return;
        }

        originalPosition = transform.position;
        originalParent = transform.parent;

        Debug.Log("Original Parent: " + originalParent.name);
        Debug.Log("Original Position: " + originalPosition);

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        Debug.Log("CanvasGroup configurado - alpha: 0.6f, blocksRaycasts: false");

        if (parentCanvas != null)
        {
            transform.SetParent(parentCanvas.transform);
            Debug.Log("Movido al canvas: " + parentCanvas.name);
        }
        else
        {
            Debug.LogError("No hay parentCanvas para mover el objeto");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        Debug.Log("OnDrag - Posición del mouse: " + eventData.position);

        if (parentCanvas != null && parentCanvas.worldCamera != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                parentCanvas.transform as RectTransform,
                eventData.position,
                parentCanvas.worldCamera,
                out Vector3 worldPoint);

            transform.position = worldPoint;
            Debug.Log("Posición actualizada: " + worldPoint);
        }
        else
        {
            transform.position = eventData.position;
            Debug.Log("Usando fallback - Posición: " + eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("=== ON END DRAG ===");
        Debug.Log("Objeto: " + gameObject.name);
        Debug.Log("PointerEnter: " + (eventData.pointerEnter != null ? eventData.pointerEnter.name : "NULL"));

        if (currentItem == null) return;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            Debug.Log("CanvasGroup restaurado - alpha: 1f, blocksRaycasts: true");
        }

        if (eventData.pointerEnter != null)
        {
            Debug.Log("Buscando CraftingSlot en: " + eventData.pointerEnter.name);

            CraftingSlot craftingSlot = eventData.pointerEnter.GetComponent<CraftingSlot>();
            if (craftingSlot == null)
            {
                craftingSlot = eventData.pointerEnter.GetComponentInParent<CraftingSlot>();
                Debug.Log("Buscando en parent - CraftingSlot: " + (craftingSlot != null ? craftingSlot.name : "NULL"));
            }

            if (craftingSlot != null && craftingSlot.slotType == CraftingSlot.SlotType.Ingredient)
            {
                Debug.Log("Slot válido encontrado: " + craftingSlot.name);

                craftingSlot.AssignItem(currentItem);

                transform.SetParent(craftingSlot.transform);
                transform.localPosition = Vector3.zero;

                Debug.Log("Item asignado al slot: " + craftingSlot.name);

                CraftingSystem craftingSystem = FindFirstObjectByType<CraftingSystem>();
                if (craftingSystem != null)
                {
                    craftingSystem.CheckCraftingRecipe();
                    Debug.Log("CraftingSystem notificado");
                }
                return;
            }
        }
        else
        {
            Debug.Log("No se soltó sobre ningún objeto (pointerEnter es NULL)");
        }

        Debug.Log("Regresando a posición original");
        ReturnToOriginalPosition();
    }

    public void ReturnToOriginalPosition()
    {
        if (originalParent != null)
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
            Debug.Log("Regresado a: " + originalParent.name);
        }
        else
        {
            Debug.LogError("No hay originalParent para regresar");
        }
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        Debug.Log("SetItem llamado en: " + gameObject.name + " - Item: " + (item != null ? item.name : "NULL"));

        if (itemImage != null && item != null)
        {
            itemImage.sprite = item.itemIcon;
            itemImage.color = Color.white;
            Debug.Log("Sprite asignado: " + item.itemIcon.name);
        }
        else
        {
            Debug.LogError("ItemImage es NULL o Item es NULL");
        }
    }

    public void ClearItem()
    {
        currentItem = null;
        if (itemImage != null)
        {
            itemImage.sprite = null;
            itemImage.color = new Color(1, 1, 1, 0);
        }
    }
}