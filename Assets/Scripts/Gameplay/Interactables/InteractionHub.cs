using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHub : MonoBehaviour
{
    [SerializeField] MeshFilter highlighterMF;
    [SerializeField] List<Interactable> interactables = new List<Interactable>();
    [SerializeField] Collider detector;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Interactable interactable))
        {
            interactables.Add(interactable);
            HighlightInteractable(GetNearestInteractable());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            interactables.Remove(interactable);

            if(interactables.Count < 1)
            {
                highlighterMF.gameObject.SetActive(false);
                return;
            }

            HighlightInteractable(GetNearestInteractable());
        }
    }

    public Interactable GetNearestInteractable()
    {
        if (interactables.Count < 1) return null;

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

    public void HighlightInteractable(Interactable interactable)
    {
        if (interactable == null) return;

        Transform transformToHighlight = interactable.HighlightObject == null ? interactable.transform : interactable.HighlightObject.transform;
        MeshFilter interactableMeshFilter = transformToHighlight.GetComponent<MeshFilter>();

        if(interactableMeshFilter == null)
        {
            Logger.LogError($"No mesh filter found on interactable: {interactable.gameObject.name}");
            return;
        }

        highlighterMF.mesh = interactableMeshFilter.mesh;
        highlighterMF.transform.parent = transformToHighlight;
        highlighterMF.transform.localPosition = Vector3.zero;
        highlighterMF.transform.localRotation = Quaternion.identity;
        highlighterMF.transform.localScale = Vector3.one;

        highlighterMF.gameObject.SetActive(true);
    }
}
