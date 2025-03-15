using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class RebindingButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI display;

    [SerializeField] string actionMapName;
    [SerializeField] string actionName;
    [SerializeField] RebindingScript.ControlPathMask pathMask;
    [SerializeField] int bindingIndex;
    [SerializeField] string jsonOverrides;

    PlayerInput playerInput;
    RebindingScript rebindingScript;

    public void Init(PlayerInput _playerInput, RebindingScript _rebindingScript)
    {
        playerInput = _playerInput;
        rebindingScript = _rebindingScript;
        UpdateDisplayText();
    }

    public void StartRebinding()
    {
        if (RebindingScript.currentRebindingOperation != null)
        {
            return;
        }

        playerInput.SwitchCurrentActionMap("Rebinding");

        var action = playerInput.actions.FindAction(actionName);
        var oldBinding = action.bindings[bindingIndex];

        RebindingScript.currentRebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(
            operation =>
            {
                Debug.Log($"Attempting to rebind '{action}' to '{operation.selectedControl}'");
                playerInput.SwitchCurrentActionMap("TestRebinding");

                var bind = action.bindings[bindingIndex];
                string ifBoundActionName = "";

                if (bind.effectivePath == "<Keyboard>/backspace" || bind.effectivePath == "<Gamepad>/start")
                {
                    action.ApplyBindingOverride(bindingIndex, oldBinding);
                    Debug.Log($"Binding restored to old binding: {oldBinding}");
                }
                else if(!bind.effectivePath.Contains(pathMask.ToString()))
                {
                    action.ApplyBindingOverride(bindingIndex, oldBinding);
                    Debug.Log("Invalid input type");
                } 
                else if (rebindingScript.CheckDuplicateBinding(bind.effectivePath, actionMapName, action, out ifBoundActionName))
                {
                    action.ApplyBindingOverride(bindingIndex, oldBinding);
                    Debug.Log($"{bind.effectivePath} is already bound to {ifBoundActionName}");
                }
                else
                {
                    Debug.Log($"{action} bound to {bind.effectivePath}");
                }
                
                Reset();
                rebindingScript.SaveBindingOverrides();
                operation.Dispose();
            })
            .Start();

        display.text = "Waiting for input...";
        rebindingScript.SetButtonsInteractable(false, this.GetComponent<Button>());
    }

    public void Reset()
    {
        UpdateDisplayText();
        RebindingScript.currentRebindingOperation = null;
        rebindingScript.SetButtonsInteractable(true, this.GetComponent<Button>());
    }

    void UpdateDisplayText(string overrideDefault = "")
    {
        if (overrideDefault != "")
        {
            display.text = overrideDefault;
            return;
        }

        display.text = rebindingScript.GetCurrentBinding(actionName, bindingIndex);
    }
}
