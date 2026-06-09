using UnityEngine;

public class Snowball : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
