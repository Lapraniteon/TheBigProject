using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class LightShineGameManager : MonoBehaviour
{

    public List<PlayerController> players = new ();

    public PatrolController patrolController;

    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            foreach (var player in GameManager.Instance.players)
            {
                players.Add(player.GetComponent<PlayerController>());
            }
        }

        foreach (PlayerController player in players)
        {
            cameraTargetGroup.AddMember(player.transform, 1f, 1f);
        }
        
        StartPatrol();
    }

    public void StartPatrol()
    {
        patrolController.MoveToNextPatrolPoint();
    }

    public void EndLevel()
    {
        throw new NotImplementedException();
    }
}
