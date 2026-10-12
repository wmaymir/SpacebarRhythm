using System;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private string _userOffsetInput;
    [SerializeField]
    private TMP_InputField offsetInputField;
    public static event Action<double> OnGlobalOffsetUpdated;

    public void OnApplyUserOffset()
    {
        _userOffsetInput = offsetInputField.text;

        try
        {
            OnGlobalOffsetUpdated?.Invoke(double.Parse(_userOffsetInput));
        }
        catch
        {
            //Debug.Log("Failed to parse entered offset to double");
        }

    }
}
