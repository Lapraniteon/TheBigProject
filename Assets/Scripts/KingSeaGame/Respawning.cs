using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = System.Numerics.Vector3;

public class Respawning : MonoBehaviour
{
    [SerializeField]private Transform respawnPoint;
    private PlayerInput _playerInput;
    private CharacterController _controller;
    private PlayerController _playerController;
    private SpawningSnowballs _spawningSnowballs;
    private bool _respawned;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.y < -10)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        DisableControls();
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        EnableControls();
    }

    private void DisableControls()
    {
        _playerInput.enabled = false;
        _controller.enabled = false;
    }
    
    private void EnableControls()
    {
        _playerInput.enabled = true;
        _controller.enabled = true;
    }
}
