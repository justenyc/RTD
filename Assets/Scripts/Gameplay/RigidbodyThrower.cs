using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RigidbodyThrower : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbodyToThrow;
    CollisionOverrides collisionOverrides = new();

    public class CollisionOverrides
    {
        public UnityAction<Collision> onEnter;
        public UnityAction<Collision> onStay;
        public UnityAction<Collision> onExit;

        public CollisionOverrides(UnityAction<Collision> _onEnter = null, UnityAction<Collision> _onStay = null, UnityAction<Collision> _onExit = null)
        {
            onEnter = _onEnter;
            onStay = _onStay;
            onExit = _onExit;
        }

        public void Reset()
        {
            onEnter = null;
            onStay = null;
            onExit = null;
        }
    }

    public RigidbodyThrower SetRigidbodyToThrow(Rigidbody rb)
    {
        rigidbodyToThrow = rb;
        return this;
    }

    public void ThrowRigidbody(Vector3 direction, ForceMode forceMode = ForceMode.Impulse, Rigidbody overrideRb = null)
    {
        if(overrideRb != null)
        {
            rigidbodyToThrow = overrideRb;
        }

        rigidbodyToThrow.AddForce(direction, forceMode);
        rigidbodyToThrow = null;
    }

    public void ThrowGameObject(GameObject go, Vector3 direction, ForceMode forceMode = ForceMode.Impulse)
    {
        if(go.TryGetComponent(out Rigidbody rb))
        {
            var goInstance = Instantiate(go, transform.position, Quaternion.identity);
            SetRigidbodyToThrow(goInstance.GetComponent<Rigidbody>());
            AddCollisionOverrides();
            ThrowRigidbody(direction, forceMode);

            var col = go.GetComponentInChildren<Collider>();
            if(col != null)
            {
                col.enabled = true;
            }
            collisionOverrides.Reset();
        }
    }

    public CollisionOverrides OverrideCollisionEnter(UnityAction<Collision> _onEnter)
    {
        collisionOverrides.onEnter = _onEnter;
        return collisionOverrides;
    }

    public CollisionOverrides OverrideCollisionStay(UnityAction<Collision> _onStay)
    {
        collisionOverrides.onStay = _onStay;
        return collisionOverrides;
    }

    public CollisionOverrides OverrideCollisionExit(UnityAction<Collision> _onExit)
    {
        collisionOverrides.onExit = _onExit;
        return collisionOverrides;
    }

    void AddCollisionOverrides()
    {
        if(rigidbodyToThrow.TryGetComponent(out OnCollisionHelper collisionHelper))
        {
            if (collisionOverrides.onEnter != null)
            {
                collisionHelper.OnCollisionEnterEvent.AddListener(collisionOverrides.onEnter);
            }

            if (collisionOverrides.onStay != null)
            {
                collisionHelper.OnCollisionEnterEvent.AddListener(collisionOverrides.onStay);
            }

            if (collisionOverrides.onExit != null)
            {
                collisionHelper.OnCollisionEnterEvent.AddListener(collisionOverrides.onExit);
            }
        }
    }
}