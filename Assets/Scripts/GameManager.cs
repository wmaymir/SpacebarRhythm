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
        SceneManager.LoadSceneAsync("Title");
    }
}