using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DisplayUI : MonoBehaviour
{

    public TextMeshProUGUI JudgementTextElement;
    public TextMeshProUGUI MoneyTextElement;
    public TextMeshProUGUI EXTextElement;

    public void OnEnable()
    {
        Judge.OnEvaluation += DisplayJudgement;
        Referee.OnMoneyScored += DisplayMoneyScore;
        Referee.OnEXScored += DisplayEXScore;
    }

    public void OnDisable()
    {
        Judge.OnEvaluation -= DisplayJudgement;
        Referee.OnMoneyScored -= DisplayMoneyScore;
        Referee.OnEXScored -= DisplayEXScore;
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadSceneAsync("Song");
    }

    public void OnReturnClicked()
    {
        SceneManager.LoadSceneAsync("Title");
    }

    // Future: make the ratings pop, have colors, go away, etc
    private void DisplayJudgement(string judgement)
    {
        JudgementTextElement.text = judgement;
    }

    private void DisplayMoneyScore(string scoreString)
    {
        MoneyTextElement.text = scoreString;
        MoneyTextElement.gameObject.SetActive(true);
    }

    private void DisplayEXScore(string scoreString)
    {
        EXTextElement.text = scoreString;
        EXTextElement.gameObject.SetActive(true);
    }

}
