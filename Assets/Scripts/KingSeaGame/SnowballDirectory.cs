using UnityEngine;

public class SnowballDirectory : MonoBehaviour
{
    private Vector3 _playerPosition;
    private Vector3 _targetPosition;
    private Vector3 _direction;

    void Start()
    {
        
    }

    void FixDirection()
    {
        transform.rotation = Quaternion.LookRotation(_targetPosition - _playerPosition);
    }
}
