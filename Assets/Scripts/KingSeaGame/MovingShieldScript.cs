using UnityEngine;
using DG.Tweening;
using FMODUnity;
using NaughtyAttributes;

public class MovingShieldScript : MonoBehaviour
{
    [SerializeField] private Vector3[] shieldPos;
    [SerializeField] private int transitionDuration;
    private int _shieldIndex;
    public static int shieldPosition;

    [SerializeField] private Transform shieldMesh;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shieldIndex = 1;
        shieldPosition = 0;
    }

    private void ShieldMoving()
    {
        RuntimeManager.PlayOneShot("event:/SFX/KingSea/Move Shield");
        transform.DOMove(shieldPos[_shieldIndex], transitionDuration);
        shieldPosition = WhichPlatform();
        _shieldIndex++;
    }

    [Button]
    private void ShakeShield()
    {
        shieldMesh.transform.DOPunchPosition(Random.onUnitCircle * 0.05f, .2f, 100);
    }

    private int WhichPlatform()
    {
        if (shieldPos[_shieldIndex].x < -1)
        {
            return 0;
        }
        if (shieldPos[_shieldIndex].x > 1)
        {
            return 2;
        }
        return 1;
    }
    
    private void OnEnable()
    {
        KingSeaScript.SwitchingShieldPosition += ShieldMoving;
        SpawningSnowballs.HitShield += ShakeShield;
    }

    private void OnDisable()
    {
        KingSeaScript.SwitchingShieldPosition -= ShieldMoving;
        SpawningSnowballs.HitShield -= ShakeShield;
    }
}
