using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public static event Action<GameObject> Interaction;
    public event Action ShootSnowball;

    public static event Action Continue;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;

    private Vector2 _movementInput = Vector2.zero;
    
    public List<PlayerControllerCollection> playerControllers = new ();
    
    
    public virtual void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
    }

    private void FixedUpdate()
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
        
        //assign joystick input from _movementInput assigned in OnMovement.
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

        //jump if player is jumping according to the OnJump method and is grounded.
        if (_jumped && _groundedPlayer)
        {
            Debug.Log("Jumping");
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravityValue);
        }
    }

    public void SwitchCurrentActionMap(string actionMapName)
    {
        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMapName);
        Debug.Log("Switched actionMap to " + actionMapName);
    }
    
    public void OnMovement(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        _jumped = value.isPressed;
    }

    public virtual void OnInteraction(InputValue value)
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

    public void StopMovement()
    {
        _movementInput = Vector2.zero;
        _playerVelocity = Vector2.zero;
    }
    
    public virtual void OnShoot(InputValue value)
    {
        ShootSnowball?.Invoke();
    }

    public void OnContinue(InputValue value)
    {
        Continue?.Invoke();
        Debug.Log("Continue Inputted");
    }

    #if UNITY_EDITOR
    //draws the sphere used to detect interactables.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
    #endif
    
    [System.Serializable]
    public class PlayerControllerCollection
    {
        public List<MonoBehaviour> players;
    }
    
    public void SetControllersActive(int index, bool active)
    {
        if (index >= playerControllers.Count)
            return;
        
        foreach (var script in playerControllers[index].players)
        {
            if (script != null)
            {
                script.enabled = active;
            }
        }
    }
}


