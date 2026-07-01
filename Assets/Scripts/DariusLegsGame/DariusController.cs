using DG.Tweening;
using UnityEngine;

public class DariusController : MonoBehaviour
{

    [SerializeField] private Animator dariusAnimator;
    
    public void StartBackwardsMovement(float destinationZ, float duration)
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalMoveZ(destinationZ, duration).SetEase(Ease.Linear))
            .InsertCallback(sequence.Duration() - 1f, DariusLegsGameManager.Instance.FinalPlayerPush)
            .AppendCallback(MovementFinished);

        sequence.Play();
    }

    public void LookBack()
    {
        dariusAnimator.SetTrigger("LookBack");
    }

    private void MovementFinished()
    {
        DariusLegsGameManager.Instance.EndLevel();
    }
}
