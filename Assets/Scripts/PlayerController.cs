using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    // [Header("Input Actions")]
    // public InputActionReference moveAction;
    // public InputActionReference jumpAction;

    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //jumped = context.ReadValue<bool>();
        jumped = context.action.triggered;
    }
    
    // private void OnEnable()
    // {
    //     moveAction.action.Enable();
    //     jumpAction.action.Enable();
    // }
    //
    // private void OnDisable()
    // {
    //     moveAction.action.Disable();
    //     jumpAction.action.Disable();
    // }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }
        
        //Move the player
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        if (controller.enabled)
            controller.Move(move * Time.deltaTime * playerSpeed);

        // Player jumps
        if (groundedPlayer && jumped)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        playerVelocity.y += gravityValue * Time.deltaTime;
        
        if (controller.enabled)
            controller.Move(playerVelocity * Time.deltaTime);
    }
}
