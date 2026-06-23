using System;
using DG.Tweening;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    // private Rigidbody _rb;
    // private Vector3 _endposition;
    void Start()
    {
        // _rb = GetComponent<Rigidbody>();
        Thrown();
        Invoke("DestroySnowball", 2);
    }

    private void Thrown()
    {
        //transform.DOMove(_endposition, 0.2f);
        // _rb.AddForce(transform.forward * -500f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("KingSeaDeath"))
            return;
        
        DestroySnowball();
    }

    private void DestroySnowball()
    {
        Destroy(this.gameObject);
    }
}
