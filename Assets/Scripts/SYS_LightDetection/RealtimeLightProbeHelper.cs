using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(SphereCollider))]
public class RealtimeLightProbeHelper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LightType lightType;
    [SerializeField] LayerMask raycastMask;
    [SerializeField] Light myLight;
    [SerializeField] SphereCollider sphereCollider;

    [Header("Debug")]
    [SerializeField] bool overrideOnValidate = false;

    private void OnValidate()
    {
        if (overrideOnValidate)
        {
            return;
        }

        lightType = GetComponent<Light>().type;
        myLight = GetComponent<Light>();
        gameObject.layer = 12;
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = myLight.dilatedRange;
        sphereCollider.isTrigger = true;
    }

    public bool Scan(Transform samplePoint)
    {
        switch(lightType)
        {
            case LightType.Spot: return SpotlightScan(samplePoint, transform, myLight);
            case LightType.Point: return PointLightScan(samplePoint, transform, myLight);
        }

        Logger.LogWarning($"Light Type on {this.name} was invalid");
        return true;
    }

    bool SpotlightScan(Transform sampleTransform, Transform lightTransform, Light light)
    {
        Vector3 toLight = lightTransform.position - sampleTransform.position;
        float angle = Vector3.Angle(lightTransform.forward, -toLight);
                
        if (angle > light.spotAngle * 0.5f) return false;

        return FireRay(lightTransform.position, -toLight, sampleTransform);
    }

    bool PointLightScan(Transform sampleTransform, Transform lightTransform, Light light)
    {
        return FireRay(lightTransform.position, sampleTransform.position - lightTransform.position, sampleTransform);
    }

    bool FireRay(Vector3 origin, Vector3 direction, Transform sampleTransform)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, myLight.range, raycastMask))
        {
            Color debugColor = hit.collider.transform == sampleTransform ? Color.green : Color.red;
            Logger.DrawRay(origin, hit.point - origin, debugColor);
            return hit.collider.transform == sampleTransform;
        }

        Logger.DrawRay(origin, direction, Color.red);
        return false;
    }
}