using System;
using System.Collections.Generic;
using FMODUnity;
using Unity.Cinemachine;
using UnityEngine;

public class LightShineGameManager : MonoBehaviour
{

    public List<PlayerController> players = new ();

    public PatrolController patrolController;

    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;
    
    [SerializeField]
    private Transform[] spawnPoints;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            players = GameManager.Instance.players;
        }

        foreach (PlayerController player in players)
        {
            cameraTargetGroup.AddMember(player.transform, 1f, 1f);
        }
        
        StartPatrol();
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
    }

    public void StartPatrol()
    {
        patrolController.MoveToNextPatrolPoint();
    }

    public void EndLevel()
    {
        RuntimeManager.PlayOneShot("event:/BGM/MUS_VictorySting");
        GameManager.Instance.FinishedMinigame();
    }
}
