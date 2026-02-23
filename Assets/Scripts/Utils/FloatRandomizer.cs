using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class FloatRandomizer : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The minimum number that the randomizer will include in its rolls")]
    [SerializeField] float randomMin;
    [Tooltip("The maximum number that the randomizer will include in its rolls")]
    [SerializeField] float randomMax;
    [Tooltip("How many frames will pass before the Randomizer will pass the Float To Pass at a rate of 60FPS")]
    [SerializeField] int tickRate = 0;
    [Tooltip("How many frames will pass before the Randomizer will randomize a new float for the Float To Pass to Lerp to")]
    [SerializeField] int randomizationRate = 0;
    [Tooltip("How quickly Float To Pass will transition to the new randomized float")]
    [SerializeField] float lerpStrength = 1;
    [Space(30)]
    [SerializeField] UnityEvent<float> OnFloatPass;

    [Header("Debug")]
    [Tooltip("The float to passed by the randomizer")]
    [SerializeField] float floatToPass;
    [Tooltip("The randomized float that Float To Pass will lerp towards")]
    [SerializeField] float randomizedFloat;
    [SerializeField] int tick = 0;
    [SerializeField] int randomizationTick = 0;

    private void OnValidate()
    {
        if(randomMin > randomMax)
        {
            float max = randomMin;
            randomMin = randomMax;
            randomMax = max;
            Debug.LogWarning($"<color=yellow>Warning!</color> Reassigning Random Min and Random Max because Min was bigger than Max!");
        }
    }
    
    private void FixedUpdate()
    {
        if(randomizationTick >= randomizationRate)
        {
            randomizedFloat = Random.Range(randomMin, randomMax);
            randomizationTick = 0;
        }

        if(tick >= tickRate)
        {
            OnFloatPass?.Invoke(floatToPass);
            tick = 0;
        }

        floatToPass = Mathf.Lerp(floatToPass, randomizedFloat, Time.fixedDeltaTime * lerpStrength);

        tick++;
        randomizationTick++;
    }
}
