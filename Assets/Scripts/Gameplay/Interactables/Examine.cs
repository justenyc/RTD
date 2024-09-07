using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : Interactable
{
    [SerializeField] string m_message;

    public override void Interact(PlayerController player)
    {
        Debug.Log(m_message);
    }
}
