using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using FMODUnity;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class SpawningSnowballs : MonoBehaviour
{
    private KingSeaScript _kingSea;
    public GameObject snowball;
    [SerializeField] private Vector3 offset = new Vector3(2, 1, 0);
    private InputValue _value;
    private GameObject _snowballInstance;
    public int whichPlatform;
    public Vector3[] endPositions;

    private void OnEnable()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        //playerController.ShootSnowball += OnShoot;
        
        _kingSea = FindFirstObjectByType<KingSeaScript>();
        if (_kingSea == null)
            Debug.LogWarning("No KingSeaScript found");
    }

    private void OnDisable()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        //playerController.ShootSnowball -= OnShoot;
    }

    public void OnShoot()
    {
        ThrowSnowball(snowball);
    }

    private void ThrowSnowball(GameObject snowBall)
    {
        RuntimeManager.PlayOneShot("event:/SFX/KingSea/Throw");
        _snowballInstance = Instantiate(snowBall, transform.position + offset, Quaternion.identity);
        WhichPlatform();
        Debug.Log(endPositions[whichPlatform]);
        
        Sequence s = DOTween.Sequence();
        s.Append(_snowballInstance.transform.DOMove(endPositions[whichPlatform], 0.2f));
        if (DoesItLand())
        {
            Debug.Log("Snowball lands");
            s.InsertCallback(0.1f, () => RuntimeManager.PlayOneShot("event:/SFX/KingSea/Snowball Hit"));
            s.InsertCallback(0.1f, () => _kingSea?.TakingDamage());
        }
        else
        {
            s.InsertCallback(0.1f, () => RuntimeManager.PlayOneShot("event:/SFX/KingSea/Snowball Shield Hit"));
        }
        s.Play();
    }

    private void WhichPlatform()
    {
        if (transform.position.x <= 0)
        {
            if (transform.position.x <= -5)
            {
                whichPlatform = 0;
            }
            else
            {
                whichPlatform = 1;
            }
        } else if (transform.position.x > 0)
        {
            if (transform.position.x > 5)
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
            return whichPlatform == 2 || whichPlatform == 3;
        }
        if (MovingShieldScript.shieldPosition == 1)
        {
            return whichPlatform == 0 || whichPlatform == 3;
        }
        if (MovingShieldScript.shieldPosition == 2)
        {
            return whichPlatform == 0 || whichPlatform == 1;
        }
        return true;
    }
}
