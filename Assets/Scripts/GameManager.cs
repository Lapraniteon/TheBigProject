using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = Unity.Mathematics.Random;

public class GameManager : MonoBehaviour
{
    [Header("Player Information")] 
    [SerializeField] 
    private Color32[] playerColors;
    [SerializeField]
    private Transform[] playerSpawnPoints;
    public List<GameObject> players;
    
    public String[] scenes;
    public Boolean[] minigamesWon;

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
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("OnPlayerJoined");
        if (SceneManager.GetActiveScene().name == "LibraryHub")
        {
            Debug.Log("Library loaded");
            //Assign playerInput to the player array, name it and prepare it's number in collections.
            players.Add(playerInput.gameObject);
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
        foreach (GameObject player in players)
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

    public void FinishedMinigame(string mingameScene)
    {
        int sceneIndex = Array.IndexOf(scenes, mingameScene);
        minigamesWon[sceneIndex] = true;
        Debug.Log("Selected this scenenumber: " + sceneIndex);
        StartCoroutine(SceneController.Instance.LoadLibraryHub());
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
