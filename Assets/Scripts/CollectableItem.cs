using System;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Item Settings")]
    public Item itemData;

    [Header("Collection Settings")]
    public float collectionRange = 1.5f;

    private GameObject player;
    public InventorySystem inventory;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private T FindFirstObjectOfType<T>()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (player != null && inventory != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance <= collectionRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    CollectItem();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        if (inventory != null && itemData != null)
        {
            if (inventory.AddItem(itemData))
            {
                Debug.Log("Recolectado: " + itemData.itemName);
                
                // Notificar al sistema de persistencia
                CollectiblePersistence persistence = GetComponent<CollectiblePersistence>();
                if (persistence != null)
                {
                    persistence.OnCollected();
                }
                
                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectionRange);
    }
}