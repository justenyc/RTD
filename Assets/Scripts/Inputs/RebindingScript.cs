using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;
//using Xefier.Persistence;

public class RebindingScript : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    //[SerializeField] Xefier.Persistence.PersistentVariableView persistentVariableView;
    public string bindingOverrides;

    public static InputActionRebindingExtensions.RebindingOperation currentRebindingOperation;

    private void Start()
    {
        //var bindingOverrides = Saves.Get(persistentVariableView.SaveName).GetString(persistentVariableView.Key, "");
        if (bindingOverrides != "")
        {
            playerInput.actions.LoadBindingOverridesFromJson(bindingOverrides);
        }

        foreach (RebindingButton rb in this.GetComponentsInChildren<RebindingButton>())
        {
            rb.Init(playerInput, this);
        }
    }

    public void SetButtonsInteractable(bool b, Button exceptFor = null)
    {
        foreach (var butt in transform.parent.GetComponentsInChildren<Button>())
        {
            if (butt != exceptFor)
            {
                butt.interactable = b;
            }
        }
    }

    public string GetCurrentBinding(string actionName, int bindingIndex)
    {
        var action = playerInput.actions.FindAction(actionName);
        Debug.Log($"{actionName} : {bindingIndex}");
        string s = InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        return s;
    }

    public bool CheckDuplicateBinding(string inputPath, string actionMapName, InputAction exceptionAction, out string boundActionName)
    {
        var bindings = playerInput.actions.FindActionMap(actionMapName).bindings;
        foreach (var binding in playerInput.actions.FindActionMap(actionMapName).bindings)
        {
            if(binding.action == exceptionAction.name)
            {
                continue;
            }

            if(inputPath == binding.effectivePath)
            {
                boundActionName = binding.action;
                return true;
            }
        }
        boundActionName = "";
        return false;
    }

    public void DefaultControls()
    {
        playerInput.actions.RemoveAllBindingOverrides();
        //bindingOverrides = "";
        foreach (var button in GetComponentsInChildren<RebindingButton>())
        {
            button.Reset();
        }
    }

    public void SaveBindingOverrides()
    {
        bindingOverrides = playerInput.actions.SaveBindingOverridesAsJson();
        //persistentVariableView.SetString(playerInput.actions.SaveBindingOverridesAsJson());
    }

    public void OnTestAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log($"TestAction: {ctx.action.bindings[0].hasOverrides}");
        }
    }

    public void OnTestAction2(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log($"TestAction2: {ctx.action.bindings[0].hasOverrides}");
        }
    }    
    
    public void OnTestAction3(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log($"TestAction3: {ctx.action.bindings[0].hasOverrides}");
        }
    }

    public enum ControlPathMask
    {
        Gamepad,
        Keyboard,
        Mouse
    }
}
