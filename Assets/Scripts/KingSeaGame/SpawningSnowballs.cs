using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class SpawningSnowballs : PlayerController
{
    private KingSeaScript _kingSea;
    public GameObject snowball;
    [SerializeField] private Vector3 offset = new Vector3(2, 1, 0);
    private InputValue _value;
    private GameObject _snowballInstance;
    public int whichPlatform;
    public Vector3[] endPositions;

    public override void Start()
    {
        base.Start();
        _kingSea = FindFirstObjectByType<KingSeaScript>();
    }
    
    public override void OnShoot(InputValue value)
    {
        ThrowSnowball(snowball);
    }

    private void ThrowSnowball(GameObject snowBall)
    {
        _snowballInstance = Instantiate(snowBall, transform.position + offset, Quaternion.identity);
        WhichPlatform();
        Debug.Log(endPositions[whichPlatform]);
        
        Sequence s = DOTween.Sequence();
        s.Append(_snowballInstance.transform.DOMove(endPositions[whichPlatform], 0.2f));
        if (DoesItLand())
        {
            s.AppendCallback(() => _kingSea?.TakingDamage());
        }
        s.Play();
    }

    private void WhichPlatform()
    {
        if (transform.position.x > 0)
        {
            if (transform.position.x > 5)
            {
                whichPlatform = 0;
            }
            else
            {
                whichPlatform = 1;
            }
        } else if (transform.position.x < 0)
        {
            if (transform.position.x < -5)
            {
                whichPlatform = 3;
            }
            else
            {
                whichPlatform = 2;
            }
        }
    }

    private bool DoesItLand()
    {
        if (MovingShieldScript.shieldPosition == 0)
        {
            if (whichPlatform == 0)
            {
                return false;
            } 
            if (whichPlatform == 1)
            {
                return false;
            } 
            return true;
        }
        if (MovingShieldScript.shieldPosition == 1)
        {
            if (whichPlatform == 1)
            {
                return false;
            } 
            if (whichPlatform == 2)
            {
                return false;
            } 
            return true;
        }
        if (MovingShieldScript.shieldPosition == 2)
        {
            if (whichPlatform == 2)
            {
                return false;
            } 
            if (whichPlatform == 3)
            {
                return false;
            } 
            return true;
        }
        return true;
    }
}
