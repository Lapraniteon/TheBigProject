using System;
using UnityEngine;

public class LightShineGameManager : MonoBehaviour
{

    public PlayerController[] players;

    public PatrolController patrolController;

    private void Start()
    {
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
