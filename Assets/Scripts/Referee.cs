using System;
using UnityEngine;

public class Referee : MonoBehaviour
{
    // Although not yet implimented, the Referee's job is also this:
    // loading songs, defining bpm, giving level data to the composer
    // healthbar, combo scoring, broadcasting stuff for UI, ending level in cases of fail-outs, etc

    private bool _songStarted;
    public static event Action OnSongStart;

    private float _moneyScorePossiblePerNote;
    private float _moneyScore;
    public static event Action<string> OnMoneyScored;

    private int _exScore;
    private int _maxEXScore;
    public static event Action<string> OnEXScored;

    private void Awake()
    {
        _songStarted = false;
    }

    private void OnEnable()
    {
        PlayerInput.OnKeyPressed += StartSong;
        Judge.OnEvaluation += CalculateScore;
        MusicPlayer.OnSongOver += CalculateFinalScore;
        Composer.OnNoteCountFinished += SetPossibleScorePerNote;
    }

    private void OnDisable()
    {
        PlayerInput.OnKeyPressed -= StartSong;
        Judge.OnEvaluation -= CalculateScore;
        MusicPlayer.OnSongOver -= CalculateFinalScore;
        Composer.OnNoteCountFinished -= SetPossibleScorePerNote;
    }

    private void StartSong(string keyPressed)
    {
        if(keyPressed == "Cb-A" && !_songStarted)
        {
            //Debug.Log("Start Song");
            OnSongStart?.Invoke();
            _songStarted = true;
        }
    }

    // Scoring Stuff

    private void SetPossibleScorePerNote(int total)
    {
        _moneyScorePossiblePerNote = 1000000 / total;
        _maxEXScore = total * 4;
    }

    private void CalculateScore(string judgement)
    {
        if (judgement == "Okay")
        {

            _moneyScore += _moneyScorePossiblePerNote * 0.5f;
            _exScore += 1;

        }
        else if (judgement == "Great")
        {

            _moneyScore += _moneyScorePossiblePerNote * 0.8f;
            _exScore += 2;

        }
        else if (judgement == "Perfect")
        {

            _moneyScore += _moneyScorePossiblePerNote;
            _exScore += 3;

        }
        else if (judgement == "Perfect+")
        {
            
            _moneyScore += _moneyScorePossiblePerNote;
            _exScore += 4;

        }
    }

    private void CalculateFinalScore()
    {
        OnEXScored?.Invoke("EX Score: " + _exScore + " out of " + _maxEXScore);
        OnMoneyScored?.Invoke("Money Score: " + _moneyScore + " out of 1000000");
    }
}
