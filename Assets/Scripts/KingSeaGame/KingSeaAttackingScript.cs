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
    
    private void Attacking()
    {
        transform.DOLocalRotate(new Vector3(90, 0, 0),kingSeaAttackDuration);
        Invoke("PlatformsSplitting", 0.2f);
    }

    private void PlatformsSplitting()
    {
        platform1.transform.DOMoveX(platform1.transform.position.x - splitDistance / 2, platformSplittingDuration).SetEase(Ease.OutFlash);
        platform2.transform.DOMoveX(platform2.transform.position.x + splitDistance / 2, platformSplittingDuration).SetEase(Ease.OutFlash);
    }
}
