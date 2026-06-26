using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorLeft;
    [SerializeField] private Transform doorRight;

    public void CloseDoor()
    {
        doorLeft.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.InOutBack);
        doorRight.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.InOutBack);
    }
}
