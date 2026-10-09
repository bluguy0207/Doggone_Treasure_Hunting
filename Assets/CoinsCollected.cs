using UnityEngine;
using TMPro;

public class CoinsCollected : MonoBehaviour
{
    public TMP_Text coinsText;
    public int coins = 0;

    void Start()
    {
        UpdateCoinsText();
    }

    public void AddCoin()
    {
        coins++;
        Debug.Log("Coins variable is now: " + coins);
        UpdateCoinsText();
    }

    public bool SpendCoins(int amount)
    {
        if (amount < 0 || coins < amount)
            return false;

        coins -= amount;
        UpdateCoinsText();
        return true;
    }

    void UpdateCoinsText()
    {
        coinsText.text = "Coins: " + coins;
    }
}
