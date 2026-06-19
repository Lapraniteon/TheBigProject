using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DariusLegsGameManager : MonoBehaviour
{
    
    private static DariusLegsGameManager _instance; // Game Manager singleton pattern

    private float dariusPlayerSpeed = 15f;
    private float playerSpeed = 5f;
    public static DariusLegsGameManager Instance
    {
        get
        {
            if (_instance is null) // Error checking in case the Game Manager is not assigned
                Debug.LogError("GameManager is null!");

            return _instance;
        }
    } // Game Manager instance property
    
    public List<PlayerController> players = new ();
    [SerializeField]
    private Transform[] spawnPoints;
    
    [HideInInspector] public WorldMovement worldMovement;
    
    private void Awake()
    {
        _instance = this;
    }
    
    void Start()
    {
        
        players = GameManager.Instance.players;
        
        worldMovement = FindFirstObjectByType<WorldMovement>();
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
        
        GameManager.Instance.AdjustPlayerSpeed(dariusPlayerSpeed);
    }

    public void EliminatePlayer(PlayerController player)
    {
        StartCoroutine(EliminatePlayerCoroutine(player));
    }
    
    private IEnumerator EliminatePlayerCoroutine(PlayerController player)
    {
        Debug.Log("Player got hit!");

        player.gameObject.SetActive(false); // Replace with animation at some point
        
        // Pause gate spawning, to make sure the player doesnt need to immediately dodge a new gate after respawning
        worldMovement.SetGateSpawnPaused(true);
        
        yield return new WaitForSeconds(3f); // Replace with animation at some point
        
        player.transform.rotation = Quaternion.identity;
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
        
        worldMovement.SetGateSpawnPaused(false);
    }

    public void EndLevel()
    {
        bool allEliminated = true;
        foreach (PlayerController player in players) // Check if there is at least one active player left.
            allEliminated = player.gameObject.activeSelf ? false : allEliminated;
        
        Debug.Log("Players won! Darius reached");
        worldMovement.StopAllMovement();
        GameManager.Instance.FinishedMinigame();
        GameManager.Instance.AdjustPlayerSpeed(playerSpeed);
    }
    
}
