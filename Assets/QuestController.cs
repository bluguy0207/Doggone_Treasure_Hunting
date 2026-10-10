
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
    public int crowKeyCost = 3;

    [Header("Quest Completion")]
    public bool crowKeyRewardClaimed = false;
    public bool catCloverQuestCompleted = false;

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

        if (inventory == null || coinCounter == null)
        {
            Debug.LogError(
                "QuestController: Inventory or Coin Counter is missing!"
            );
            return false;
        }

        if (coinCounter.coins < crowKeyCost)
        {
            Debug.LogWarning("Not enough coins to buy the Golden Key.");
            return false;
        }

        // Add the key first.
        bool added = inventory.AddItemReward(
            keyItemID,
            keyItemName,
            keyItemIcon
        );

        if (!added)
            return false;

        // Charge three coins after the key is added.
        if (!coinCounter.SpendCoins(crowKeyCost))
        {
            inventory.RemoveItem(keyItemID);
            return false;
        }

        crowKeyRewardClaimed = true;
        KujoAudio.Instance.Bark();
        return true;
    }
}
