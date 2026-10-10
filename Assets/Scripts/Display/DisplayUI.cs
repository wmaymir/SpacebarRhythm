using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DisplayUI : MonoBehaviour
{

    public TextMeshProUGUI JudgementTextElement;

    public void OnEnable()
    {
        Referee.OnScored += DisplayJudgement;
    }

    public void OnDisable()
    {
        Referee.OnScored -= DisplayJudgement;
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadSceneAsync("Song");
    }

    public void OnReturnClicked()
    {
        SceneManager.LoadSceneAsync("Title");
    }

    //FUTURE: make the ratings pop, have colors, go away, etc
    private void DisplayJudgement(string judgement)
    {
        JudgementTextElement.text = judgement;
    }

}
