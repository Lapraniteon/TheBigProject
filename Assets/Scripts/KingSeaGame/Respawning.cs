using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = System.Numerics.Vector3;

public class Respawning : MonoBehaviour
{
    [SerializeField]private Transform respawnPoint;
    private PlayerInput _playerInput;
    private CharacterController _controller;
    private SpawningSnowballs _spawningSnowballs;
    private bool _respawned;

    [SerializeField] private ParticleSystem splashParticles;
    
    private KingSeaScript _kingSeaScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _kingSeaScript = FindFirstObjectByType<KingSeaScript>();
        if (_kingSeaScript == null)
            Debug.LogWarning("No KingSeaScript found");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterPlane") || other.CompareTag("KingSeaArm"))
        {
            RuntimeManager.PlayOneShot("event:/SFX/KingSea/Fall in Water");
            Instantiate(splashParticles, transform.position, Quaternion.identity);
            _kingSeaScript.Laugh();
            
            if (other.CompareTag("KingSeaArm"))
                Respawn();
        }
        
        if (other.CompareTag("KingSeaDeath"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        if (_kingSeaScript != null)
        {
            //DisableControls();
        
            // Check which player I am
            PlayerController controller = GetComponent<PlayerController>();
            if (controller == null)
                return;
            
            int index = GameManager.Instance.players.IndexOf(controller);
        
            transform.position = _kingSeaScript.spawnPoints[index].position;
            transform.rotation = _kingSeaScript.spawnPoints[index].rotation;
        
            Physics.SyncTransforms();
            //EnableControls();
        }
        else
            Debug.LogError("KingSeaScript not found. Respawn failed.");
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
