using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //the target the gameObject follows.
    [SerializeField]
    private Transform target;
    
    //distance the gameObject keeps from the target.
    [SerializeField]
    private Vector3 offset;
    
    //refrence to the gameObject velocity during SmoothDamp for later use.
    private Vector3 velocity = Vector3.zero;
    
    //the time it takes for the gameObject to move to the target.
    [SerializeField] [Range(0.1f, 0.5f)]
    private float smoothTime = 0.12f;

    
    //gets run after Update.
    private void LateUpdate()
    {
        //calculates the position the gameObject should move to.
        Vector3 futurePosition = target.position + offset;
        //moves the gameObject from it's current position to the future position using the smoothTime float.
        transform.position = Vector3.SmoothDamp(transform.position, futurePosition, ref velocity, smoothTime);
    }
}
