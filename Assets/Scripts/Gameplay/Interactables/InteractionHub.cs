using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHub : MonoBehaviour
{
    [SerializeField] List<Interactable> interactables = new List<Interactable>();
    [SerializeField] Collider detector;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Interactable interactable))
        {
            interactables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            interactables.Remove(interactable);
        }
    }

    public Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        if(interactables.Count > 0)
        {
            nearest = interactables[0];
            foreach(Interactable interactable in interactables) 
            {
                nearest = Vector3.Distance(nearest.transform.position, transform.position) < Vector3.Distance(interactable.transform.position, transform.position) ?  nearest : interactable;
            }
        }
        return nearest;
    }
}
