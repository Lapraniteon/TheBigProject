using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    
    public float moveSpeed;
    
    public Vector2 movementDirection;
    
    public InputActionReference moveAction;


    private void Update()
    {
        movementDirection = moveAction.action.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementDirection.x * moveSpeed, 0, movementDirection.y * moveSpeed);
    }
}
