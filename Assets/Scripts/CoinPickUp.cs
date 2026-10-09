using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Search for the coin counter, including inactive UI objects.
        CoinsCollected[] counters =
            Resources.FindObjectsOfTypeAll<CoinsCollected>();

        CoinsCollected coinCounter = null;

        foreach (CoinsCollected counter in counters)
        {
            if (counter.gameObject.scene == gameObject.scene)
            {
                coinCounter = counter;
                break;
            }
        }

        if (coinCounter != null)
        {
            coinCounter.AddCoin();

            if (KujoAudio.Instance != null)
            {
                KujoAudio.Instance.Bark();
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("CoinsCollected was not found in this scene!");
        }
    }
}