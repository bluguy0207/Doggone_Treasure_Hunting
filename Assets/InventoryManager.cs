using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory UI")]
    public InventorySlotUI[] slots;

    private readonly List<InventoryItemData> collectedItems =
        new List<InventoryItemData>();

    private void Start()
    {
        RefreshInventory();
    }

    public void AddItem(KeyItem item)
    {
        if (item == null)
            return;

        // Prevent collecting the same item twice.
        if (collectedItems.Exists(x => x.itemID == item.itemID))
        {
            Destroy(item.gameObject);
            return;
        }

        // Make sure there is room in the inventory.
        if (slots == null || collectedItems.Count >= slots.Length)
        {
            Debug.LogWarning("Inventory is full!");
            return;
        }

        // Save the item's information.
        InventoryItemData data = new InventoryItemData
        {
            itemID = item.itemID,
            itemName = item.itemName,
            itemIcon = item.itemIcon
        };

        collectedItems.Add(data);

        // Update the inventory UI.
        RefreshInventory();

        // Remove the collected object from the scene.
        Destroy(item.gameObject);
    }

    public void RefreshInventory()
    {
        if (slots == null)
        {
            Debug.LogError("InventoryManager: Slots array is not assigned.");
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogError(
                    "InventoryManager: Slot " + i + " is not assigned."
                );
                continue;
            }

            if (i < collectedItems.Count)
            {
                slots[i].SetItem(collectedItems[i], this);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}

[System.Serializable]
public class InventoryItemData
{
    public string itemID;
    public string itemName;
    public Sprite itemIcon;
}