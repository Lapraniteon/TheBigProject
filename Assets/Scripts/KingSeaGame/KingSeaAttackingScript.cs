using UnityEngine;
using DG.Tweening;
using FMODUnity;

public class KingSeaAttackingScript : MonoBehaviour
{
    [SerializeField]private float kingSeaAttackDuration;
    [SerializeField]private float platformSplittingDuration;
    [SerializeField]private GameObject platform1;
    [SerializeField]private GameObject platform2;
    [SerializeField] private float splitDistance;

    private void Start()
    {
        Attacking();
    }
    
    //Move tentacle in position
    private void Attacking()
    {
        Sequence attackSequence = DOTween.Sequence();
        attackSequence.Append(transform.DOMoveY(transform.position.y + 8, kingSeaAttackDuration).SetEase(Ease.OutFlash))
            .InsertCallback(0.5f, () => RuntimeManager.PlayOneShot("event:/SFX/KingSea/Charge"))
            .InsertCallback(3.7f, PlatformsSplitting)
            .InsertCallback(3.3f, () => RuntimeManager.PlayOneShot("event:/SFX/KingSea/Slash & Hit Platform"));

        attackSequence.Play();
    }

    //Split the platform
    private void PlatformsSplitting()
    {
        platform1.transform.DOMoveX(platform1.transform.position.x - splitDistance / 2, platformSplittingDuration).SetEase(Ease.OutSine);
        platform2.transform.DOMoveX(platform2.transform.position.x + splitDistance / 2, platformSplittingDuration).SetEase(Ease.OutSine);
        Retreating();
    }

    //move the tentacle down again
    private void Retreating()
    {
        transform.DOMoveY(transform.position.y - 15, kingSeaAttackDuration).SetEase(Ease.OutFlash);
        Invoke("DisableArm", 2f);
    }

    //disable the arm again.
    private void DisableArm()
    {
        transform.gameObject.SetActive(false);
    }
}
