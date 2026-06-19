using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Information")] 
    [SerializeField] 
    private Color32[] playerColors;
    [SerializeField]
    private Transform[] playerSpawnPoints;
    public List<PlayerController> players = new ();
    
    public String[] scenes;
    public Boolean[] minigamesWon;
    public String[] actionMaps;
    
    
    public static GameManager Instance;
    

    [SerializeField]
    private PlayerInputManager playerInputManager;
    
    
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FlipPlayerJoining(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            playerInputManager.EnableJoining();
        }
        else
        {
            playerInputManager.DisableJoining();
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
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("OnPlayerJoined");
        if (SceneManager.GetActiveScene().name == "LibraryHub")
        {
            Debug.Log("Library loaded");
            //Assign playerInput to the player array, name it and prepare it's number in collections.
            players.Add(playerInput.gameObject.GetComponent<PlayerController>());
            playerInput.gameObject.name = "Player " + players.Count; //Rename to the player number
            int playerNumber = players.Count - 1; //number to get the right variable from the arrays.
       
            //Set the colour of the player.
            playerInput.gameObject.GetComponent<Renderer>().material.color = playerColors[playerNumber];
       
            //Set the position of the joined player to the corresponding spawnpoint.
            playerInput.transform.position = playerSpawnPoints[playerNumber].transform.position;
            SceneManager.MoveGameObjectToScene(playerInput.gameObject, SceneManager.GetSceneByName("ManagerScene"));
            Physics.SyncTransforms(); //Makes sure the player teleports because the CharacterController often stops this.
            Debug.Log("Spawned Player " + playerNumber + " at " + playerSpawnPoints[playerNumber].transform.position);
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
            StartCoroutine(SceneController.Instance.LoadScene(interactableObject.GetComponent<BookScript>().sceneToLoad));
            Debug.Log(SceneController.Instance.LoadScene(interactableObject.GetComponent<BookScript>().sceneToLoad) + " loading");
        }
    }

    public void PlayersToSpawnPoints(Transform[] spawnPoints)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = spawnPoints[i].transform.position;
            Physics.SyncTransforms(); //Makes sure the player teleports because the CharacterController often stops this.
            Debug.Log(players[i].transform.position + spawnPoints[i].transform.position);
        }
    }

    public void FinishedMinigame()
    {
        string minigameScene = SceneManager.GetActiveScene().name;
        int sceneIndex = Array.IndexOf(scenes, minigameScene);
        minigamesWon[sceneIndex] = true;
        Debug.Log("Selected this scenenumber: " + sceneIndex);
        StartCoroutine(SceneController.Instance.LoadLibraryHub());
    }

    public void AdjustPlayerSpeed(float speed)
    {
        foreach (var player in players)
        {
            player.gameObject.GetComponent<PlayerController>().playerSpeed = speed;
        }
    }

    private void OnEnable()
    {
        PlayerController.Interaction += InteractionDetected;
    }

    private void OnDisable()
    {
        PlayerController.Interaction -= InteractionDetected;
    }
}


