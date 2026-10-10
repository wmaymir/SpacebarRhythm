using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    //scene info
    private AudioSource _songAudioSource;

    //pre-song
    private double _globalOffsetMS = -200; // - for if the beats come too early, + for if they come too late
    private bool _songStarted;

    //begin-song
    private double _dspSongStartTime;

    //mid-song
    double elapsedSongTime;
    double songPositionInSeconds;
    public static event Action<double> OnSongPositionInSecondsChanged;

    //end-song
    private bool _songOver;
    public static event Action OnSongOver;

    void Awake()
    {
        _songAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Referee.OnSongStart += StartSong;
        Composer.OnChartOver += EndSong;
    }

    private void OnDisable()
    {
        Referee.OnSongStart -= StartSong;
    }

    private void Update()
    {
        //debugdspTime()
        TimeSong();
    }

    private void StartSong()
    {
        _dspSongStartTime = AudioSettings.dspTime;
        _songAudioSource.Play();
        _songStarted = true;
        _songOver = false;
    }

    private void TimeSong()
    {
        if (_songStarted && !_songOver)
        {
            //calculates the time elapsed on the audio thread
            elapsedSongTime = AudioSettings.dspTime - _dspSongStartTime;

            //apply offsets
            songPositionInSeconds = elapsedSongTime + (_globalOffsetMS / 1000);

            OnSongPositionInSecondsChanged?.Invoke(songPositionInSeconds);

            if (_songAudioSource.isPlaying == false)
            {
                EndSong();
            }
        }
    }

    private void EndSong()
    {
        if (!_songOver)
        {
            _songOver = true;
            OnSongOver?.Invoke();
            Debug.Log("End Song");

            //disable this object to save computing on the update method
            //background music and FX should be on separate components
            this.enabled = false;
        }
    }

    private void debugdspTime()
    {
        Debug.Log(AudioSettings.dspTime);
    }
}
