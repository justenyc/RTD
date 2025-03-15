using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyBindingRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI kbmLabel;
    [SerializeField] TextMeshProUGUI gamepadLabel;

    public void UpdateRow(string labelString, string kbmBinding, string gamepadBinding)
    {
        label.text = labelString;
        kbmLabel.text = kbmBinding;
        gamepadLabel.text = gamepadBinding;
    }
}
