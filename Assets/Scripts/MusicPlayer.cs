using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Scene info

    private AudioSource _songAudioSource;

    // Pre-song

    // - for if the beats come too early, + for if they come too late
    // -315 is a fair default for Itch IO, -300 is better for editor testing
    private double _globalOffsetMS = -315d;
    private bool _songStarted;

    // Begin-song

    private double _dspSongStartTime;

    // Mid-song

    private double _elapsedSongTime;
    private double _songPositionInSeconds;
    public static event Action<double> OnSongPositionInSecondsChanged;

    // End-song

    private bool _songOver;
    public static event Action OnSongOver;

    void Awake()
    {
        _songAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Referee.OnSongStart += StartSong;
        Settings.OnGlobalOffsetUpdated += UpdateOffset;
        Composer.OnChartOver += EndSong;
    }

    private void OnDisable()
    {
        Referee.OnSongStart -= StartSong;
        Settings.OnGlobalOffsetUpdated -= UpdateOffset;
        Composer.OnChartOver -= EndSong;
    }

    private void Update()
    {
        TimeSong();
        //DebugDSPTime()
    }

    private void StartSong()
    {
        _dspSongStartTime = AudioSettings.dspTime;
        _songAudioSource.Play();
        _songStarted = true;
        _songOver = false;
    }

    private void UpdateOffset(double newOffset)
    {
        _globalOffsetMS = newOffset;
    }

    private void TimeSong()
    {
        if (_songStarted && !_songOver)
        {
            // Calculates the time elapsed on the audio thread
            _elapsedSongTime = AudioSettings.dspTime - _dspSongStartTime;

            // Applies offsets
            _songPositionInSeconds = _elapsedSongTime + (_globalOffsetMS / 1000);

            OnSongPositionInSecondsChanged?.Invoke(_songPositionInSeconds);

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
            //Debug.Log("End Song");

            // Disables this object to save computing on the update method
            // Background music and FX should be on separate components
            this.enabled = false;
        }
    }

    private void DebugDSPTime()
    {
        Debug.Log(AudioSettings.dspTime);
    }
}
