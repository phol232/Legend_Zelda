using System;
using UnityEngine;
using System.Collections.Generic;

public class CraftingSystem : MonoBehaviour
{
    [Header("Crafting References")]
    public CraftingSlot slot1;
    public CraftingSlot slot2;
    public CraftingSlot resultSlot;

    [Header("Crafting Recipes")]
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

    private InventorySystem inventory;
    private bool isInitialized = false;

    void Start()
    {
        InitializeCraftingSystem();
    }

    void InitializeCraftingSystem()
    {
        Debug.Log("=== INITIALIZING CRAFTING SYSTEM ===");

        if (slot1 == null) Debug.LogError("CraftingSystem: slot1 no está asignado en el Inspector");
        if (slot2 == null) Debug.LogError("CraftingSystem: slot2 no está asignado en el Inspector");
        if (resultSlot == null) Debug.LogError("CraftingSystem: resultSlot no está asignado en el Inspector");

        inventory = FindFirstObjectByType<InventorySystem>();
        if (inventory == null)
        {
            Debug.LogError("CraftingSystem: No se encontró InventorySystem en la escena");
        }
        else
        {
            Debug.Log("CraftingSystem: InventorySystem encontrado");
        }

        if (recipes.Count == 0)
        {
            Debug.LogWarning("CraftingSystem: No hay recetas configuradas");
        }
        else
        {
            Debug.Log("CraftingSystem: " + recipes.Count + " recetas configuradas");
        }

        isInitialized = (slot1 != null && slot2 != null && resultSlot != null && inventory != null);
        Debug.Log("CraftingSystem inicializado: " + isInitialized);
    }

    public void CheckCraftingRecipe()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("CraftingSystem no está inicializado, no se puede verificar receta");
            return;
        }

        Debug.Log("=== CHECKING CRAFTING RECIPE ===");

        if (slot1 == null || slot2 == null || resultSlot == null)
        {
            Debug.LogError("CraftingSystem: Uno o más slots son null");
            return;
        }

        Item item1 = slot1.GetItem();
        Item item2 = slot2.GetItem();

        Debug.Log("Slot1: " + (item1 != null ? item1.name : "NULL"));
        Debug.Log("Slot2: " + (item2 != null ? item2.name : "NULL"));

        if (resultSlot != null)
        {
            resultSlot.ClearSlot();
            resultSlot.SetDraggable(false);
        }

        if (item1 != null && item2 != null)
        {
            bool recipeFound = false;

            foreach (CraftingRecipe recipe in recipes)
            {
                if (recipe == null)
                {
                    Debug.LogWarning("CraftingSystem: Receta null encontrada en la lista");
                    continue;
                }

                if (recipe.ingredient1 == null || recipe.ingredient2 == null || recipe.result == null)
                {
                    Debug.LogWarning("CraftingSystem: Receta con ingredientes o resultado null");
                    continue;
                }

                bool recipeMatch = (recipe.ingredient1 == item1 && recipe.ingredient2 == item2) ||
                                 (recipe.ingredient1 == item2 && recipe.ingredient2 == item1);

                Debug.Log("Verificando receta: " + recipe.ingredient1.name + " + " + recipe.ingredient2.name + " = " + recipe.result.name);
                Debug.Log("Coincide: " + recipeMatch);

                if (recipeMatch)
                {
                    Debug.Log("¡Receta encontrada! Resultado: " + recipe.result.name);
                    recipeFound = true;

                    if (resultSlot != null)
                    {
                        resultSlot.SetResultItem(recipe.result);
                        resultSlot.SetDraggable(true);
                    }

                    AutoCraftItem(recipe.result);
                    break;
                }
            }

            if (!recipeFound)
            {
                Debug.Log("No se encontró receta para " + item1.name + " + " + item2.name);
            }
        }
        else
        {
            Debug.Log("Faltan ingredientes para craftear");
        }
    }

    private void AutoCraftItem(Item resultItem)
    {
        Debug.Log("=== AUTO CRAFTING ===");

        if (inventory != null && resultItem != null)
        {
            if (inventory.AddItem(resultItem))
            {
                Debug.Log("¡Item crafteado y agregado al inventario: " + resultItem.name + "!");

                RemoveIngredients();
            }
            else
            {
                Debug.Log("No hay espacio en el inventario para: " + resultItem.name);
            }
        }
        else
        {
            Debug.LogError("InventorySystem o resultItem es NULL");
        }
    }

    private void RemoveIngredients()
    {
        Debug.Log("=== REMOVING INGREDIENTS ===");

        Item item1 = slot1?.GetItem();
        Item item2 = slot2?.GetItem();

        if (inventory != null)
        {
            if (item1 != null) inventory.RemoveItem(item1);
            if (item2 != null) inventory.RemoveItem(item2);
        }

        if (slot1 != null) slot1.ClearSlot();
        if (slot2 != null) slot2.ClearSlot();

        Debug.Log("Slots de ingredientes limpiados");
    }

    public void OnResultItemCollected()
    {
        Debug.Log("Resultado recolectado - limpiando slot de resultado");
        if (resultSlot != null)
        {
            resultSlot.ClearSlot();
            resultSlot.SetDraggable(false);
        }
    }
}

[System.Serializable]
public class CraftingRecipe
{
    public Item ingredient1;
    public Item ingredient2;
    public Item result;
}