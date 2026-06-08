using Player;
using UnityEngine;

public class Examine : Interactable
{
    [SerializeField] string m_message;

    public override void Interact(PlayerController player)
    {
        Logger.LogMessage(m_message);
    }
}
