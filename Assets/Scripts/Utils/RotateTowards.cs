using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    public Transform targetTransform;
    Vector3 target;

    void Update()
    {
        target = new Vector3(targetTransform.position.x, this.transform.position.y, targetTransform.position.z);
        transform.LookAt(target);
    }
}
