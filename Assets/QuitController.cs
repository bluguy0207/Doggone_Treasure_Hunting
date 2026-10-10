using UnityEngine;

public class QuitController : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit(); // Exit Game

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
