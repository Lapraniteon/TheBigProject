using System;
using UnityEngine;
using DG.Tweening;

public class Wheel : MonoBehaviour
{
    [SerializeField] [Tooltip("Time in seconds to make 1 revolution.")] private float singleRotationTime;

    private void Start()
    {
        transform.DOLocalRotate(new Vector3(45f, 0f, 0f), singleRotationTime / 8f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
}
