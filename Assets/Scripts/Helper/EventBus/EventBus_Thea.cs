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
        SwordAppear,
        SwordDisappear
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