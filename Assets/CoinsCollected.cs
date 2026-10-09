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

    void UpdateCoinsText()
    {
        coinsText.text = "Coins: " + coins;
    }
}
