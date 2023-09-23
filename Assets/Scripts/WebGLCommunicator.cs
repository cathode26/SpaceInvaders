using UnityEngine;

public class WebGLCommunicator : MonoBehaviour
{
    public void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            Time.timeScale = 0;
    }
    public void ShowPlayer(string show)
    {
        // Convert the string to a bool value
        bool showBool = show.ToLower() == "true";

        if (showBool)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;

        Debug.Log("ShowPlayer called with value: " + show);
    }
}