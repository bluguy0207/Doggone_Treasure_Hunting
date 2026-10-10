
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class InventoryManager : MonoBehaviour
// {
//     [Header("Inventory UI")]
//     public InventorySlotUI[] slots;
//     public TMP_Text selectedItemName;

//     private readonly List<InventoryItemData> collectedItems =
//         new List<InventoryItemData>();

//     private void Start()
//     {
//         RefreshInventory();

//         if (selectedItemName != null)
//             selectedItemName.text = "Select an item";
//     }

//     public void AddItem(KeyItem item)
//     {
//         if (item == null)
//             return;

//         if (collectedItems.Exists(x => x.itemID == item.itemID))
//         {
//             Destroy(item.gameObject);
//             return;
//         }

//         if (collectedItems.Count >= slots.Length)
//         {
//             Debug.LogWarning("Inventory is full!");
//             return;
//         }

//         InventoryItemData data = new InventoryItemData
//         {
//             itemID = item.itemID,
//             itemName = item.itemName,
//             itemIcon = item.itemIcon
//         };

//         collectedItems.Add(data);
//         RefreshInventory();

//         Destroy(item.gameObject);
//     }

//     public void RefreshInventory()
//     {
//         if (slots == null)
//         {
//             Debug.LogError("InventoryManager: Slots array is not assigned.");
//             return;
//         }

//         for (int i = 0; i < slots.Length; i++)
//         {
//             if (slots[i] == null)
//             {
//                 Debug.LogError("InventoryManager: Slot " + i +
//                     " is empty in the Inspector.");
//                 continue;
//             }

//             if (i < collectedItems.Count)
//                 slots[i].SetItem(collectedItems[i], this);
//             else
//                 slots[i].ClearSlot();
//         }
//     }

//     public void SelectItem(InventoryItemData item)
//     {
//         if (selectedItemName != null && item != null)
//             selectedItemName.text = item.itemName;
//     }
// }

// [System.Serializable]
// public class InventoryItemData
// {
//     public string itemID;
//     public string itemName;
//     public Sprite itemIcon;
// }
