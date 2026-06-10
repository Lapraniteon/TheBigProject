using System;
using System.Collections;
using UnityEngine;

public class DoorCollision : MonoBehaviour
{
    
    private PlayerController _controller;

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
        if (_controller == null) Debug.LogError($"No CharacterController attached to {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DariusDoor"))
            return;
        
        DariusLegsGameManager.Instance.EliminatePlayer(_controller);
    }

    
}
