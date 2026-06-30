using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Information")] 
    [SerializeField] 
    private GameObject[] playerModels;
    [SerializeField] 
    private Vector3 playerModelPosition;
    
    [SerializeField]
    private GameObject[] cars;
    [SerializeField]
    private Vector3 carPosition;
    
    [SerializeField]
    private Vector3 childModelRotation;
    
    
    public List<PlayerController> players = new ();
    
    public String[] scenes;
    public Boolean[] minigamesWon;
    public String[] actionMaps;
    
    
    public static GameManager Instance;
    

    [SerializeField]
    private PlayerInputManager playerInputManager;

    public static event Action PlayerJoin;
    public bool firstPlayerJoin = false;
    
    
    
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
            if (firstPlayerJoin == false)
            {
                PlayerJoin?.Invoke();
                firstPlayerJoin = true;
            }
            //Assign playerInput to the player array, name it and prepare it's number in collections.
            PlayerController controller = playerInput.gameObject.GetComponent<PlayerController>();
            players.Add(controller);
            playerInput.gameObject.name = "Player " + players.Count; //Rename to the player number
            
            int playerNumber = players.Count - 1; //number to get the right variable from the arrays.
            
            //Add the playermodel with the right color as a child.
            GameObject playerModel = Instantiate(playerModels[playerNumber], playerInput.gameObject.transform.position + playerModelPosition, playerInput.gameObject.transform.rotation, playerInput.gameObject.transform);
            playerModel.transform.localRotation = Quaternion.Euler(childModelRotation);
            playerModel.gameObject.name = "playerModel";
            controller.playerAnimator = playerModel.GetComponent<Animator>();
            
            //Add the right colour car as a child and disable it.
            GameObject car = Instantiate(cars[players.IndexOf(playerInput.gameObject.GetComponent<PlayerController>())], playerInput.gameObject.transform.position + carPosition, playerInput.transform.rotation, playerInput.gameObject.transform);
            car.transform.localRotation = Quaternion.Euler(childModelRotation);
            car.gameObject.name = "carModel";
            car.SetActive(false);
       
            //Set the position of the joined player to the corresponding spawnpoint.
            GameObject libraryManager = GameObject.Find("LibraryManager");
            playerInput.transform.position = libraryManager.GetComponent<LibraryManager>().spawnPoints[playerNumber].transform.position; 
            SceneManager.MoveGameObjectToScene(playerInput.gameObject, SceneManager.GetSceneByName("ManagerScene"));
            Physics.SyncTransforms(); //Makes sure the player teleports because the CharacterController often stops this.
            //Debug.Log("Spawned Player " + playerNumber + " at " + playerSpawnPoints[playerNumber].transform.position);
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

    public void FinishedMinigame()
    {
        //wait a little or implement a victorious animation?
        string minigameScene = SceneManager.GetActiveScene().name;
        int sceneIndex = Array.IndexOf(scenes, minigameScene);

        if (sceneIndex >= minigamesWon.Length)
            return;
        
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

    private void OnEnable()
    {
        PlayerController.Interaction += InteractionDetected;
    }

    private void OnDisable()
    {
        PlayerController.Interaction -= InteractionDetected;
    }
    
    public static bool IsFmodEventPlaying(EventInstance instance)
    {
        PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state == PLAYBACK_STATE.PLAYING;
    }
}


