
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string itemID;
    public string itemName;
    public Sprite itemIcon;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        InventoryManager inventory =
            FindAnyObjectByType<InventoryManager>();

        if (inventory != null)
        {
            inventory.AddItem(this);
        }
    }
}
