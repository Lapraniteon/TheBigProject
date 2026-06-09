using DG.Tweening;
using UnityEngine;

public class DariusController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBackwardsMovement(float destinationZ, float duration)
    {
        transform.DOLocalMoveZ(destinationZ, duration).SetEase(Ease.Linear);
    }
}
