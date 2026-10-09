using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance;

    [Header("References")]
    public CoinsCollected coinCounter;
    public InventoryManager inventory;

    [Header("Golden Key Reward")]
    public string keyItemID = "GoldenKey";
    public string keyItemName = "Golden Key";
    public Sprite keyItemIcon;

    public bool crowKeyRewardClaimed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasCrowKeyReward()
    {
        return crowKeyRewardClaimed;
    }

    public bool GiveCrowKey()
    {
        if (crowKeyRewardClaimed)
            return true;

        if (inventory == null)
        {
            Debug.LogError(
                "QuestController: Inventory is not assigned!"
            );
            return false;
        }

        bool added = inventory.AddItemReward(
            keyItemID,
            keyItemName,
            keyItemIcon
        );

        if (added)
            crowKeyRewardClaimed = true;

        return added;
    }
}