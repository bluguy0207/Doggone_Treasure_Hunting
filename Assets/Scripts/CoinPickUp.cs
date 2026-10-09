
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Find the coin counter in this scene.
        CoinsCollected[] counters =
            Resources.FindObjectsOfTypeAll<CoinsCollected>();

        CoinsCollected coinCounter = null;

        foreach (CoinsCollected counter in counters)
        {
            if (counter.gameObject.scene == gameObject.scene)
            {
                coinCounter = counter;
                Debug.Log("Selected coin counter: " + counter.gameObject.name);
                break;
            }
        }

        if (coinCounter != null)
        {
            coinCounter.AddCoin();

            Debug.Log("Coin pickup updated: "
                + coinCounter.gameObject.name
                + " | Coins: " + coinCounter.coins);

            if (KujoAudio.Instance != null)
            {
                KujoAudio.Instance.Bark();
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError(
                "CoinsCollected was not found in this scene!"
            );
        }
    }
}
