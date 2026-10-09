using System;
using UnityEngine;

public class Referee : MonoBehaviour
{
    //essentially is the auctual game manager
    //remember, all scripts should be components that communicate via events,
    //update method should be used minimally and components should be able to shut off
    //after they don't need it anymore, like MusicPlayer

    //FUTURE STUFF IT SHOULD DO
    //loading songs, defining bpm, giving level data to composer
    //healthbar, combo scoring, broadcasting stuff for UI, ending level in cases of fail-outs, etc

    private bool songStarted;
    public static event Action OnSongStart;

    private int hitOpportunities;
    private int notesHit;

    private float endScore;

    private void Awake()
    {
        songStarted = false;
    }

    private void OnEnable()
    {
        PlayerInput.OnKeyPressed += StartSong;
        Judge.OnWasEvaluationSucessful += TallyScore;
    }

    private void OnDisable()
    {
        PlayerInput.OnKeyPressed -= StartSong;
        Judge.OnWasEvaluationSucessful -= TallyScore;
    }

    private void StartSong(string keyPressed)
    {
        if(keyPressed == "Cb-A" && !songStarted)
        {
            Debug.Log("Start Song");
            OnSongStart?.Invoke();
            songStarted = true;
            //no need for end song yet, since music player has song over event
            //which sucessfully stops everything. Might be useful down the line
        }
    }

    //FUTURE: add logic for note timing windows here, not in Judge
    private void TallyScore(bool addOne)
    {
        if(addOne == true)
        {
            notesHit++;
            hitOpportunities++;
        }
        else
        {
            hitOpportunities++;
        }
    }

    private void CalculateTotals()
    {
        //when the end of song event is broadcasted, calculate and debug the total
    }
}
