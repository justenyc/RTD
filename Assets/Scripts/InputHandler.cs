using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 m_moveVector;// { get; private set; }
    public Vector2 m_lookVector;// { get; private set; }
    public bool m_useWeapon;
    public bool m_guard;
    public bool m_turnAround;
    public bool m_aim;
    public bool m_sprint;
    public bool m_cycleCurrentItemLeft;
    public bool m_cycleCurrentItemRight;
    public bool m_interact;
    public bool m_useCurrentItem;
    public bool m_orbInteract;
    public bool m_burst;
    public bool m_openMenu;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        m_moveVector = ctx.ReadValue<Vector2>();
        //Debug.Log(m_moveVector);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        m_lookVector = ctx.ReadValue<Vector2>();
        //Debug.Log(m_lookVector);
    }

    public void OnUseWeapon(InputAction.CallbackContext ctx)
    {
        m_useWeapon = ctx.performed;
    }

    public void Guard(InputAction.CallbackContext ctx)
    {
        m_guard = ctx.performed;
    }

    public void TurnAround(InputAction.CallbackContext ctx)
    {
        m_turnAround = ctx.performed;
    }

    public void Aim(InputAction.CallbackContext ctx)
    {
        m_aim = ctx.performed;
    }

    public void Sprint(InputAction.CallbackContext ctx)
    {
        m_sprint = ctx.performed;
    }

    public void CycleCurrentItemLeft(InputAction.CallbackContext ctx)
    {
        m_cycleCurrentItemLeft = ctx.performed;
    }

    public void CycleCurrentItemRight(InputAction.CallbackContext ctx)
    {
        m_cycleCurrentItemRight = ctx.performed;
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        m_interact = ctx.performed;
    }

    public void UseCurrentItem(InputAction.CallbackContext ctx)
    {
        m_useCurrentItem = ctx.performed;
    }

    public void OrbInteract(InputAction.CallbackContext ctx)
    {
        m_orbInteract = ctx.performed;
    }

    public void Burst(InputAction.CallbackContext ctx)
    {
        m_burst = ctx.performed;
    }

    public void OpenMenu(InputAction.CallbackContext ctx)
    {
        m_openMenu = ctx.performed;
    }
}
