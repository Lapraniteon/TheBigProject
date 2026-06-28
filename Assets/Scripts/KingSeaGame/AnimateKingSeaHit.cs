using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class AnimateKingSeaHit : MonoBehaviour
{

    [SerializeField] private Transform animatedTransform;

    private void OnEnable()
    {
        KingSeaScript.KingSeaTakesDamage += Hit;
    }

    private void OnDisable()
    {
        KingSeaScript.KingSeaTakesDamage -= Hit;
    }

    [Button]
    private void Hit(float damage = 0f)
    {
        DOTween.Sequence()
            .Append(animatedTransform.DOLocalRotate(new Vector3(-2f, 180f, 0f), .07f))
            .Append(animatedTransform.DOLocalRotate(new Vector3(0f, 180f, 0f), .14f))
            .Play();
    }
}
