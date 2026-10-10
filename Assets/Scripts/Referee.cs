using System;
using UnityEngine;

public class Referee : MonoBehaviour
{
    //FUTURE STUFF IT SHOULD DO
    //loading songs, defining bpm, giving level data to composer
    //healthbar, combo scoring, broadcasting stuff for UI, ending level in cases of fail-outs, etc

    private bool songStarted;
    public static event Action OnSongStart;

    private double hitOpportunities;
    private double notesHit;
    public static event Action<string> OnScored;

    private double endScore;
    public static event Action<double> OnSongScored;

    private void Awake()
    {
        songStarted = false;
    }

    private void OnEnable()
    {
        PlayerInput.OnKeyPressed += StartSong;
        Judge.OnWasEvaluationSucessful += CalculateScore;
        MusicPlayer.OnSongOver += CalculateFinalScore;
    }

    private void OnDisable()
    {
        PlayerInput.OnKeyPressed -= StartSong;
        Judge.OnWasEvaluationSucessful -= CalculateScore;
        MusicPlayer.OnSongOver -= CalculateFinalScore;
    }

    private void StartSong(string keyPressed)
    {
        if(keyPressed == "Cb-A" && !songStarted)
        {
            Debug.Log("Start Song");
            OnSongStart?.Invoke();
            songStarted = true;
        }
    }

    //FUTURE: add logic for note timing windows here, not in Judge
    private void CalculateScore(bool addOne)
    {
        if(addOne == true)
        {
            notesHit++;
            hitOpportunities++;
            OnScored?.Invoke("Okay!");
        }
        else
        {
            hitOpportunities++;
            OnScored?.Invoke("Miss!");
        }
    }

    private void CalculateFinalScore()
    {
        endScore = notesHit / hitOpportunities;
        OnSongScored?.Invoke(endScore);
    }
}
