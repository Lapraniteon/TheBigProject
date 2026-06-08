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

        void MoveStepForward() => move.Append(transform
            .DOLocalMove(transform.position + transform.forward, 1f / movementSpeedMultiplier).SetEase(Ease.Linear));
        
        void Rotate(float degrees) => move.Append(transform
            .DOLocalRotate(transform.localRotation.eulerAngles + new Vector3(0f, degrees, 0f), 1f / rotateSpeedMultiplier));

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

        IsMoving = true;
        move.Play();
        yield return move.WaitForCompletion();
        IsMoving = false;
    }

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
