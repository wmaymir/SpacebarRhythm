using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * STRUCTURE FOR GAME CODE:
 * Variables
 * Initialization methods
 * Not initialization / other Unity methods
 * Helper methods
 * Debugging / commented methods
 */

public class GameManager : MonoBehaviour
{
    // Makes an instance of the Gamemanager class
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If the instance exists, and this isn't it, 
        // this GameObject needs to go

        if (Instance != this && Instance != null)
        {

            Destroy(this.gameObject);

        } else
        {
            // Assign the instance to this object and start up the game

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Application.targetFrameRate = 60;

            if(SceneManager.GetActiveScene().name == "Boot")
            {
                SceneManager.LoadSceneAsync("Title");
            }
        }
    }
}