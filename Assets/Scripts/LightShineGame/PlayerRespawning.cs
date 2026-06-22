using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerRespawning : MonoBehaviour
{

    private LightShineGameManager _gameManager;

    [SerializeField] private Camera mainCamera;
    private Coroutine[] countingDown;
    [SerializeField] [Label("Off-screen elimination delay")] private float eliminationDelay;

    private void Start()
    {
        _gameManager = GetComponent<LightShineGameManager>();
        countingDown = new Coroutine[_gameManager.players.Count];
    }

    private void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        for (var index = 0; index < _gameManager.players.Count; index++)
        {
            var player = _gameManager.players[index];
            if (GeometryUtility.TestPlanesAABB(planes, player.GetComponent<Collider>().bounds))
            {
                // Inside view
                if (countingDown[index] == null) 
                    continue;
                
                StopCoroutine(countingDown[index]);
                countingDown[index] = null;
            }
            else
            {
                // Outside view
                if (countingDown[index] != null) 
                    continue;
                
                countingDown[index] = StartCoroutine(EliminatePlayerAfterDelay(_gameManager.players[index]));
            }
                
        }
    }

    private IEnumerator EliminatePlayerAfterDelay(PlayerController player)
    {
        yield return new WaitForSeconds(eliminationDelay);
        
        EliminatePlayer(player);
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
