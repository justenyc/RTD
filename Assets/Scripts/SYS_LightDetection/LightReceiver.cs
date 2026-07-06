using UnityEngine;
using UnityEngine.Events;

public class LightReceiver : MonoBehaviour
{
    [SerializeField] Collider col;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask staticLightProbeMask;
    [Tooltip("How bright the sample from the lightmap needs to be to be considered lit")]
    [SerializeField] float litThreshold = 1f;

    [SerializeField] UnityEvent onLitEvent;
    [SerializeField] UnityEvent onUnlitEvent;

    [SerializeField] bool currentlyLit = false;

    private void OnValidate()
    {
        if (gameObject.layer != 11)
        {
            Logger.LogMessage($"Enforcing physics layer [LightProbe] for [{this.name}]");
            gameObject.layer = 11;
        }

        col = this.GetComponentInAll<Collider>();
        rb = this.GetComponentInAll<Rigidbody>();

        if (col == null) Logger.LogMessage($"No Collider found for LightProbe: {this.name}");
        if (rb == null) Logger.LogMessage($"No RigidBody found for LightProbe: {this.name}");
    }

    private void Awake()
    {
        LightingManager.singletonComplete += () =>
        {
            LightingManager.Instance.RegisterProbe(this);
        };
    }

    void OnEnable()
    {
        LightingManager.Instance?.RegisterProbe(this);
    }

    void OnDisable()
    {
        if (LightingManager.Instance != null)
            LightingManager.Instance.UnregisterProbe(this);
    }

    // Called by LightingManager once per scan interval with the result of scanning
    // all currently-overlapping lights for this probe.
    public void ApplyLitResult(bool lit)
    {
        currentlyLit = lit;
        if (lit == currentlyLit) return; // only fire events on state change
        Evaluate();
    }

    void Evaluate()
    {
        if (currentlyLit)
        {
            onLitEvent.Invoke();
            return;
        }

        onUnlitEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RealtimeLightProbeHelper probe))
        {
            LightingManager.Instance.OnLightEnteredProbe(this, probe);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out RealtimeLightProbeHelper probe))
        {
            LightingManager.Instance.OnLightExitedProbe(this, probe);
        }
    }
}
