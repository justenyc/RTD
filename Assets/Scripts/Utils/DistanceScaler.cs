using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.Events;

public class DistanceScaler : MonoBehaviour
{
    [SerializeField] Transform compareTo;
    [SerializeField] float min = 0;
    [SerializeField] float max = 100;
    [Range(0,2)]
    [SerializeField] float multiplier = 1;
    [Tooltip("How many frames to wait before next update")]
    [SerializeField] int tickRate;

    [SerializeField] UnityEvent<float> scaledDistance;
    
    int tickTime;

    private void Awake()
    {
        tickTime = tickRate;
    }

    private void FixedUpdate()
    {
        if(compareTo == null)
        {
            return;
        }

        tickTime -= 1;
        if (tickTime <= 0)
        {
            tickTime = tickRate;
            float f = Mathf.Clamp(Vector3.Distance(compareTo.position, transform.position), min, max);
            scaledDistance?.Invoke(f * multiplier);
        }

    }
}
