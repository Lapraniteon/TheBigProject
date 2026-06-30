using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
            cameraTargetGroup.AddMember(player.transform, 2f / players.Count, 1f);
        }
        
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
        
        DOTween.Sequence()
            .AppendInterval(2f)
            .AppendCallback(StartPatrol)
            .Play();
        
    }

    public void StartPatrol()
    {
        patrolController.MoveToNextPatrolPoint();
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCoroutine());
    }

    private IEnumerator EndLevelCoroutine()
    {
        RuntimeManager.PlayOneShot("event:/BGM/MUS_VictorySting");
        
        DOTween.Sequence()
            .AppendInterval(2f)
            .AppendCallback(() => StartCoroutine(GameManager.Instance.FinishedMinigame()))
            .Play();
        
        yield return null;
    }
}
