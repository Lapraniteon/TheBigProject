using Unity.Cinemachine;
using UnityEngine;

[SaveDuringPlay]
public class LockCameraAxes : CinemachineExtension
{
    [Header("Lock Position Axes")]
    public bool lockX = false;
    public bool lockY = true;
    public bool lockZ = true;

    public float y;

    private Vector3 _lockedPosition;
    private bool _initialized;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        /*// Only modify the camera after the Body stage has finished.
        if (stage != CinemachineCore.Stage.Body)
            return;

        if (!_initialized || deltaTime < 0)
        {
            _lockedPosition = state.RawPosition;
            _initialized = true;
        }

        Vector3 pos = state.RawPosition;

        if (lockX)
            pos.x = _lockedPosition.x;
        else
            _lockedPosition.x = pos.x;

        if (lockY)
            pos.y = _lockedPosition.y;
        else
            _lockedPosition.y = pos.y;

        if (lockZ)
            pos.z = _lockedPosition.z;
        else
            _lockedPosition.z = pos.z;

        state.RawPosition = pos;*/
        
        if (stage == CinemachineCore.Stage.Finalize)
        {
            var pos = state.RawPosition;
            pos.y = y;
            state.RawPosition = pos;
        }
    }
}