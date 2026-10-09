
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public List<KeyItemData> items = new List<KeyItemData>();

    public InventorySlotUI[] slots;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    public void AddItem(KeyItem item)
    {
        // Don't collect the same unique item twice.
        if (items.Exists(x => x.itemID == item.itemID))
            return;

        KeyItemData data = new KeyItemData();
        data.itemID = item.itemID;
        data.itemName = item.itemName;
        data.description = item.description;

        items.Add(data);
        RefreshInventory();

        Destroy(item.gameObject);
    }

    public void RefreshInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
                slots[i].SetItem(items[i], this);
            else
                slots[i].ClearSlot();
        }
    }

    public void SelectItem(KeyItemData item)
    {
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
    }

    public bool HasItem(string id)
    {
        return items.Exists(x => x.itemID == id);
    }
}

[System.Serializable]
public class KeyItemData
{
    public string itemID;
    public string itemName;
    public string description;
}
