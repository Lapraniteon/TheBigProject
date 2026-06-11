using UnityEngine;

public class Snowball : MonoBehaviour
{
    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Thrown();
        Invoke("DestroySnowball", 2);
    }

    private void Thrown()
    {
        _rb.AddForce(transform.forward * -500f);
    }

    private void DestroySnowball()
    {
        Destroy(this.gameObject);
    }
}
