using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralizes all light-probe scanning. Instead of each LightReactor driving its own
/// OnTriggerStay raycasts every physics frame, this manager runs one scheduled pass
/// at a fixed interval, only for probes that currently have at least one light in range.
/// </summary>
public class LightingManager : MonoBehaviour
{
    public static LightingManager Instance { get; private set; }

    [Header("Scan Settings")]
    [Tooltip("How often (seconds) the lit/unlit state is re-evaluated.")]
    [Range(0.01f, 1f)]
    [SerializeField] float scanInterval = 0.15f;

    [Tooltip("Spread probe scans across multiple frames instead of all at once.")]
    [SerializeField] int probesPerBatch = 25;

    // All probes currently in the scene, registered on enable.
    readonly List<LightReceiver> activeProbes = new();

    // Per-probe set of lights currently overlapping it (filled by trigger enter/exit on the probe).
    readonly Dictionary<LightReceiver, HashSet<RealtimeLightProbeHelper>> probeLights = new();

    public static System.Action singletonComplete;

    Coroutine scanRoutine;

    void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        singletonComplete?.Invoke();
    }

    void OnDisable()
    {
        StopScanRoutine();
    }

    public void RegisterProbe(LightReceiver probe)
    {
        activeProbes.Add(probe);
        probeLights[probe] = new HashSet<RealtimeLightProbeHelper>();

        if (activeProbes.Count > 0) StartScanRoutine();
    }

    public void UnregisterProbe(LightReceiver probe)
    {
        activeProbes.Remove(probe);
        probeLights.Remove(probe);

        if (activeProbes.Count < 1) StopScanRoutine();
    }

    void StartScanRoutine()
    {
        if (scanRoutine != null) return; // already running
        scanRoutine = StartCoroutine(ScanLoop());
    }

    void StopScanRoutine()
    {
        if (scanRoutine == null) return; // already stopped
        StopCoroutine(scanRoutine);
        scanRoutine = null;
    }

    public void OnLightEnteredProbe(LightReceiver probe, RealtimeLightProbeHelper light)
    {
        if (probeLights.TryGetValue(probe, out var set)) set.Add(light);
    }

    public void OnLightExitedProbe(LightReceiver probe, RealtimeLightProbeHelper light)
    {
        if (probeLights.TryGetValue(probe, out var set)) set.Remove(light);
    }

    IEnumerator ScanLoop()
    {
        var wait = new WaitForSeconds(scanInterval);

        while (true)
        {
            int processed = 0;

            foreach (var probe in activeProbes)
            {
                if (!probeLights.TryGetValue(probe, out var lights) || lights.Count == 0)
                {
                    // No lights nearby at all — guaranteed unlit, skip raycasting entirely.
                    probe.ApplyLitResult(false);
                    continue;
                }

                bool lit = false;
                foreach (var light in lights)
                {
                    if (light.Scan(probe.transform))
                    {
                        lit = true;
                        break; // early-out on first hit, same as before
                    }
                }

                probe.ApplyLitResult(lit);

                processed++;
                if (processed >= probesPerBatch)
                {
                    processed = 0;
                    yield return null; // spread remaining probes over subsequent frames
                }
            }

            yield return wait;
        }
    }
}
