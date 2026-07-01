using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<PlayerController> players = new ();
    public float dariusPlayerSpeed = 15f;
    public float playerSpeed = 6.5f;
    
    public String[] scenes;
    public Boolean[] minigamesWon;
    public String[] actionMaps;

    [SerializeField]
    private int endScreenWaitTime = 3;
    [SerializeField]
    private GameObject endscreen;
    [SerializeField]
    private Sprite[] endScreens;
    
    
    public static GameManager Instance;
    

    [SerializeField]
    private PlayerInputManager playerInputManager;
    
    public bool firstPlayerJoin = false;
    
    public bool GamePaused { get; private set; }
    [SerializeField] private PauseMenuController pauseMenu;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FlipPlayerJoining(string sceneName)
    {
        if (sceneName == "LibraryHub")
        {
            PlayerInputManager.instance.EnableJoining();
        }
        else
        {
            PlayerInputManager.instance.DisableJoining();
        }
    }

    public void SwitchActionMaps(string sceneName)
    {
        int sceneIndex = Array.IndexOf(scenes, sceneName);
        if (sceneIndex > 4)
        {
            sceneIndex = 4;
        }
        foreach (var player in players)
        {
            string actionMapName = actionMaps[sceneIndex];
            player.gameObject.GetComponent<PlayerController>().SwitchCurrentActionMap(actionMapName);
        }
    }

    public void SwitchActionMapsByName(string actionMapName)
    {
        foreach (var player in players)
        {
            player.gameObject.GetComponent<PlayerController>().SwitchCurrentActionMap(actionMapName);
        }
    }

    public void SwitchPlayerControllers(string sceneName)
    {
        int sceneIndex = Array.IndexOf(scenes, sceneName);
        int sceneAmount = scenes.Length;
        foreach (var player in players)
        {
            for (int i = 0; i < sceneAmount; i++)
            {
                if (sceneIndex == i)
                {
                    player.gameObject.GetComponent<PlayerController>().SetControllersActive(i, true);
                }
                else
                {
                    player.gameObject.GetComponent<PlayerController>().SetControllersActive(i, false);
                }
            }
        }
    }
    
    

    public void PlayerInputsActive(bool playerInputsActive)
    {
        foreach (var player in players)
        {
            PlayerInput playerInput = player.gameObject.GetComponent<PlayerInput>();
            playerInput.enabled = playerInputsActive;
        }
    }
    private void InteractionDetected(GameObject interactableObject)
    {
        if (interactableObject.CompareTag("Book"))
        {
            StartCoroutine(SceneController.Instance.LoadScene(interactableObject.GetComponentInParent<BookScript>().sceneToLoad));
            //Debug.Log(interactableObject.GetComponent<BookScript>().sceneToLoad + " loading");
        }
    }

    public void PlayersToSpawnPoints(Transform[] spawnPoints)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = spawnPoints[i].transform.position;
            players[i].transform.rotation = spawnPoints[i].transform.rotation;
            Physics.SyncTransforms(); //Makes sure the player teleports because the CharacterController often stops this.
            //Debug.Log(players[i].transform.position + spawnPoints[i].transform.position);
        }
    }

    public IEnumerator FinishedMinigame()
    {
        string minigameScene = SceneManager.GetActiveScene().name;
        int sceneIndex = Array.IndexOf(scenes, minigameScene);

        //if (sceneIndex >= minigamesWon.Length)
            //return;
            
        endscreen.SetActive(true);
        endscreen.GetComponent<Image>().sprite = endScreens[sceneIndex];

        yield return new WaitForSeconds(endScreenWaitTime);
        
        endscreen.SetActive(false);
        minigamesWon[sceneIndex] = true;
        //Debug.Log("Selected this scenenumber: " + sceneIndex);
        StartCoroutine(SceneController.Instance.LoadScene("LibraryHub"));
    }

    public void AdjustPlayerSpeed(float speed)
    {
        foreach (var player in players)
        {
            player.gameObject.GetComponent<PlayerController>().playerSpeed = speed;
        }
    }

    public void SetCarActive(bool active)
    {
        foreach (PlayerController player in players)
        {
            GameObject car = player.gameObject.transform.GetChild(1).gameObject;
            Debug.Log(car.name);
            car.SetActive(active);
            
            // Make the player sit or stand up
            player.playerAnimator.SetFloat("Sitting", active ? 1 : 0);
        }
    }

    public void TogglePause()
    {
        SetPause(!GamePaused);
    }

    public void SetPause(bool state)
    {
        GamePaused = state;
        pauseMenu.SetState();

        Time.timeScale = state ? 0f : 1f;
        
        if (GamePaused)
            SwitchActionMapsByName("Pause Menu");
        else
            SwitchActionMaps(SceneManager.GetActiveScene().name);
    }

    private void ResetAndRestartEntireGame()
    {
        Debug.Log("Restarting entire game");
        firstPlayerJoin = false;
        endscreen.SetActive(false);
        for (int i = 0; i < minigamesWon.Length; i++)
        {
            minigamesWon[i] = false;
        }

        foreach (var player in players)
        {
            Destroy(player.gameObject);
        }
        players.Clear();
        SetPause(false);
        SceneController.Instance.Start();
    }

    private void OnEnable()
    {
        PlayerController.Interaction += InteractionDetected;
        PlayerController.Pause += TogglePause;
        PlayerController.RestartGame += ResetAndRestartEntireGame;
    }

    private void OnDisable()
    {
        PlayerController.Interaction -= InteractionDetected;
        PlayerController.Pause -= TogglePause;
        PlayerController.RestartGame -= ResetAndRestartEntireGame;
    }
    
    public static bool IsFmodEventPlaying(EventInstance instance)
    {
        PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state == PLAYBACK_STATE.PLAYING;
    }
}


