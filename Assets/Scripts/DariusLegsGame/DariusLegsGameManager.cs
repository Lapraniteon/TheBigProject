using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using UnityEngine;

public class DariusLegsGameManager : MonoBehaviour
{
    
    private static DariusLegsGameManager _instance; // Game Manager singleton pattern

    [Space] 
    [SerializeField] private float respawnDelay;
    [Space]

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
        
        RuntimeManager.PlayOneShot("event:/SFX/DariusLegs/Grumble");
    }

    public void EliminatePlayer(PlayerController player)
    {
        StartCoroutine(EliminatePlayerCoroutine(player));
    }
    
    private IEnumerator EliminatePlayerCoroutine(PlayerController player)
    {
        Debug.Log("Player got hit!");
        
        RuntimeManager.PlayOneShot("event:/SFX/DariusLegs/Player Hit Door");

        //player.gameObject.SetActive(false); // Replace with animation at some point
        DOTween.Sequence()
            .Append(player.transform.DOMove(player.transform.position + new Vector3(10f, 10f, -25f), .9f).SetEase(Ease.OutBack))
            .Join(player.transform.DOLocalRotateQuaternion(player.transform.rotation * Quaternion.Euler(-90f, 0f, -165f), .9f).SetEase(Ease.OutBack))
            .AppendCallback(() => player.gameObject.SetActive(false))
            .Play();
        
        // Pause gate spawning, to make sure the player doesnt need to immediately dodge a new gate after respawning
        //worldMovement.SetGateSpawnPaused(true);
        
        yield return new WaitForSeconds(respawnDelay); // Replace with animation at some point
        
        player.transform.rotation = Quaternion.identity;
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
        
        //worldMovement.SetGateSpawnPaused(false);
    }

    public void EndLevel()
    {
        bool allEliminated = true;
        foreach (PlayerController player in players) // Check if there is at least one active player left.
        {
            allEliminated = player.gameObject.activeSelf ? false : allEliminated;
            player.gameObject.SetActive(true);
        }
        
        Debug.Log("Players won! Darius reached");
        worldMovement.StopAllMovement();
        RuntimeManager.PlayOneShot("event:/BGM/MUS_VictorySting");
        GameManager.Instance.AdjustPlayerSpeed(playerSpeed);
        
        DOTween.Sequence()
            .AppendInterval(3f)
            .AppendCallback(() => GameManager.Instance.FinishedMinigame())
            .Play();
    }
    
}
