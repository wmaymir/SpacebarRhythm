using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsUI : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadSceneAsync("Song");
    }

    public void OnReturnClicked()
    {
        SceneManager.LoadSceneAsync("Title");
    }
}
