using UnityEngine;

public class TargetGroupAxisLock : MonoBehaviour
{
    [SerializeField] private Transform targetGroup;
    [SerializeField] private Transform axisLockedTarget;

    void LateUpdate()
    {
        Vector3 pos = axisLockedTarget.position;

        pos.x = targetGroup.position.x;

        axisLockedTarget.position = pos;
    }
}
