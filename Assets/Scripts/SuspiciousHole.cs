
using UnityEngine;

public class SuspiciousHole : MonoBehaviour, IInteractable
{
    [Header("Quest Settings")]
    public InventoryManager inventory;
    public GameObject treasureChest;
    public string goldenKeyID = "GoldenKey";

    private bool chestRevealed = false;

    private void Start()
    {
        if (treasureChest != null)
        {
            treasureChest.SetActive(false);
        }
        else
        {
            Debug.LogError("Treasure chest is not assigned!");
        }
    }

    public bool CanInteract()
    {
        return !chestRevealed;
    }

    public void Interact()
    {
        if (inventory == null)
        {
            Debug.LogError("InventoryManager is not assigned!");
            return;
        }

        if (!inventory.HasItem(goldenKeyID))
        {
            Debug.Log("Kujo needs the Golden Key to find the treasure.");
            return;
        }

        if (treasureChest == null)
        {
            Debug.LogError("Treasure chest is not assigned!");
            return;
        }

        treasureChest.SetActive(true);
        chestRevealed = true;
        KujoAudio.Instance.Bark();

        Debug.Log("Kujo dug up the treasure chest!");
    }
}
