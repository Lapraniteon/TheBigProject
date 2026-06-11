using DG.Tweening;
using UnityEngine;

public class GateSet : MonoBehaviour
{

    public Sequence movementSequence;

    [SerializeField] private Transform[] doors;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        foreach (Transform door in doors)
        {
            door.gameObject.SetActive(false);
        }
    }

    public void StartMovement(float despawnDistance, float globalMovementSpeed, float doorClosingDistance)
    {
        movementSequence = DOTween.Sequence();
        float totalDistance = transform.position.z + despawnDistance;
        movementSequence
            .Append(transform.DOLocalMoveZ(-despawnDistance, totalDistance / globalMovementSpeed)
                .SetEase(Ease.Linear))
            .InsertCallback((totalDistance - despawnDistance - doorClosingDistance) / globalMovementSpeed, CloseDoors)
            .AppendCallback(DestroySelf);

        movementSequence.Play();
    }

    private void CloseDoors()
    {
        int openDoor = Random.Range(0, doors.Length);
        CloseDoorsExcept(openDoor);
    }

    private void CloseDoorsExcept(int doorIndex)
    {
        foreach (Transform door in doors)
        {
            door.gameObject.SetActive(true);
        }
        
        doors[doorIndex].gameObject.SetActive(false);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
