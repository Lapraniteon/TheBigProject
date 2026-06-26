using UnityEngine;

public class BookScript: MonoBehaviour
{
    public string sceneToLoad;
    [SerializeField] 
    private GameObject interacationPopUp;

    private int playersInReach;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInReach++;
            ActivateInteracationPopUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInReach--;
            ActivateInteracationPopUp();
        }
    }

    private void ActivateInteracationPopUp()
    {
        if (playersInReach > 0)
        {
            interacationPopUp.SetActive(true);
        }
        else
        {
            interacationPopUp.SetActive(false);
        }
    }
}
