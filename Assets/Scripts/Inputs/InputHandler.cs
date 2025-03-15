using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 moveVector;// { get; private set; }
    public Vector2 lookVector;// { get; private set; }
    public Action<InputAction.CallbackContext> UseWeapon;
    public Action<InputAction.CallbackContext> Guard;
    public Action<InputAction.CallbackContext> TurnAround;
    public Action<InputAction.CallbackContext> Aim;
    public Action<InputAction.CallbackContext> Sprint;
    public Action<InputAction.CallbackContext> CycleItemLeft;
    public Action<InputAction.CallbackContext> CycleItemRight;
    public Action<InputAction.CallbackContext> Interact;
    public Action<InputAction.CallbackContext> UseCurrentItem;
    public Action<InputAction.CallbackContext> OrbInteract;
    public Action<InputAction.CallbackContext> Burst;
    public Action<InputAction.CallbackContext> OpenMenu;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector2>();
        //Debug.Log(m_moveVector);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        lookVector = ctx.ReadValue<Vector2>();
        //Debug.Log(m_lookVector);
    }

    public void OnUseWeapon(InputAction.CallbackContext ctx)
    {
        UseWeapon?.Invoke(ctx);
    }

    public void OnGuard(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.phase);
        Guard?.Invoke(ctx);
    }

    public void OnTurnAround(InputAction.CallbackContext ctx)
    {
        TurnAround?.Invoke(ctx);
    }

    public void OnAim(InputAction.CallbackContext ctx)
    {
        //Debug.Log(ctx.phase);
        Aim?.Invoke(ctx);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        Sprint?.Invoke(ctx);
    }

    public void OnCycleCurrentItemLeft(InputAction.CallbackContext ctx)
    {
        CycleItemLeft?.Invoke(ctx);
    }

    public void OnCycleCurrentItemRight(InputAction.CallbackContext ctx)
    {
        CycleItemRight?.Invoke(ctx);
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        Interact?.Invoke(ctx);
    }

    public void OnUseCurrentItem(InputAction.CallbackContext ctx)
    {
        UseCurrentItem?.Invoke(ctx);
    }

    public void OnOrbInteract(InputAction.CallbackContext ctx)
    {
        OrbInteract?.Invoke(ctx);
    }

    public void OnBurst(InputAction.CallbackContext ctx)
    {
        Burst?.Invoke(ctx);
    }

    public void OnOpenMenu(InputAction.CallbackContext ctx)
    {
        OpenMenu?.Invoke(ctx);
    }
}
