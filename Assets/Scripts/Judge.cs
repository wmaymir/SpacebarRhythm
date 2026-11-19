using System;
using UnityEngine;

public class Judge : MonoBehaviour
{
    // Creates one evaluation per evaluable beat window

    private string[,] _currentGoal;
    private string _onlyButtonInput;

    private bool _beatIsEval;
    private bool _windowIsOpen;
    private bool _windowEvaluationCompleted;

    public static event Action<string> OnEvaluation;

    private bool _greatWindowOpen;
    private bool _perfectWindowOpen;
    private bool _perfectPlusWindowOpen;

    private void Awake()
    {
        _beatIsEval = false;
        _windowIsOpen = false;
        _windowEvaluationCompleted = false;
    }

    private void OnEnable()
    {
        PlayerInput.OnKeyPressed += NewInput;
        Metronome.OnWindowOpenedOrClosed += NewWindow;
        Composer.OnNextTargetReady += NewGoal;

        Metronome.OnGreatWindowOpenedOrClosed += SetGreatWindow;
        Metronome.OnPerfectWindowOpenedOrClosed += SetPerfectWindow;
        Metronome.OnPerfectPlusWindowOpenedOrClosed += SetPerfectPlusWindow;
    }

    private void OnDisable()
    {
        PlayerInput.OnKeyPressed -= NewInput;
        Metronome.OnWindowOpenedOrClosed -= NewWindow;
        Composer.OnNextTargetReady -= NewGoal;

        Metronome.OnGreatWindowOpenedOrClosed -= SetGreatWindow;
        Metronome.OnPerfectWindowOpenedOrClosed -= SetPerfectWindow;
        Metronome.OnPerfectPlusWindowOpenedOrClosed -= SetPerfectPlusWindow;
    }

    // Judging wether a note is hit or missed

    private void NewInput(string input)
    {
        _onlyButtonInput = input;

        if (_windowIsOpen && !_windowEvaluationCompleted && _onlyButtonInput == _currentGoal[0,1] && _beatIsEval)
        {
            //Debug.Log("Hit!");
            JudgeNote(true);
        }
    }

    private void NewWindow(bool isOpen)
    {
        _windowIsOpen = isOpen;
        
        if (_windowIsOpen)
        {
            _windowEvaluationCompleted = false;
        }
        
        if (!_windowIsOpen && !_windowEvaluationCompleted && _beatIsEval)
        {
            //Debug.Log("Miss!");
            JudgeNote(false);
        }
    }

    private void NewGoal(string[,] newGoal)
    {
        _currentGoal = newGoal;

        if (_currentGoal[0,1] != "Cb-null")
        {
            _beatIsEval = true;
        } else
        {
            _beatIsEval = false;
        }
    }

    // Assigning a note's timing

    private void SetGreatWindow(bool open)
    {
        if (open)
        {
            _greatWindowOpen = true;
        }
        else
        {
            _greatWindowOpen = false;
        }
    }

    private void SetPerfectWindow(bool open)
    {
        if (open)
        {
            _perfectWindowOpen = true;
        }
        else
        {
            _perfectWindowOpen = false;
        }
    }

    private void SetPerfectPlusWindow(bool open)
    {
        if (open)
        {
            _perfectPlusWindowOpen = true;
        }
        else
        {
            _perfectPlusWindowOpen = false;
        }
    }

    private void JudgeNote(bool hit)
    {
        if (hit)
        {

            if (_perfectPlusWindowOpen)
            {

                OnEvaluation?.Invoke("Perfect+");

            }
            else if (_perfectWindowOpen)
            {

                OnEvaluation?.Invoke("Perfect");

            }
            else if (_greatWindowOpen)
            {

                OnEvaluation?.Invoke("Great");

            }
            else
            {

                OnEvaluation?.Invoke("Okay");

            }

        } else
        {

            OnEvaluation?.Invoke("Miss");

        }

        _windowEvaluationCompleted = true;
    }
}
