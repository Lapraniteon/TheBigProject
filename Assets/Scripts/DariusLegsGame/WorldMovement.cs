using DG.Tweening;
using UnityEngine;

public class WorldMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] [Tooltip("Movement speed of the scene in units/second.")] private float globalMovementSpeed;
    
    [Header("Floor")]
    [SerializeField] private Transform floorTransform;
    private Tween _floorMovementTween;

    [Header("Gates")] 
    [SerializeField] private Transform gateSpawnPoint;
    [SerializeField] private GateSet gatePrefab;
    [SerializeField] [Tooltip("Spawn interval in units.")] private float gateSpawnInterval;
    private float _despawnDistance = 20f;
    [SerializeField] [Tooltip("Distance behind Darius at which the doors close.")] private float doorClosingDistance;
    private Sequence _gateSpawningLoop;
    
    [Header("Darius")]
    [SerializeField] private DariusController darius;
    [SerializeField] [Tooltip("Time it takes for the player to have \"caught\" up to Darius.")] private float catchupTime;
    [SerializeField] private float finalCatchDistance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartMovement();
        StartGateSpawning();
        
        darius.StartBackwardsMovement(finalCatchDistance, catchupTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartMovement()
    {
        _floorMovementTween = floorTransform.DOLocalMoveZ(-5f, 5f / globalMovementSpeed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void SetGateSpawnPaused(bool setPaused)
    {
        if (setPaused)
            _gateSpawningLoop?.Pause();
        else
            _gateSpawningLoop?.Play();
    }

    private void StartGateSpawning()
    {
        _gateSpawningLoop = DOTween.Sequence();
        _gateSpawningLoop
            .AppendCallback(SpawnGate)
            .AppendInterval(gateSpawnInterval / globalMovementSpeed)
            .SetLoops(-1, LoopType.Restart);

        _gateSpawningLoop.Play();
    }

    private void SpawnGate()
    {
        Vector3 offset = new Vector3(0f, -8f, 0f);
        GateSet gate = Instantiate(gatePrefab, gateSpawnPoint.position + offset, Quaternion.identity);
        gate.StartMovement(_despawnDistance, offset.y, globalMovementSpeed, darius.transform.position.z - doorClosingDistance);
    }

    public void StopAllMovement()
    {
        _floorMovementTween?.Kill();
        _gateSpawningLoop?.Kill();
    }
}
