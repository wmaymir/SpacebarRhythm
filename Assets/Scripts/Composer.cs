using System;
using Unity.VisualScripting;
using UnityEngine;

public class Composer : MonoBehaviour
{
    [SerializeField]
    private TextAsset _chartFile;

    private string[] _lines;
    private string[,] _notesArray;

    private string[,] _targetBeatAndNote;
    public static event Action<string[,]> OnNextTargetReady;

    private int spawnBeat;
    private string[,] _spawnBeatAndNote;
    public static event Action<string[,]> OnNoteSpawnInfoReady;

    private bool _chartEnded;
    public static event Action OnChartOver;


    void Awake()
    {
        _chartEnded = false;

        if (_chartFile != null)
        {
            //split by line breaks and remove any completely empty trailing lines
            //\r\n", "\r", "\n correspond to the codes for linebreaks from all 3 major operating systems
            _lines = _chartFile.text.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            //Debug.Log("Successfully loaded " + _lines.Length + " lines into the chart array.");

            //makes a 2D array with beat location of the note and it's value
            _notesArray = new string[_lines.Length, 2];

            for (int i = 0; i < _lines.Length; i++)
            {
                _notesArray[i, 0] = _lines[i]; //0: the text line
                _notesArray[i, 1] = (i + 1).ToString(); //1: the line number (from 1)
            }
        }
    }

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

    private void BroadcastNextBeat(int nextBeat)
    {
        //essentially, stop calculations if there's no more lines in the chart
        if (nextBeat < _lines.Length)
        {
            _targetBeatAndNote = new string[1, 2];

            _targetBeatAndNote[0, 0] = nextBeat.ToString();

            //converts chart Xs and Os to button names defined in PlayerInput
            //important for having multiple buttons in the future
            if (_notesArray[nextBeat, 0] == "o")
            {
                _targetBeatAndNote[0, 1] = "Cb-null";
            }
            else if (_notesArray[nextBeat, 0] == "x")
            {
                _targetBeatAndNote[0, 1] = "Cb-A";
            }

            //Debug.Log(_targetBeatAndNote[0, 0] + _targetBeatAndNote[0, 1]);
            //random spaces break the chart, something to look into
            OnNextTargetReady?.Invoke(_targetBeatAndNote);
        }
        else if (!_chartEnded)
        {
            _chartEnded = true;
            OnChartOver?.Invoke();
        }
    }

    private void BroadcastSpawnInfo(int lastBeat)
    {
        //almost the same function but for the display to spawn future notes
        spawnBeat = lastBeat + 4;

        if (spawnBeat < _lines.Length)
        {
            _spawnBeatAndNote = new string[1, 2];

            _spawnBeatAndNote[0, 0] = spawnBeat.ToString();

            //converts chart Xs and Os to button names defined in PlayerInput
            //important for having multiple buttons in the future
            if (_notesArray[spawnBeat, 0] == "o")
            {
                _spawnBeatAndNote[0, 1] = "Cb-null";
            }
            else if (_notesArray[spawnBeat, 0] == "x")
            {
                _spawnBeatAndNote[0, 1] = "Cb-A";
            }

            //Debug.Log(_spawnBeatAndNote[0, 0] + _spawnBeatAndNote[0, 1]);
            //random spaces break the chart, something to look into
            OnNoteSpawnInfoReady?.Invoke(_spawnBeatAndNote);
        }
    }
}