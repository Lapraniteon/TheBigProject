using UnityEngine;
using DG.Tweening;

public class MovingShieldScript : MonoBehaviour
{
    private Vector3[] _shieldPos;
    [SerializeField] private int transitionDuration;
    private int _shieldIndex;
    public static int shieldPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shieldPos = new Vector3[3];
        _shieldPos[0] = transform.position;
        _shieldPos[1] = transform.position - new Vector3(transform.position.x, 0, 0);
        _shieldPos[2] = transform.position - new Vector3(2 * transform.position.x, 0, 0);
        _shieldIndex = 1;
    }

    private void ShieldMoving()
    {
        transform.DOMove(_shieldPos[_shieldIndex],transitionDuration);
        shieldPosition = WhichPlatform();
        _shieldIndex++;
    }

    private int WhichPlatform()
    {
        if (_shieldPos[_shieldIndex].x > 1)
        {
            return 0;
        }
        if (_shieldPos[_shieldIndex].x < -1)
        {
            return 2;
        }
        return 1;
    }
    
    private void OnEnable()
    {
        KingSeaScript.SwitchingShieldPosition += ShieldMoving;
    }

    private void OnDisable()
    {
        KingSeaScript.SwitchingShieldPosition += ShieldMoving;
    }
}
