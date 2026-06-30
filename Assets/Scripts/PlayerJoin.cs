using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerJoin : MonoBehaviour
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
    
    public static event Action PlayerJoined;
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("OnPlayerJoined");
        if (SceneManager.GetActiveScene().name == "LibraryHub")
        {
            if (GameManager.Instance.firstPlayerJoin == false)
            {
                PlayerJoined?.Invoke();
                GameManager.Instance.firstPlayerJoin = true;
            }
            //Assign playerInput to the player array, name it and prepare it's number in collections.
            PlayerController controller = playerInput.gameObject.GetComponent<PlayerController>();
            GameManager.Instance.players.Add(controller);
            playerInput.gameObject.name = "Player " + GameManager.Instance.players.Count; //Rename to the player number
            
            int playerNumber = GameManager.Instance.players.Count - 1; //number to get the right variable from the arrays.
            
            //Add the playermodel with the right color as a child.
            GameObject playerModel = Instantiate(playerModels[playerNumber], playerInput.gameObject.transform.position + playerModelPosition, playerInput.gameObject.transform.rotation, playerInput.gameObject.transform);
            playerModel.transform.localRotation = Quaternion.Euler(childModelRotation);
            playerModel.gameObject.name = "playerModel";
            controller.playerAnimator = playerModel.GetComponent<Animator>();
            
            //Add the right colour car as a child and disable it.
            GameObject car = Instantiate(cars[GameManager.Instance.players.IndexOf(playerInput.gameObject.GetComponent<PlayerController>())], playerInput.gameObject.transform.position + carPosition, playerInput.transform.rotation, playerInput.gameObject.transform);
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
}
