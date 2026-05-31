using Player;
using UnityEngine;

public class ProceduralWaistBend : MonoBehaviour
{
    [Header("References")]
    public Transform waistBone;
    public Transform characterRoot;
    public PlayerController playerController;
    [Range(0,1)]
    public float weight = 1;

    [Header("Settings")]
    public float verticalAimAngle = 0f;
    public float smoothSpeed = 8f;

    //private float _currentAngle;

    void LateUpdate()
    {
        verticalAimAngle = playerController.cameraFollow.transform.rotation.eulerAngles.x * weight;
        //_currentAngle = Mathf.Lerp(_currentAngle, verticalAimAngle, Time.deltaTime * smoothSpeed);

        Quaternion tilt = Quaternion.AngleAxis(verticalAimAngle, characterRoot.right);
        waistBone.rotation = tilt * waistBone.rotation;
    }
}