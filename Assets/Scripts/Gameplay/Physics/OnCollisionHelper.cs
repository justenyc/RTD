using UnityEngine;
using UnityEngine.Events;

public class OnCollisionHelper : MonoBehaviour
{
    //[Tooltip("How fast the rigidbody needs to be going in order to trigger OnCollisionEnter()")]
    //[SerializeField] float m_collisionVelocityThreshold = 0;

    public UnityEvent<Collision> OnCollisionEnterEvent;
    public UnityEvent<Collision> OnCollisionStayEvent;
    public UnityEvent<Collision> OnCollisionExitEvent;

    private void OnCollisionEnter(Collision collision)
    {
        //Logger.LogMessage($"CollisionEntered! {gameObject.name}");
        //Logger.LogMessage(OnCollisionEnterEvent.GetPersistentEventCount());
        OnCollisionEnterEvent.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollisionStayEvent?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollisionExitEvent?.Invoke(collision);
    }
}
