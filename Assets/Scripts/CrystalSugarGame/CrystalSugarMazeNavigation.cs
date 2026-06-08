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
    [SerializeField] private DirectionalPad directionalPad;
    [SerializeField] private float directionPickWaitTime = 4f;
    [SerializeField] [ReadOnly] private float directionPickWaitTimer;
    [Space]
    [SerializeField] private float movementSpeedMultiplier = 1f;
    [SerializeField] private float rotateSpeedMultiplier = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _wallLayerMask = LayerMask.GetMask(wallLayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    private void Step()
    {
        if (IsMoving)
            return;
        
        StartCoroutine(StepCoroutine());
    }

    private IEnumerator StepCoroutine()
    {
        
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
            
            // Convert direction Vector2 to CS' local Vector3 direction.
            /// THIS ISNT WORKING RIGHT!! The picked Vector2 is correct, but it doesnt get converted correctly to the instruction for the player.
            Debug.Log($"Picked direction: {direction}");
            Vector3 directionVector = ConvertChosenDirectionToVector3(direction);
            Vector3 directionVectorLocal = Vector3Int.RoundToInt(transform.InverseTransformDirection(directionVector));
            
            // Check wall in that direction, if there is one, rotate 180 and go back.
            if (CheckWall(directionVectorLocal))
            {
                Rotate(180f);
                MoveStepForward();
                yield break;
            }

            if (directionVectorLocal == transform.forward)
                MoveStepForward();
            else if (directionVectorLocal == transform.right)
            {
                Rotate(90f);
                MoveStepForward();
            }
            else if (directionVectorLocal == -transform.right)
            {
                Rotate(-90f);
                MoveStepForward();
            }
            
            Debug.Log("End");
                
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
    }

    private Vector3 ConvertChosenDirectionToVector3(Vector2 direction)
    {
        return new Vector3(-direction.x, 0f, direction.y);
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
