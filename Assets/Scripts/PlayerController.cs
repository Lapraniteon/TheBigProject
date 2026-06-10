using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    
    public float jumpHeight = 1.5f;
    private float _gravityValue = -9.81f;
    private bool _jumped = false;

    public float interactRadius = 5;
    private bool _interactionAttempted = false;
    public Action<GameObject> Interaction;
    
    private bool _shootAttempted = false;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;

    private Vector2 _movementInput = Vector2.zero;
    
    
    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        _interactionAttempted = context.action.triggered;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        _shootAttempted = context.action.triggered;
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        _jumped = context.action.triggered;
    }
    
    void Update()
    {
        // Slight downward velocity to keep grounded stable unless a jump is in progress.
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer)
        {
            if (_playerVelocity.y < -2f)
                _playerVelocity.y = -2f;
        }
        
        //Applies gravity to the player.
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
        
        //methods that are called based on the input system.
        Move();
        
        if (_groundedPlayer && _jumped) { Jump(); }
        
        if (_interactionAttempted) { Interact(); }
        
        if (_shootAttempted){Shoot();}
    }
    
    public virtual void Move()
    {
        //assign joystick input.
        float horizontal = _movementInput.x;
        float vertical = _movementInput.y;
        
        //assign them (correctly somehow) to the vector3 direction.
        Vector3 direction = new Vector3(-vertical, 0, horizontal);
        //if the vector isn't too small (controller would otherwise move when the joystick is in the middle).
        if (direction.sqrMagnitude > 0.1f)
        {
            //rotate player in accordance with the vector
            transform.localRotation = Quaternion.LookRotation(direction);
            //move forward (joystick input is weird seemingly) using the playerspeed.
            _controller.Move(transform.right * playerSpeed * Time.deltaTime);
        }
    }

    public virtual void Jump()
    {
        _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravityValue);
    }

    public virtual void Interact()
    {
        //find the interactables in range
        Collider[] hitInteractables = Physics.OverlapSphere(transform.position, interactRadius, LayerMask.GetMask("Interactable"));
        //sorts the array to the distance from the player to the center of the interactable.
        hitInteractables = hitInteractables.OrderBy(c => (c.transform.position - transform.position).sqrMagnitude).ToArray();
        //declare list to be used for arrays in view.
        List<Collider> seenInteractables = new List<Collider>();
        
        //check which interacatbales are in view using raycast.
        foreach(Collider collider in hitInteractables)
        {
            Debug.DrawRay(transform.position, collider.transform.position - transform.position, Color.red);
            if (Physics.Raycast(transform.position, collider.transform.position - transform.position, interactRadius))
            {
                seenInteractables.Add(collider);
            }
            
        }
        
        //Call the interact Action on the first interactable in the list (which should be the closest).
        if (seenInteractables.Count != 0)
        {
            Interaction?.Invoke(seenInteractables[0].gameObject);
            Debug.Log("Interacted with " + seenInteractables[0].gameObject.name);
        }
    }
    
    public virtual void Shoot()
    {
        
    }

    #if UNITY_EDITOR
    //draws the sphere used to detect interactables.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
    #endif
}


