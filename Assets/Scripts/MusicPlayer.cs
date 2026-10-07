using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    /*
     * STRUCTURE FOR THE GAME:
     * variables
     * Initialization methods
     * not initialization methods
     * helper methods
     * debugging / commented methods
     */

    //scene info
    private AudioSource _songAudioSource;

    //pre-song
    public bool songStarted { get; private set; }

    //begin-song
    private double _dspSongStartTime;

    //mid-song
    double elapsedSongTime;
    double songPositionInSeconds;
    public static event Action<double> OnSongPositionInSecondsChanged;

    //end-song
    //private bool _songOver = false;

    void Awake()
    {
        songStarted = false;
        _songAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //add OnSpacePressed to the list of things to do
        PlayerInput.OnSpacePressed += StartSong;
    }

    private void OnDisable()
    {
        //remove it
        PlayerInput.OnSpacePressed -= StartSong;
    }

    private void Update()
    {
        //debugdspTime()
        TimeSong();
    }

    private void StartSong()
    {
        if (!songStarted)
        {
            //start the song
            _dspSongStartTime = AudioSettings.dspTime;
            _songAudioSource.Play();
            songStarted = true;
        }
    }

    private void TimeSong()
    {
        if (songStarted)
        {
            //calculates the time elapsed on the audio thread
            elapsedSongTime = AudioSettings.dspTime - _dspSongStartTime;

            //will change with more offset options
            songPositionInSeconds = elapsedSongTime;

            OnSongPositionInSecondsChanged?.Invoke(songPositionInSeconds);
        }
    }

    private void debugdspTime()
    {
        //you can't directly reset AudioSettings.dspTime
        //it's managed internally and only resets when the application restarts/reloads
        //this is why an offset is necessary
        Debug.Log(AudioSettings.dspTime);
    }
}
