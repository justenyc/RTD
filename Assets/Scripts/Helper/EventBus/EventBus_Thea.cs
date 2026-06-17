using UnityEngine;
using UnityEngine.Events;
using RotaryHeart.Lib.SerializableDictionary;

public class EventBus_Thea : EventBus
{
    [SerializeField] SerializableDictionaryBase<EventId, UnityEvent> eventContainer;

    [Header("Debug")]
    [SerializeField] bool logEvents = false;

    [System.Serializable]
    public enum EventId
    {
        None,
        DisableMovement,
        EnableMovement,
        DisableThrow,
        EnableThrow,
        DisableInputs,
        EnableInputs,
        SwordAppear,
        SwordDisappear,
        CanCancel,
        ThrowItem,
        SwordHitboxActive,
        SwordHitboxInactive,
        EnableRootMotion,
        DisableRootMotion,
        DisableAnimatorRootMotion,
        EnableAnimatorRootMotion
    }

    public UnityEvent GetEvent(EventId id)
    {
        return eventContainer.ContainsKey(id) ? eventContainer[id] : null;
    }

    public void InvokeEvent(EventId id)
    {
        if(logEvents) Logger.LogMessage($"Invoking <color=yellow>{id}</color> from <color=cyan>{this}</color> located on <color=cyan>{this.name}</color>");

        if(eventContainer.ContainsKey(id))
        {
            eventContainer[id]?.Invoke();
        }
    }
}