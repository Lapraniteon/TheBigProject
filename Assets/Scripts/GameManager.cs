using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Player Information")] 
    [SerializeField] 
    private Color32[] playerColors;
    [SerializeField]
    private Transform[] playerSpawnPoints;
    public List<GameObject> players;
    
    public String[] scenes;


    public void OnPlayerJoined(PlayerInput playerInput)
    {
       //Assign playerInput to the player array, name it and prepare it's number in collections.
       players.Add(playerInput.gameObject);
       playerInput.gameObject.name = "Player " + players.Count; //Rename to the player number
       int playerNumber = players.Count - 1; //number to get the right variable from the arrays.
       
       //Set the colour of the player.
       playerInput.gameObject.GetComponent<Renderer>().material.color = playerColors[playerNumber];
       
       //Set the position of the joined player to the corresponding spawnpoint.
       playerInput.transform.position = playerSpawnPoints[playerNumber].transform.position; 
       Physics.SyncTransforms(); //Makes sure the player teleports because the CharacterController often stops this.
       Debug.Log("Spawned Player " + playerNumber + " at " + playerSpawnPoints[playerNumber].transform.position);
    }

    private void InteractionDetected(GameObject interactableObject)
    {
        if (interactableObject.CompareTag("Book"))
        {
            SceneController.Instance.LoadScene(scenes[0]);
            Debug.Log(scenes[0] + " loading");
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
