using System;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        SetState();
    }

    public void SetState()
    {
        pauseMenu.SetActive(GameManager.Instance.GamePaused);
    }
}
