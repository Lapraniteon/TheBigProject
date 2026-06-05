using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

[RequireComponent(typeof(CheckCollisionWithLight))]
public class PatrolController : MonoBehaviour
{
    
    private LightShineGameManager _lightShineGameManager;
    private CheckCollisionWithLight _checkCollisionWithLight;
    
    [SerializeField] private PatrolPoint[] patrolPoints;
    private Queue<PatrolPoint> _patrolPointsStack;
    private PatrolPoint _currentPatrolPoint;

    private Sequence _currentPatrolSequence;

    private void Start()
    {
        _lightShineGameManager = FindFirstObjectByType<LightShineGameManager>();
        _checkCollisionWithLight = GetComponent<CheckCollisionWithLight>();

        _patrolPointsStack = new Queue<PatrolPoint>(patrolPoints);
    }

    private void Update()
    {
        if (_currentPatrolPoint is null)
            return;
        
        // Check if all players are past the threshold of the current patrol area
        bool passedThreshold = true;
        foreach (PlayerController player in _lightShineGameManager.players)
        {
            if (player.transform.position.x <= _currentPatrolPoint.completionThresholdX)
            {
                passedThreshold = false;
                break;
            }
        }
        
        if (passedThreshold)
            MoveToNextPatrolPoint();
    }

    [NaughtyAttributes.Button]
    public void MoveToNextPatrolPoint()
    {
        //_currentPatrolPoint = _currentPatrolPoint is null ? _patrolPointsStack.Peek() : _patrolPointsStack.Dequeue();
        _currentPatrolPoint = _patrolPointsStack.Dequeue();
        StartCoroutine(StartPatrolSequenceCoroutine(_currentPatrolPoint));
    }

    private IEnumerator StartPatrolSequenceCoroutine(PatrolPoint patrolPoint)
    {
        _checkCollisionWithLight.detectionEnabled = false;
        _currentPatrolSequence?.Kill();
        
        Tween tween = transform.DOMove(patrolPoint.GetPatrolStartPoint(), 2f).SetEase(Ease.InOutBack);

        yield return tween.WaitForCompletion();
        
        _checkCollisionWithLight.detectionEnabled = true;
        _currentPatrolSequence = patrolPoint.GetPatrolPatternMotion(transform).Play();
    }
}
