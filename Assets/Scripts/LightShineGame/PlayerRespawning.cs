using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerRespawning : MonoBehaviour
{

    private LightShineGameManager _gameManager;

    private void Start()
    {
        _gameManager = GetComponent<LightShineGameManager>();
    }

    public void EliminatePlayer(PlayerController player)
    {
        Debug.Log("Player Enter Light");
        
        StartCoroutine(EliminatePlayerCoroutine(player));
    }

    private IEnumerator EliminatePlayerCoroutine(PlayerController player)
    {
        /*PlayerInput playerInput = player.GetComponent<PlayerInput>();
        playerInput.enabled = false;*/
        
        player.StopMovement();
        
        player.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);

        player.transform.position = _gameManager.patrolController.GetRespawnPoint();
        player.gameObject.SetActive(true);
        /*playerInput.enabled = true;*/
    }

    private void OnEnable()
    {
        CheckCollisionWithLight.OnDetectPlayer += EliminatePlayer;
    }

    private void OnDisable()
    {
        CheckCollisionWithLight.OnDetectPlayer -= EliminatePlayer;
    }
}
