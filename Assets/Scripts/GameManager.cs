using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * STRUCTURE FOR THE GAME:
 * variables
 * Initialization methods
 * not initialization / other Unity methods
 * helper methods
 * debugging / commented methods
 */

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
        SceneManager.LoadSceneAsync("Title");
    }
}