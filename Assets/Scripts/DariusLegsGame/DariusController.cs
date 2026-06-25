using DG.Tweening;
using UnityEngine;

public class DariusController : MonoBehaviour
{
    public void StartBackwardsMovement(float destinationZ, float duration)
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalMoveZ(destinationZ, duration).SetEase(Ease.Linear))
            .AppendCallback(MovementFinished);

        sequence.Play();
    }

    private void MovementFinished()
    {
        DariusLegsGameManager.Instance.EndLevel();
    }
}
