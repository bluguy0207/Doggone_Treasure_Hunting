
using UnityEngine;
using TMPro;

public class TimePlayed : MonoBehaviour
{
    public TMP_Text timeText;
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.unscaledDeltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timeText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
