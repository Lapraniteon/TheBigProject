using UnityEngine;

public class Snowball : MonoBehaviour
{
    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        thrown();
        Invoke("DestorySnowball", 2);
    }

    private void thrown()
    {
        _rb.AddForce(transform.right * 500f);
    }

    private void DestorySnowball()
    {
        Destroy(this.gameObject);
    }
}
