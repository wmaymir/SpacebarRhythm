using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnManager : MonoBehaviour
{
    public void OnReturnClicked()
    {
        SceneManager.LoadSceneAsync("Title");
    }
}
