using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Sequence = DG.Tweening.Sequence;

[RequireComponent(typeof(CheckCollisionWithLight))]
public class PatrolController : MonoBehaviour
{
    
    private LightShineGameManager _lightShineGameManager;
    private CheckCollisionWithLight _checkCollisionWithLight;
    
    [SerializeField] private PatrolPoint[] patrolPoints;
    private Queue<PatrolPoint> _patrolPointsQueue;
    private PatrolPoint _currentPatrolPoint;

    private Sequence _currentPatrolSequence;
    
    [Header("Events")]
    [SerializeField] private UnityEvent<PatrolPoint> onEndCurrentPatrolPoint;
    [SerializeField] private UnityEvent<PatrolPoint> onStartNewPatrolPoint;

    private void Start()
    {
        _lightShineGameManager = FindFirstObjectByType<LightShineGameManager>();
        _checkCollisionWithLight = GetComponent<CheckCollisionWithLight>();

        _patrolPointsQueue = new Queue<PatrolPoint>(patrolPoints);
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
        onEndCurrentPatrolPoint?.Invoke(_currentPatrolPoint);

        if (_patrolPointsQueue.Count <= 0)
            return;
        
        _currentPatrolPoint = _patrolPointsQueue.Dequeue();
        StartCoroutine(StartPatrolSequenceCoroutine(_currentPatrolPoint));
        
        onStartNewPatrolPoint?.Invoke(_currentPatrolPoint);
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
