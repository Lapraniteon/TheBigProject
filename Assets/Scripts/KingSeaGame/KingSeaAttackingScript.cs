using UnityEngine;
using DG.Tweening;

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
        transform.DOMoveY(transform.position.y + 8, kingSeaAttackDuration).SetEase(Ease.OutFlash);
        Invoke("PlatformsSplitting", 4f);
    }

    //Split the platform
    private void PlatformsSplitting()
    {
        platform1.transform.DOMoveX(platform1.transform.position.x - splitDistance / 2, platformSplittingDuration).SetEase(Ease.OutFlash);
        platform2.transform.DOMoveX(platform2.transform.position.x + splitDistance / 2, platformSplittingDuration).SetEase(Ease.OutFlash);
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
