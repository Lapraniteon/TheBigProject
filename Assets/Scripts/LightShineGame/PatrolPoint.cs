using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{

    private enum PatrolShapes
    {
        Circle,
        Line,
        FigureEight
    };

    public float completionThresholdX;
    
    [SerializeField] private PatrolShapes patrolShape;
    [SerializeField] private float movementSpeedMultiplier;

    [Space]
    
    [ShowIf("patrolShape", PatrolShapes.Circle)]
    [Label("Radius")] [SerializeField] private float circleRadius;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPatrolStartPoint()
    {
        Vector3 patrolPoint = Vector3.zero;
        
        switch (patrolShape)
        {
            case PatrolShapes.Circle:
                patrolPoint = new Vector3(transform.position.x + circleRadius, transform.position.y, transform.position.z);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(completionThresholdX, 0.5f, 20f), new Vector3(completionThresholdX, 0.5f, -20f));

        // Draw paths
        UnityEditor.Handles.color = Color.cyan;
        switch (patrolShape)
        {
            case PatrolShapes.Circle:
                UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, circleRadius);
                break;
            case PatrolShapes.Line:
                break;
            case PatrolShapes.FigureEight:
                break;
        }
    }
}
