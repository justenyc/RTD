using DG.Tweening;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] GameObject interactPrompt;
    [SerializeField] MeshRenderer m_meshRenderer;
    Tween moveTween;

    public MeshRenderer meshRenderer => m_meshRenderer;

    public Tween StartMoveTween(Vector3 position, float time, TweenCallback onCompleteCallback)
    {
        if (moveTween == null)
        {
            SetInteractPromptActive(false);
            moveTween = transform.DOMove(position, time);
            moveTween.onComplete += onCompleteCallback;
            moveTween.onComplete += OnTweenComplete;
            return moveTween.Play();
        }

        return moveTween;
    }

    void OnTweenComplete()
    {
        meshRenderer.enabled = false;
        moveTween = null;
    }

    public void SetInteractPromptActive(bool b)
    {
        interactPrompt.SetActive(b);
    }
}
