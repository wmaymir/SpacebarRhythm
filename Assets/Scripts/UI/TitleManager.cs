using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string difficulty;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnEasyClicked()
    {
        difficulty = "easy";
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadSceneAsync("Song");
    }

    public void OnHardClicked()
    {
        difficulty = "hard";
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadSceneAsync("Song");
    }

    //this runs after the new scene finishes loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Song")
        {
            SceneManager.MoveGameObjectToScene(this.gameObject, scene);
        }
    }
}