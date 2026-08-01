using Player;
using UnityEngine;
using System.Collections.Generic;

public class Examine : Interactable
{
    [SerializeField] List<string> m_message;

    public override void Interact(PlayerController player)
    {
        if (m_message.Count < 1) return;
        if (isInteracting) return;

        isInteracting = true;
        InGameUiManager.instance.DisplayMessage(m_message, () => isInteracting = false);
        return;
    }
}
