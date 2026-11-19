using System;
using UnityEngine;

public class Composer : MonoBehaviour
{
    // Charting rules:
    // o means no note
    // x means note
    // New line means new beat
    // The first note can only be charted as early as the 4th audible beat of the song
    // Charts must end in a o, otherwise the judge will think there's an unjudged sliver
    // of a beat at the end of the song due to the nature of the metronome
    // no bpm changes
    // one note combinations only, for now

    [SerializeField]
    private TextAsset _chartFile;

    private string[] _lines;
    private string[,] _notesArray;
    private int _noteCount;
    public static event Action<int> OnNoteCountFinished;

    private string[,] _targetBeatAndNote;
    public static event Action<string[,]> OnNextTargetReady;

    private int _spawnBeat;
    private string[,] _spawnBeatAndNote;
    public static event Action<string[,]> OnNoteSpawnInfoReady;

    private bool _chartEnded;
    public static event Action OnChartOver;

    private void OnEnable()
    {
        Metronome.OnNewWindow += BroadcastNextBeat;
        Metronome.OnLastBeat += BroadcastSpawnInfo;
    }
    private void OnDisable()
    {
        Metronome.OnNewWindow -= BroadcastNextBeat;
        Metronome.OnLastBeat -= BroadcastSpawnInfo;
    }

    private void Start()
    {
        _chartEnded = false;

        if (_chartFile != null)
        {
            // Split by line breaks and remove any completely empty trailing lines
            // "\r\n", "\r", and "\n" correspond to the codes for linebreaks from all 3 major operating systems

            _lines = _chartFile.text.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            //Debug.Log("Successfully loaded " + _lines.Length + " lines into the chart array.");

            // Makes a 2D array of size _lines.Legnth on the X and 2 on the Y
            _notesArray = new string[_lines.Length, 2];

            // Fills the 2D array with the beat location and the note value
            for (int i = 0; i < _lines.Length; i++)
            {
                _notesArray[i, 0] = _lines[i]; // 0 on the Y: the text line. Assigns our "o" or "x" from the 1D array
                _notesArray[i, 1] = (i + 1).ToString(); // 1 on the Y: the line number (from 1). Assigns the beat number

                // Broadcasts an event with the number of notes in the song (useful for moneyscore)
                if (_lines[i] != "o") // Future: if combos end up becoming notes, this becomes "as long as it's not an empty line"
                {
                    _noteCount++;
                }
            }

            OnNoteCountFinished?.Invoke(_noteCount);
        }
    }

    private void BroadcastNextBeat(int nextBeat)
    {
        if (nextBeat < _lines.Length)
        {
            _targetBeatAndNote = new string[1, 2];

            _targetBeatAndNote[0, 0] = nextBeat.ToString();

            // Converts chart Xs and Os to button names defined in PlayerInput
            // This is important for having multiple buttons in the future

            if (_notesArray[nextBeat, 0] == "o")
            {
                _targetBeatAndNote[0, 1] = "Cb-null";
            }
            else if (_notesArray[nextBeat, 0] == "x")
            {
                _targetBeatAndNote[0, 1] = "Cb-A";
            }

            // Random spaces still break the chart, something to look into
            //Debug.Log(_targetBeatAndNote[0, 0] + _targetBeatAndNote[0, 1]);
            OnNextTargetReady?.Invoke(_targetBeatAndNote);
        }
        else if (!_chartEnded)
        {
            _chartEnded = true;
            OnChartOver?.Invoke();
        }
    }

    // Almost the same function but for the display to spawn future notes
    private void BroadcastSpawnInfo(int lastBeat)
    {
        _spawnBeat = lastBeat + 4;

        if (_spawnBeat < _lines.Length)
        {
            _spawnBeatAndNote = new string[1, 2];

            _spawnBeatAndNote[0, 0] = _spawnBeat.ToString();

            // Converts chart Xs and Os to button names defined in PlayerInput
            // This is important for having multiple buttons in the future

            if (_notesArray[_spawnBeat, 0] == "o")
            {
                _spawnBeatAndNote[0, 1] = "Cb-null";
            }
            else if (_notesArray[_spawnBeat, 0] == "x")
            {
                _spawnBeatAndNote[0, 1] = "Cb-A";
            }

            //Debug.Log(_spawnBeatAndNote[0, 0] + _spawnBeatAndNote[0, 1]);
            OnNoteSpawnInfoReady?.Invoke(_spawnBeatAndNote);
        }
    }
}