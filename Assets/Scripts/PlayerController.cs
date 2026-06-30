using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    
    public float jumpHeight = 1.5f;
    [SerializeField] private float _gravityValue = -9.81f;
    private bool _jumped = false;

    public float interactRadius = 5;
    public static event Action<GameObject> Interaction;
    public event Action ShootSnowball;

    public static event Action Continue;

    public static event Action Pause;
    public static event Action ReturnToLibrary;
    public static event Action RestartGame;

    private CharacterController _controller;
    private Vector3 _playerVelocity;

    private Vector2 _movementInput = Vector2.zero;
    
    public List<PlayerControllerCollection> playerControllers = new ();

    [SerializeField]
    private GameObject playerModel;
    [SerializeField] 
    public Animator playerAnimator;
    [SerializeField]
    private GameObject carModel;
    
    
    public virtual void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        playerModel = gameObject.transform.Find("playerModel").gameObject;
        carModel = gameObject.transform.Find("carModel").gameObject;
    }
    
    /// <summary>
    /// Input handling
    /// </summary>
    /// <param name="value"></param>

    public void OnMovement(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        Debug.Log("Jumping Inputted");
        _jumped = value.isPressed;
    }
    
    public virtual void OnShoot(InputValue value)
    {
        Debug.Log("Shooting");
        ShootSnowball?.Invoke();
    }

    public void OnContinue(InputValue value)
    {
        Continue?.Invoke();
        Debug.Log("Continue Inputted");
    }

    public void OnPause(InputValue value)
    {
        Pause?.Invoke();
        Debug.Log("Pause");
    }
    
    public void OnButtonReturnToLibrary(InputValue value)
    {
        ReturnToLibrary?.Invoke();
        Debug.Log("Return to Library");
    }
    
    public void OnButtonRestart(InputValue value)
    {
        RestartGame?.Invoke();
        Debug.Log("Restart Inputted");
    }

    public virtual void OnInteraction(InputValue value)
    {
        Debug.Log("Interaction Inputted");
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
            Debug.Log(seenInteractables.Count);
            Interaction?.Invoke(seenInteractables[0].gameObject);
            //Debug.Log("Interacted with " + seenInteractables[0].gameObject.name);
        }
    }
    
    /// <summary>
    /// Movement
    /// </summary>
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f, LayerMask.GetMask("Ground")))
        {
            if (_jumped)
            {
                _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravityValue);
                RuntimeManager.PlayOneShot("event:/SFX/KingSea/Jump", transform.position);
                playerAnimator.SetTrigger("Jump");
                _jumped = false;
            }
        }
        else
        {
            _playerVelocity.y += _gravityValue * Time.deltaTime;
        }
        
        _controller.Move(_playerVelocity * Time.deltaTime);
        
        //assign joystick input from _movementInput assigned in OnMovement.
        float horizontal = _movementInput.x;
        float vertical = _movementInput.y;
        
        //assign them (correctly somehow) to the vector3 direction.
        Vector3 direction = new Vector3(-vertical, 0, horizontal);
        Vector3 dariusDirection = new Vector3(horizontal, 0, 0);
        if (direction.sqrMagnitude > 0.1f)
        {
            //movement specific to Darius
            if (SceneManager.GetActiveScene().name == "DariusLegsMinigame")
            {
                _controller.Move(dariusDirection * playerSpeed * Time.deltaTime);
                //rotate left
                if (dariusDirection.x == -1)
                {
                    transform.DOLocalRotate(new Vector3(0, -120, 0), 0.2f);
                }
                //rotate right
                else if (dariusDirection.x == 1)
                {
                    transform.DOLocalRotate(new Vector3(0, -60, 0), 0.2f);
                }
            }
            //normal movement
            else
            {
                //rotate player in accordance with the vector
                transform.localRotation = Quaternion.LookRotation(direction);
                //move forward (joystick input is weird seemingly) using the playerspeed.
                _controller.Move(transform.right * playerSpeed * Time.deltaTime);
                playerAnimator.SetBool("IsWalking", true);
                //if the vector isn't too small (controller would otherwise move when the joystick is in the middle).
            }
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
        }
        //rotate back to neutral in Darius
        if (SceneManager.GetActiveScene().name == "DariusLegsMinigame")
        {
            if (dariusDirection.x == 0)
            {
                transform.DOLocalRotate(new Vector3(0, -90, 0), 0.2f);
            }
        }
    }
    
    
    public void StopMovement()
    {
        _movementInput = Vector2.zero;
        _playerVelocity = Vector2.zero;
    }
    
    /// <summary>
    /// Input Switching
    /// </summary>
    /// <param name="actionMapName"></param>
    
    public void SwitchCurrentActionMap(string actionMapName)
    {
        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMapName);
        Debug.Log("Switched actionMap to " + actionMapName);
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

    public void RespawnActive(bool respawn)
    {
        playerModel.SetActive(!respawn);
        if (SceneManager.GetActiveScene().name == "DariusLegsMinigame")
        {
            carModel.SetActive(!respawn);
            playerAnimator.SetFloat("Sitting", 1f);
        }
        if (respawn)
        {
            StopMovement();
        }
        Physics.SyncTransforms();
    }
    
    [System.Serializable]
    public class PlayerControllerCollection
    {
        public List<MonoBehaviour> players;
    }
    
    #if UNITY_EDITOR
    //draws the sphere used to detect interactables.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
    #endif
}


