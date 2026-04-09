using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TorqueApplicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector3 torqueAxis;
    [Range(0, 1000)]
    [SerializeField] float strength;
    [SerializeField] Rigidbody bodyToAffect;

    [Header("When")]
    [SerializeField] bool onEnable = false;
    [SerializeField] bool onStart = false;
    [SerializeField] bool onUpdate = false;

    private void Awake()
    {
        if(bodyToAffect == null)
        {
            if(TryGetComponent(out Rigidbody rb))
            {
                bodyToAffect = rb;
            }
            Debug.LogError($"No <color=cyan>Rigidbody</color> found on {gameObject}'s <color=cyan>TorqueApplicator</color>");
        }
    }

    void Start()
    {
        if(onStart)
        {
            Debug.Log($"{transform.right} + {torqueAxis} = {transform.right + torqueAxis}");
            bodyToAffect.AddRelativeTorque(torqueAxis * strength);
        }
    }

    private void OnEnable()
    {
        if(onEnable)
        {
            bodyToAffect.AddRelativeTorque(transform.right + torqueAxis * strength);
        }
    }

    void Update()
    {
        if (onUpdate)
        {
            bodyToAffect.AddRelativeTorque(transform.right + torqueAxis * strength * Time.deltaTime);
        }
    }
}
