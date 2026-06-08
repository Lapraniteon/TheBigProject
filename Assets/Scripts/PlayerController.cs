using System;
using System.Collections.Generic;
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
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (_playerVelocity.y < -2f)
                _playerVelocity.y = -2f;
        }
        
        Move();
        
        if (_groundedPlayer && _jumped) { Jump(); }
        
        if (_interactionAttempted) { Interact(); }
        
        if (_shootAttempted){Shoot();}

        // Move
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    public virtual void Move()
    {
        //Move the player
        Vector3 move = new Vector3(_movementInput.x, 0, _movementInput.y);
        _controller.Move(move * Time.deltaTime * playerSpeed);
    }

    public virtual void Jump()
    {
        _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravityValue);
    }

    public virtual void Interact()
    {
        Collider[] hitInteractables = Physics.OverlapSphere(transform.position, interactRadius, LayerMask.GetMask("Interactable"));
        List<Collider> seenInteractables = new List<Collider>();
        
        
        foreach(Collider collider in hitInteractables)
        {
            Debug.DrawRay(transform.position, collider.transform.position - transform.position, Color.red);
            if (Physics.Raycast(transform.position, collider.transform.position - transform.position, interactRadius))
            {
                seenInteractables.Add(collider);
            }
            
            if (seenInteractables.Count != 0)
            {
                Interaction?.Invoke(collider.gameObject);
            }
        }
    }

    public virtual void Shoot()
    {
        
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
    #endif
}


