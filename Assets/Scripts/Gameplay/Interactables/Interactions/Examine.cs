using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class Examine : Interactable
{
    [SerializeField] string m_message;

    public override void Interact(PlayerController player)
    {
        Logger.LogMessage(m_message);

        if(GameStateManager.instance.IsPaused == true)
        {
            GameStateManager.instance.IsPaused = false;
            InGameUiManager.instance.DisplayMessage("");
            return;
        }

        InGameUiManager.instance.DisplayMessage(m_message);
        GameStateManager.instance.IsPaused = true;
    }
}
