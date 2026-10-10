
using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [Header("Quest Settings")]
    public InventoryManager inventory;
    public GameObject victoryScreen;
    public string goldenKeyID = "GoldenKey";

    private bool chestOpened = false;

    private void Start()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
        }
    }

    public bool CanInteract()
    {
        return !chestOpened;
    }

    public void Interact()
    {
        if (chestOpened)
            return;

        if (inventory == null)
        {
            Debug.LogError("InventoryManager is not assigned!");
            return;
        }

        if (!inventory.HasItem(goldenKeyID))
        {
            Debug.Log("Kujo needs the Golden Key to open the chest.");
            return;
        }

        if (!inventory.RemoveItem(goldenKeyID))
        {
            Debug.LogError("Could not remove the Golden Key.");
            return;
        }

        chestOpened = true;

        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            Debug.Log("Victory screen active: " + victoryScreen.activeSelf);
        }
        else
        {
            Debug.LogWarning("Victory screen is not assigned!");
        }

        Debug.Log("Congratulations! Kujo found the treasure!");
    }
}
