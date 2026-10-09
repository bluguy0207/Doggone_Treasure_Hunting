using UnityEngine;

public class KujoAudio : MonoBehaviour
{
    public static KujoAudio Instance;

    public AudioSource audioSource;
    public AudioClip barkSound;

    private void Awake()
    {
        Instance = this;
    }

    public void Bark()
    {
        if (audioSource != null && barkSound != null)
        {
            audioSource.PlayOneShot(barkSound);
        }
    }
}