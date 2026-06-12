using UnityEngine;
using DG.Tweening;

public class MovingShieldScript : MonoBehaviour
{
    [SerializeField] private Vector3[] shieldPos;
    [SerializeField] private int transitionDuration;
    private int _shieldIndex;
    public static int shieldPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shieldIndex = 1;
    }

    private void ShieldMoving()
    {
        transform.DOMove(shieldPos[_shieldIndex],transitionDuration);
        shieldPosition = WhichPlatform();
        _shieldIndex++;
    }

    private int WhichPlatform()
    {
        if (shieldPos[_shieldIndex].x > 1)
        {
            return 0;
        }
        if (shieldPos[_shieldIndex].x < -1)
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
