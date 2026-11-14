using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] GameObject interactPrompt;
    [SerializeField] MeshRenderer m_meshRenderer;
    [SerializeField] Transform targetToFollow;
    [SerializeField] bool followPlayer;
    [SerializeField] float moveSpeed = 1f;
    [Tooltip("The min distance required to parent itself to the followTarget")]
    [SerializeField] float distanceToParent = 0.2f;
    System.Action onParentCallback;
    public MeshRenderer meshRenderer => m_meshRenderer;

    public void SetFollow(bool b)
    {
        followPlayer = b;
        this.enabled = b;
        transform.parent = null;
    }

    private void Update()
    {
        if(!followPlayer || transform.parent != null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetToFollow.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetToFollow.position) < distanceToParent)
        {
            transform.parent = targetToFollow;
            this.enabled = false;
            m_meshRenderer.enabled = false;
            return;
        }
    }

    public void SetInteractPromptActive(bool b)
    {
        interactPrompt.SetActive(b);
    }
}
