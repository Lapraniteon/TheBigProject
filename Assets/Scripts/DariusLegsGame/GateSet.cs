using DG.Tweening;
using FMODUnity;
using UnityEngine;

public class GateSet : MonoBehaviour
{

    public Sequence movementSequence;

    [SerializeField] private Door[] doors;

    public DariusController dariusController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        /*foreach (Transform door in doors)
        {
            door.gameObject.SetActive(false);
        }*/
    }

    public void StartMovement(float despawnDistance, float spawnHeightOffset, float globalMovementSpeed, float doorClosingDistance)
    {
        movementSequence = DOTween.Sequence();
        float totalDistance = transform.position.z + despawnDistance;
        movementSequence
            .Append(transform.DOLocalMoveZ(-despawnDistance, totalDistance / globalMovementSpeed)
                .SetEase(Ease.Linear))
            .Join(transform.DOLocalMoveY(transform.position.y - spawnHeightOffset, totalDistance / globalMovementSpeed / 3f)
                .SetEase(Ease.OutSine))
            .InsertCallback((totalDistance - despawnDistance - doorClosingDistance) / globalMovementSpeed, CloseDoors)
            .AppendCallback(() =>
            {
                if (Random.Range(0f, 1f) <= 0.5f)
                    dariusController.LookBack();
            })
            .AppendCallback(DestroySelf);

        movementSequence.Play();
    }

    private void CloseDoors()
    {
        int openDoor = Random.Range(0, doors.Length);
        CloseDoorsExcept(openDoor);
        
        Invoke(nameof(CloseDoorSFX), .25f);
        Invoke(nameof(CloseDoorSFX), .25f + .03f);
    }
    
    private void CloseDoorSFX() => RuntimeManager.PlayOneShot("event:/SFX/DariusLegs/Door Close");

    private void CloseDoorsExcept(int doorIndex)
    {
        for (var index = 0; index < doors.Length; index++)
        {
            if (index == doorIndex)
                continue;
            
            var door = doors[index];
            door.CloseDoor();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
