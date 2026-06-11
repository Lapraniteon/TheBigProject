using UnityEngine;
using DG.Tweening;

public class MovingShieldScript : MonoBehaviour
{
    [SerializeField] private Vector3[] shieldPos;
    [SerializeField] private int transitionDuration;
    private int _shieldIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = shieldPos[0];
        _shieldIndex = 1;
    }

    private void ShieldMoving()
    {
        transform.DOMove(shieldPos[_shieldIndex],transitionDuration);
        _shieldIndex++;
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
