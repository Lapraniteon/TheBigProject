using UnityEngine;

public class LibraryHubUI : MonoBehaviour
{
    [SerializeField] private GameObject bigJoin;
    [SerializeField] private GameObject smallJoin;

    private void Start()
    {
        
    }

    private void DisableJoiningButton()
    {
        bigJoin.SetActive(false);
        smallJoin.SetActive(true);
    }

    private void OnEnable()
    {
        GameManager.PlayerJoin += DisableJoiningButton;
    }
}
