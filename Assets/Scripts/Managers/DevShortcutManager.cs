using UnityEngine;

public class DevShortcutManager : MonoBehaviour
{
    //acts as a shortcut for devs to bypass needing to start on the boot screen to get the set framerate
    private void Start()
    {
        if(Application.targetFrameRate != 60)
        {
            Application.targetFrameRate = 60;
        }
    }
}
