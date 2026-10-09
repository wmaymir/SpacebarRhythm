using System;
using UnityEngine;

public class Judge : MonoBehaviour
{
    //creates one evaluation per evaluatable beat window
    //simply this: did you hit the note when it was possible to hit?
    private string[,] _currentGoal;
    private string _onlyButtonInput;

    private bool _beatIsEval; //if the beat is evaluatable, or just "o" on the chart
    private bool _windowIsOpen;
    private bool _windowEvaluationCompleted;

    public static event Action<bool> OnWasEvaluationSucessful;

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
    }

    private void OnDisable()
    {
        PlayerInput.OnKeyPressed -= NewInput;
        Metronome.OnWindowOpenedOrClosed -= NewWindow;
        Composer.OnNextTargetReady -= NewGoal;
    }

    private void NewInput(string input)
    {
        _onlyButtonInput = input;

        if (_windowIsOpen && !_windowEvaluationCompleted && _onlyButtonInput == _currentGoal[0,1] && _beatIsEval)
        {
            OnWasEvaluationSucessful?.Invoke(true);
            _windowEvaluationCompleted = true;
            Debug.Log("Hit!");
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
            OnWasEvaluationSucessful?.Invoke(false);
            Debug.Log("Miss!");
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
}
