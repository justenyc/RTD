using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject highlightObject;
    [SerializeField] UnityEvent<Player.PlayerController> OnPlayerInteractEvent;

    [SerializeField] protected bool isInteracting;

    public GameObject HighlightObject => highlightObject;

    public virtual void Interact(Player.PlayerController player)
    {
        OnPlayerInteractEvent?.Invoke(player);
    }
}