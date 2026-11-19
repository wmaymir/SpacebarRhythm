using System;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    // Song info
    private double _songBPM = 100d;
    private double _songAdjustedBPM;

    // Seconds per quarter beat, or quarter beat duration in seconds
    private double _beatDurationSec;

    private double _currentBeatPos;
    private int _nextBeat;
    private int _lastBeat;
    public static event Action<int> OnLastBeat;

    // Creates the widest window that opens and closes on the active beat
    private double _activeBeatMargainSec;
    private double _activeBeatPosA;
    private double _activeBeatPosB;
    private bool _firstExitPossible;
    private bool _firstEnterPossible;
    public static event Action<bool> OnWindowOpenedOrClosed;
    public static event Action<int> OnNewWindow;

    // Creates the Great window
    private double _greatActiveBeatMargainSec;
    private double _greatActiveBeatPosA;
    private double _greatActiveBeatPosB;
    private bool _greatFirstExitPossible;
    private bool _greatFirstEnterPossible;
    public static event Action<bool> OnGreatWindowOpenedOrClosed;

    // Creates the Perfect window
    private double _perfectActiveBeatMargainSec;
    private double _perfectActiveBeatPosA;
    private double _perfectActiveBeatPosB;
    private bool _perfectFirstExitPossible;
    private bool _perfectFirstEnterPossible;
    public static event Action<bool> OnPerfectWindowOpenedOrClosed;

    // Creates the Perfect+ window
    private double _perfectPlusActiveBeatMargainSec;
    private double _perfectPlusActiveBeatPosA;
    private double _perfectPlusActiveBeatPosB;
    private bool _perfectPlusFirstExitPossible;
    private bool _perfectPlusFirstEnterPossible;
    public static event Action<bool> OnPerfectPlusWindowOpenedOrClosed;

    private void Awake()
    {

        //Necessary if you come back to the old code*
        _songAdjustedBPM = _songBPM;

        // Margain becomes a + or -
        _activeBeatMargainSec = 0.120;
        _greatActiveBeatMargainSec = 0.060;
        _perfectActiveBeatMargainSec = 0.030;
        _perfectPlusActiveBeatMargainSec = 0.015;

        /* Old code* that may come in handy later
        * Keeps the game from breaking from this:
        * If the BPM is at or above 300 with an _activeBeatMargainSec of .200 (relation of * 1500), the game will break because the 
        * margain becomes longer than a beat itself. To avoid this, proper downscaling should be implimented
        *while (_songAdjustedBPM >= _activeBeatMargainSec * 1500)
        *{
        *    _songAdjustedBPM /= 2;
        *}
        * (note if reused in future: do to all beat margains)
        */

        _beatDurationSec = 60d / _songAdjustedBPM;

        // So that the first "last beat" starts at 0, the beginning
        // Lets the first charted note be the 4th beat of the song due to the nature
        // of the composer sending out note spawns 4 beats ahead
        // Anything more than this will break stuff
        _lastBeat = -1;
        _nextBeat = 0;

        // The active beat windows

        _firstExitPossible = false;
        _firstEnterPossible = true;

        _greatFirstExitPossible = false;
        _greatFirstEnterPossible = true;

        _perfectFirstExitPossible = false;
        _perfectFirstEnterPossible = true;

        _perfectPlusFirstExitPossible = false;
        _perfectPlusFirstEnterPossible = true;
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
        

        // Provides the current beat decimal (where 4.25 means a quarter into beat 4)
        // Ex: (3 total seconds to divide / 0.34 seconds for a beat = 8.82 beats)
        _currentBeatPos = songPosInSeconds / _beatDurationSec;

        // Used for couting beats themselves
        // Can be visualized as the ticks on a timeline
        if (_currentBeatPos >= _nextBeat)
        {
            _lastBeat++;
            OnLastBeat?.Invoke(_lastBeat);

            _nextBeat++;
        }

        // Beat windows opening and closing

        // Used for giving beats a marginal property
        // The defineable window based on the the ticks of the timeline
        // Correctly rides the active area when the beats are redefined (works with beats switching rather than using half beats)
        _activeBeatPosA = _lastBeat + _activeBeatMargainSec; // Active beat "end"'s inner rim on the timeline
        _activeBeatPosB = _nextBeat - _activeBeatMargainSec; // Active beat "start"'s outer rim on the timeline

        if (_currentBeatPos <= _nextBeat && _currentBeatPos >= _activeBeatPosB && _firstEnterPossible)
        {
            _firstEnterPossible = false;
            _firstExitPossible = true;

            //Debug.Log("Window open");
            OnNewWindow?.Invoke(_nextBeat);
            OnWindowOpenedOrClosed?.Invoke(true);
        }
        else if (_currentBeatPos > _activeBeatPosA && _currentBeatPos < _activeBeatPosB && _firstExitPossible)
        {
            _firstEnterPossible = true;
            _firstExitPossible = false;

            //Debug.Log("Window close");
            OnWindowOpenedOrClosed?.Invoke(false);
        }

        // The Great window version

        _greatActiveBeatPosA = _lastBeat + _greatActiveBeatMargainSec;
        _greatActiveBeatPosB = _nextBeat - _greatActiveBeatMargainSec;

        if (_currentBeatPos <= _nextBeat && _currentBeatPos >= _greatActiveBeatPosB && _greatFirstEnterPossible)
        {
            _greatFirstEnterPossible = false;
            _greatFirstExitPossible = true;

            //Debug.Log("Great window open");
            OnGreatWindowOpenedOrClosed?.Invoke(true);
        }
        else if (_currentBeatPos > _greatActiveBeatPosA && _currentBeatPos < _greatActiveBeatPosB && _greatFirstExitPossible)
        {
            _greatFirstEnterPossible = true;
            _greatFirstExitPossible = false;

            //Debug.Log("Great window close");
            OnGreatWindowOpenedOrClosed?.Invoke(false);
        }

        // The Perfect window version

        _perfectActiveBeatPosA = _lastBeat + _perfectActiveBeatMargainSec;
        _perfectActiveBeatPosB = _nextBeat - _perfectActiveBeatMargainSec;

        if (_currentBeatPos <= _nextBeat && _currentBeatPos >= _perfectActiveBeatPosB && _perfectFirstEnterPossible)
        {
            _perfectFirstEnterPossible = false;
            _perfectFirstExitPossible = true;

            //Debug.Log("Perfect window open");
            OnPerfectWindowOpenedOrClosed?.Invoke(true);
        }
        else if (_currentBeatPos > _perfectActiveBeatPosA && _currentBeatPos < _perfectActiveBeatPosB && _perfectFirstExitPossible)
        {
            _perfectFirstEnterPossible = true;
            _perfectFirstExitPossible = false;

            //Debug.Log("Perfect window close");
            OnPerfectWindowOpenedOrClosed?.Invoke(false);
        }

        // The Perfect+ window version

        _perfectPlusActiveBeatPosA = _lastBeat + _perfectPlusActiveBeatMargainSec;
        _perfectPlusActiveBeatPosB = _nextBeat - _perfectPlusActiveBeatMargainSec;

        if (_currentBeatPos <= _nextBeat && _currentBeatPos >= _perfectPlusActiveBeatPosB && _perfectPlusFirstEnterPossible)
        {
            _perfectPlusFirstEnterPossible = false;
            _perfectPlusFirstExitPossible = true;

            //Debug.Log("Perfect+ window open");
            OnPerfectPlusWindowOpenedOrClosed?.Invoke(true);
        }
        else if (_currentBeatPos > _perfectPlusActiveBeatPosA && _currentBeatPos < _perfectPlusActiveBeatPosB && _perfectPlusFirstExitPossible)
        {
            _perfectPlusFirstEnterPossible = true;
            _perfectPlusFirstExitPossible = false;

            //Debug.Log("Perfect+ window close");
            OnPerfectPlusWindowOpenedOrClosed?.Invoke(false);
        }
    }
}
