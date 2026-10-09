using System;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    //song info
    private double _songBPM = 100d;

    //song calculations
    private double _songAdjustedBPM;

    private double _beatDurationSec; //seconds per quarter beat, or quarter beat duration in seconds

    private double _currentBeatPos;
    private int _nextBeat; //NOT nextBeatPos
    private int _lastBeat; //NOT lastBeatPos, it means the number lastBeat
    private float _halfBeat; //simply returns 0.5 (lastbeat + 0.5) so that the window is not based off of nextbeat, which changes as soon as it is reached

    //creates the widest window that opens and closes on the active beat
    private double _activeBeatMargainSec;
    private double _activeBeatStartPos;
    private double _activeBeatEndPos;
    private bool _firstExitPossible;
    private bool _firstEnterPossible;

    public static event Action<int> OnLastBeat;
    public static event Action<int> OnBeatEntry;
    public static event Action<int> OnBeatExit;
    public static event Action<bool> OnWindowOpenedOrClosed;

    private void Awake()
    {
        //keeps the game from breaking from this:
        //if BPM is at or above 300bpm with an _activeBeatMargainSec of .200 (relation of * 1500), the game will break because the 
        //margain becomes longer than a beat itself. To avoid this, proper downscaling should be implimented
        _songAdjustedBPM = _songBPM;
        _activeBeatMargainSec = 0.200; //Margain becomes a + or -

        while (_songAdjustedBPM >= _activeBeatMargainSec * 1500)
        {
            _songAdjustedBPM /= 2;
        }

        _beatDurationSec = 60d / _songAdjustedBPM;

        _lastBeat = 0;
        _halfBeat = 0.5f;
        _nextBeat = 1;

        //the active beat window
        _firstExitPossible = false;
        _firstEnterPossible = true;
    }

    private void OnEnable()
    {
        MusicPlayer.OnSongPositionInSecondsChanged += CountBeats;
    }

    private void OnDisable()
    {
        MusicPlayer.OnSongPositionInSecondsChanged -= CountBeats;
    }

    private void CountBeats(double songPosInSeconds)
    {
        //provides the current beat decimal (where 4.25 means a quarter into beat 4)
        //ex: (3 total seconds to divide / 0.34 seconds for a beat = 8.82 beats)
        _currentBeatPos = songPosInSeconds / _beatDurationSec;

        //used for couting beats thenmselves
        //the ticks on a timeline
        if (_currentBeatPos >= _nextBeat)
        {
            _lastBeat++;
            OnLastBeat?.Invoke(_lastBeat);

            _halfBeat++;
            _nextBeat++;
        }

        //used for giving beats a marginal property
        //the defineable window based on the halfticks on the timeline
        _activeBeatStartPos = _halfBeat - _activeBeatMargainSec;
        _activeBeatEndPos = _halfBeat + _activeBeatMargainSec;

        //beat windows opening and closing
        if (_currentBeatPos >= _activeBeatStartPos && _currentBeatPos <= _activeBeatEndPos && _firstEnterPossible)
        {
            _firstEnterPossible = false;
            _firstExitPossible = true;

            OnBeatEntry?.Invoke(_lastBeat);

            OnWindowOpenedOrClosed?.Invoke(true);
        }
        else if (_currentBeatPos < _activeBeatStartPos && _firstExitPossible || _currentBeatPos > _activeBeatEndPos && _firstExitPossible)
        {
            _firstEnterPossible = true;
            _firstExitPossible = false;

            OnBeatExit?.Invoke(_lastBeat);

            OnWindowOpenedOrClosed?.Invoke(false);
        }
    }
}
