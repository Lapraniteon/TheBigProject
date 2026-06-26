using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorLeft;
    [SerializeField] private Transform doorRight;

    private Collider _doorCollider;

    private void Start()
    {
        _doorCollider = GetComponent<Collider>();
        _doorCollider.enabled = false;
    }
    
    public void CloseDoor()
    {
        doorLeft.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.InOutBack);
        doorRight.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.InOutBack);
        _doorCollider.enabled = true;
    }
}
