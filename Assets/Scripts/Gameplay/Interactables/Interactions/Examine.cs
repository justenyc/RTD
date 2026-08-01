using Player;
using UnityEngine;
using System.Collections.Generic;

public class Examine : Interactable
{
    [SerializeField] List<string> m_message;

    Queue<string> messageQueue = new();

    public override void Interact(PlayerController player)
    {
        if (isInteracting) return;

        isInteracting = true;
        InGameUiManager.instance.DisplayMessage(m_message, () => isInteracting = false);
        return;
    }
}
