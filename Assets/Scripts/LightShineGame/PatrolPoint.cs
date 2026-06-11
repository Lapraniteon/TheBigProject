using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{

    private enum PatrolShapes
    {
        Circle,
        LineHorizontal,
        LineVertical,
        FigureEight
    };

    public float completionThresholdX;
    
    [SerializeField] private PatrolShapes patrolShape;
    [SerializeField] private float movementSpeedMultiplier;

    public Transform respawnPoint;

    [Space]
    
    [ShowIf("patrolShape", PatrolShapes.Circle)]
    [Label("Radius")] [SerializeField] private float circleRadius = 5f;
    
    [ShowIf("patrolShape", PatrolShapes.FigureEight)]
    [Label("Radius")] [SerializeField] private float figureEightRadius = 5f;
    
    [ShowIf("patrolShape", PatrolShapes.LineVertical)]
    [Label("Length")] [SerializeField] private float lineLength = 5f;
    
    [ShowIf("patrolShape", PatrolShapes.LineHorizontal)]
    [Label("Length")] [SerializeField] private float lineWidth = 5f;

    public Vector3 GetPatrolStartPoint()
    {
        Vector3 patrolPoint = Vector3.zero;
        
        switch (patrolShape)
        {
            case PatrolShapes.Circle:
                patrolPoint = new Vector3(transform.position.x + circleRadius, transform.position.y, transform.position.z);
                break;
            case PatrolShapes.FigureEight:
                patrolPoint = transform.position;
                break;
            case PatrolShapes.LineVertical:
                patrolPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + lineLength / 2f);
                break;
            case PatrolShapes.LineHorizontal:
                patrolPoint = new Vector3(transform.position.x - lineWidth / 2f, transform.position.y, transform.position.z);
                break;
        }
        
        return patrolPoint;
    }

    public Sequence GetPatrolPatternMotion(Transform lightShineTransform)
    {
        if (movementSpeedMultiplier == 0) movementSpeedMultiplier = 1;
        
        switch (patrolShape)
        {
            case PatrolShapes.Circle:
                return GetPatrolPatternMotionCircle(lightShineTransform);
            case PatrolShapes.FigureEight:
                return GetPatrolPatternMotionFigureEight(lightShineTransform);
            case PatrolShapes.LineHorizontal:
                return GetPatrolPatternMotionLineHorizontal(lightShineTransform);
            case PatrolShapes.LineVertical:
                return GetPatrolPatternMotionLineVertical(lightShineTransform);
            default:
                return GetPatrolPatternMotionCircle(lightShineTransform);
        }
    }
    
    public Sequence GetPatrolPatternMotionCircle(Transform lightShineTransform)
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Insert(0f, lightShineTransform.DOLocalMoveZ(transform.position.z + circleRadius, 0.5f / movementSpeedMultiplier)
            .SetEase(Ease.OutSine));
        sequence.Insert(0f, lightShineTransform.DOLocalMoveX(transform.position.x - circleRadius, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(0.5f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveZ(transform.position.z - circleRadius, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(1f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveX(transform.position.x + circleRadius, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(1.5f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveZ(transform.position.z, 0.5f / movementSpeedMultiplier)
            .SetEase(Ease.InSine));
        sequence.SetLoops(-1, LoopType.Restart);

        return sequence;
    }
    
    public Sequence GetPatrolPatternMotionFigureEight(Transform lightShineTransform)
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Insert(0f, lightShineTransform.DOLocalMoveX(transform.position.x + figureEightRadius / 2f, 0.5f / movementSpeedMultiplier)
            .SetEase(Ease.OutSine));
        sequence.Insert(0f, lightShineTransform.DOLocalMoveZ(transform.position.z + figureEightRadius, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(0.5f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveX(transform.position.x - figureEightRadius / 2f, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(1f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveZ(transform.position.z, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(1.5f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveX(transform.position.x, 0.5f / movementSpeedMultiplier)
            .SetEase(Ease.InSine));
        
        sequence.Insert(2f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveX(transform.position.x + figureEightRadius / 2f, 0.5f / movementSpeedMultiplier)
            .SetEase(Ease.OutSine));
        sequence.Insert(2f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveZ(transform.position.z - figureEightRadius, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(2.5f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveX(transform.position.x - figureEightRadius / 2f, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(3f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveZ(transform.position.z, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(3.5f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveX(transform.position.x, 0.5f / movementSpeedMultiplier)
            .SetEase(Ease.InSine));
        
        sequence.SetLoops(-1, LoopType.Restart);

        return sequence;
    }
    
    public Sequence GetPatrolPatternMotionLineVertical(Transform lightShineTransform)
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Insert(0f, lightShineTransform.DOLocalMoveZ(transform.position.z - lineLength / 2f, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(1f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveZ(transform.position.z + lineLength / 2f, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.SetLoops(-1, LoopType.Restart);

        return sequence;
    }
    
    public Sequence GetPatrolPatternMotionLineHorizontal(Transform lightShineTransform)
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Insert(0f, lightShineTransform.DOLocalMoveX(transform.position.x + lineWidth / 2f, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.Insert(1f / movementSpeedMultiplier, lightShineTransform.DOLocalMoveX(transform.position.x - lineWidth / 2f, 1f / movementSpeedMultiplier)
            .SetEase(Ease.InOutSine));
        sequence.SetLoops(-1, LoopType.Restart);

        return sequence;
    }

    public Vector3 GetRespawnPoint()
    {
        Vector3 respawnPointReturn = respawnPoint.position;

        if (Physics.Raycast(respawnPoint.position, Vector3.down, out RaycastHit hit, 30f))
            respawnPointReturn = hit.point + Vector3.up;
        
        return respawnPointReturn;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(completionThresholdX, 0.5f, 20f), new Vector3(completionThresholdX, 0.5f, -20f));

        // Draw paths
        UnityEditor.Handles.color = Color.cyan;
        Gizmos.color = Color.cyan;
        switch (patrolShape)
        {
            case PatrolShapes.Circle:
                UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, circleRadius);
                break;
            case PatrolShapes.FigureEight:
                Vector3 topCircleCenter = new Vector3(transform.position.x, transform.position.y, transform.position.z + figureEightRadius / 2f);
                Vector3 bottomCircleCenter = new Vector3(transform.position.x, transform.position.y, transform.position.z - figureEightRadius / 2f);
                UnityEditor.Handles.DrawWireDisc(topCircleCenter, Vector3.up, figureEightRadius / 2f);
                UnityEditor.Handles.DrawWireDisc(bottomCircleCenter, Vector3.up, figureEightRadius / 2f);
                break;
            case PatrolShapes.LineHorizontal:
                Gizmos.DrawLine(transform.position - new Vector3(lineWidth / 2f, 0f, 0f), transform.position + new Vector3(lineWidth / 2f, 0f, 0f));
                break;
            case PatrolShapes.LineVertical:
                Gizmos.DrawLine(transform.position - new Vector3(0f, 0f, lineLength / 2f), transform.position + new Vector3(0f, 0f, lineLength / 2f));
                break;
        }
    }
}
