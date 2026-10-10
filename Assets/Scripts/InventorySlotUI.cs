using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("Slot References")]
    [SerializeField] private Image itemIcon;

    private void Awake()
    {
        if (itemIcon == null)
        {
            Transform iconTransform = transform.Find("ItemIcon");

            if (iconTransform != null)
            {
                itemIcon = iconTransform.GetComponent<Image>();
            }
        }
    }

    public void SetItem(InventoryItemData item, InventoryManager manager)
    {
        if (itemIcon == null)
        {
            Debug.LogError(name + ": ItemIcon Image is not assigned.");
            return;
        }

        Debug.Log(
            name + " displays " + item.itemName +
            " using Image: " + itemIcon.name +
            " under " + itemIcon.transform.parent.name
        );

        itemIcon.sprite = item.itemIcon;
        itemIcon.enabled = item.itemIcon != null;
        itemIcon.preserveAspect = true;
    }

    public void ClearSlot()
    {
        if (itemIcon == null)
        {
            Debug.LogError(name + ": ItemIcon Image is not assigned.");
            return;
        }

        itemIcon.sprite = null;
        itemIcon.enabled = false;
    }
}