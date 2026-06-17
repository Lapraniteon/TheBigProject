using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinishTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Minigame finish detected");
            GameManager.Instance.FinishedMinigame(SceneManager.GetActiveScene().name);
        }
    }
}
