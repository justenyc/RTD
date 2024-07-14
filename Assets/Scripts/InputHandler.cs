using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 m_moveVector;// { get; private set; }
    public Vector2 m_lookVector;// { get; private set; }

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
}
