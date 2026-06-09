using UnityEngine;

public class ThrowingSnowballs : MonoBehaviour
{
    public GameObject snowball;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ThrowSnowball(GameObject snowBall)
    {
        Instantiate(snowBall, transform.position, Quaternion.identity);
    }
}
