using UnityEngine;
using UnityEngine.Events;
using RotaryHeart.Lib.SerializableDictionary;

public class EventBus_Thea : EventBus
{
    [SerializeField] SerializableDictionaryBase<EventId, UnityEvent> eventContainer;

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
        ThrowItem
    }

    public UnityEvent GetEvent(EventId id)
    {
        return eventContainer.ContainsKey(id) ? eventContainer[id] : null;
    }

    public void InvokeEvent(EventId id)
    {
#if UNITY_EDITOR
        Debug.Log($"Invoking <color=yellow>{id}</color> from <color=cyan>{this}</color> located on <color=cyan>{this.name}</color>");
#endif
        if(eventContainer.ContainsKey(id))
        {
            eventContainer[id]?.Invoke();
        }
    }
}