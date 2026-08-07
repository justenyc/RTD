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
        
        // Delay setting to false by a few frames to prevent Interaction stacking on PlayerState change
        InGameUiManager.instance.DisplayMessage(m_message, () =>
        {
            StartCoroutine(Helper.DelayActionByFixedTime(() => isInteracting = false, 0.1f));
        });
        return;
    }
}
