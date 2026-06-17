using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CrystalSugarMazeNavigation : MonoBehaviour
{

    [Header("Wall Checks")] 
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] [Layer] [Label("Wall Layer")] private string wallLayer;
    private LayerMask _wallLayerMask;

    [Header("Movement")] 
    public bool IsMoving { get; private set; }
    public bool MazeEndReached { get; private set; }
    private bool _stepRunning;
    private bool _doSteps;
    
    [SerializeField] private DirectionalPad directionalPad;
    [SerializeField] private float directionPickWaitTime = 4f;
    [SerializeField] [ReadOnly] private float directionPickWaitTimer;
    [Space]
    [SerializeField] private float movementSpeedMultiplier = 1f;
    [SerializeField] private float rotateSpeedMultiplier = 1f;
    [SerializeField] private float stepDelay = 1f;

    private Coroutine _currentStep;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _wallLayerMask = LayerMask.GetMask(wallLayer);

        StartMovement(); // Replace with an event that starts the movement at some point
    }
    
    public void StartMovement() => _doSteps = true;

    private void Update()
    {
        if (IsMoving || MazeEndReached || !_doSteps || _stepRunning)
            return;
        
        Step();
    }

    [Button]
    private void Step()
    {
        _currentStep = StartCoroutine(StepCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("CS_MazeExit"))
            return;
        
        MazeEndReached = true;
        Debug.Log("Maze end reached!");
    }

    private IEnumerator StepCoroutine()
    {
        _stepRunning = true;
        
        Sequence move = DOTween.Sequence();

        void MoveStepForward() => move.AppendCallback(() => transform.DOLocalMove(transform.position + transform.forward, 1f / movementSpeedMultiplier).SetEase(Ease.Linear));
        
        void Rotate(float degrees) => move.Append(transform
            .DOLocalRotate(transform.localRotation.eulerAngles + new Vector3(0f, degrees, 0f), 1f / rotateSpeedMultiplier));

        void NormalPathfind()
        {
            if (CheckWall(transform.forward))
            {
                if (CheckWall(-transform.right))
                {
                    if (CheckWall(transform.right))
                    {
                        Debug.Log("turning around and moving back");
                        // Rotate 180, move step
                        Rotate(180f);
                        MoveStepForward();
                    }
                    else
                    {
                        Debug.Log("rotating and moving right");
                        // Rotate to right, move step
                        Rotate(90f);
                        MoveStepForward();
                    }
                }
                else
                {
                    Debug.Log("rotating and moving left");
                    // Rotate to left, move step
                    Rotate(-90f);
                    MoveStepForward();
                }
            }
            else
            {
                // Move step
                Debug.Log("moving forward");
                MoveStepForward();
            }
        }

        IEnumerator WaitForPlayersToDecideDirection()
        {

            Vector2 direction = directionalPad.ChosenDirection();

            for (directionPickWaitTimer = 0f; directionPickWaitTimer < directionPickWaitTime; directionPickWaitTimer += Time.deltaTime) // Check if the chosen direction changes in the meantime.
            {
                yield return null; // Wait 1 frame

                Vector2 newDirection = directionalPad.ChosenDirection();
                
                if (direction != newDirection)
                {
                    // Interrupt and restart timer
                    directionPickWaitTimer = 0f;
                    direction = newDirection;
                    Debug.Log("Decision timer interrupted");
                }
            }
            
            Debug.Log($"Picked direction: {direction}");
            // Convert the chosen direction to an angle based on CS' current orientation
            Vector3 directionVector = ConvertChosenDirectionToVector3(direction);
            float angle = Vector3.SignedAngle(Vector3Int.RoundToInt(transform.forward), directionVector, Vector3.up);

            if (direction == Vector2.zero)
            {
                NormalPathfind();
                yield break;
            }
            
            if (CheckWall(directionVector)) // If there is a wall in the chosen direction...
                angle = 180f; // ...turn back.

            // Queue the move
            switch (Mathf.RoundToInt(angle))
            {
                case 0:
                    MoveStepForward();
                    break;
                case 90:
                    Rotate(90f);
                    MoveStepForward();
                    break;
                case -90:
                    Rotate(-90f);
                    MoveStepForward();
                    break;
                case 180:
                case -180:
                    Rotate(180f);
                    MoveStepForward();
                    break;
            }
            
            //Debug.Log("End");
                
        }

        int wallCount = 0;

        if (CheckWall(transform.forward)) wallCount++;
        if (CheckWall(transform.right)) wallCount++;
        if (CheckWall(-transform.right)) wallCount++;

        if (wallCount <= 1) // If it is at least a 3-way or 4-way intersection
        {
            // Wait until player input
            Debug.Log("3-way or 4-way");
            
            // Read current player input & store
            Debug.Log("Waiting for player direction decision...");
            yield return StartCoroutine(WaitForPlayersToDecideDirection());
        }
        else
        {
            NormalPathfind();
        }

        IsMoving = true;
        move.Play();
        yield return move.WaitForCompletion();
        IsMoving = false;
        
        if (stepDelay != 0f) yield return new WaitForSeconds(stepDelay);
        
        _stepRunning = false;
    }

    private Vector3 ConvertChosenDirectionToVector3(Vector2 direction)
    {
        return new Vector3(direction.x, 0f, direction.y);
    }

    /// <summary>
    /// Returns true if there is a wall in the passed in direction.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool CheckWall(Vector3 direction)
    {
        return Physics.Raycast(raycastOrigin.position, direction, /*out RaycastHit hit,*/ 1f, _wallLayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 2f); // Direction CS is facing

        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastOrigin.position, transform.forward);
        Gizmos.DrawRay(raycastOrigin.position, transform.right);
        Gizmos.DrawRay(raycastOrigin.position, -transform.right);
    }
}
