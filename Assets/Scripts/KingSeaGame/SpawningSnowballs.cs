using System;
using Unity.VisualScripting;
using UnityEngine;

public class SpawningSnowballs : PlayerController
{
    public GameObject snowball;
    [SerializeField] private Vector3 offset = new Vector3(2, 1, 0);

    void Start()
    {
        Shoot();
    }
    public override void Shoot()
    {
        ThrowSnowball(snowball);
    }

    private void ThrowSnowball(GameObject snowBall)
    {
        
        Instantiate(snowBall, transform.position + offset, Quaternion.identity);
    }
}
