using System;
using UnityEngine;

public class Composer : MonoBehaviour
{
    [SerializeField]
    private TextAsset _chartFile;

    private string[] _lines;
    private string[,] _notesArray;

    private string[,] _targetBeatAndNote;
    public static event Action<string[,]> OnNextTargetReady;

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

            Debug.Log("Successfully loaded " + _lines.Length + " lines into the array.");
            //if (_lines.Length > 0)
            //{
            //    for (int i = 0; i < _lines.Length; i++)
            //    {
            //        Debug.Log("Line " + (i + 1) + " note: " + _lines[i]);
            //    }
            //}

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
        Metronome.OnLastBeat += BroadcastNextBeat;

    }
    private void OnDisable()
    {
        Metronome.OnLastBeat -= BroadcastNextBeat;
    }

    private void BroadcastNextBeat(int lastBeat)
    {
        //essentially, stop calculations if there's no more lines in the chart
        //may become obsolete later on when a song end is implimented
        if(lastBeat < _lines.Length)
        {
            _targetBeatAndNote = new string[1, 2];

            _targetBeatAndNote[0, 0] = lastBeat.ToString();
            
            //converts chart Xs and Os to button names defined in PlayerInput
            //important for having multiple buttons in the future
            if (_notesArray[lastBeat, 0] == "o")
            {
                _targetBeatAndNote[0, 1] = "Cb-null";
            } 
            else if (_notesArray[lastBeat, 0] == "x")
            {
                _targetBeatAndNote[0, 1] = "Cb-A";
            }

            //Debug.Log(_targetBeatAndNote[0, 0] + _targetBeatAndNote[0, 1]);
            OnNextTargetReady?.Invoke(_targetBeatAndNote);
        }
        else if (!_chartEnded)
        {
            _chartEnded = true;
            OnChartOver?.Invoke();
        }
    }
}