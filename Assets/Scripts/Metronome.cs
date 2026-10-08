using UnityEngine;

public class Metronome : MonoBehaviour
{
    //song info
    private double _songBPM = 150d;

    //song calculations
    private double beatDurationSec; //seconds per quarter beat, or quarter beat duration in seconds
    private double beatDurationMillisec; //MS per quarter beat

    private double _currentBeatPos;
    private int _nextBeat; //NOT nextBeatPos
    private int _lastBeat; //NOT lastBeatPos, it means the number lastBeat

    private void Awake()
    {
        beatDurationSec = 60 / _songBPM;
        beatDurationMillisec = 60 / _songBPM * 1000;

        _lastBeat = 0;
        _nextBeat = 1;
    }

    private void OnEnable()
    {
        MusicPlayer.OnSongPositionInSecondsChanged += CountBeatsRaw;
    }

    private void OnDisable()
    {
        MusicPlayer.OnSongPositionInSecondsChanged -= CountBeatsRaw;
    }

    private void Update()
    {
        CountBeats();
    }

    private void CountBeatsRaw(double songPosInSeconds)
    {
        //provides the current beat decimal (where 4.25 means a quarter into beat 4)
        //ex: (3 total seconds to divide / 0.34 seconds for a beat = 8.82 beats)
        _currentBeatPos = songPosInSeconds / beatDurationSec;
    }

    private void CountBeats()
    {
        if (_currentBeatPos >= _nextBeat)
        {
            _lastBeat++;

            //emit or send event ("beat", lastbeat)
            Debug.Log("Last beat: " + _lastBeat);

            _nextBeat++;
        }
    }
}
