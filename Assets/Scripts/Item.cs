using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Settings")]
    public string itemName;
    public string description;
    public Sprite itemIcon;

    [Header("Gameplay")]
    public bool isCraftingMaterial = true;
}