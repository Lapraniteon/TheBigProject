using System;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    
    public GameObject[] lights;

    public GameObject[] messyLibrary;
    public GameObject[] cleanLibrary;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
        Boolean[] minigamesCompleted = GameManager.Instance.minigamesWon;
        UpdateLibraryProgress(minigamesCompleted);
    }

    private void UpdateLibraryProgress(Boolean[] minigamesCompleted)
    {
        bool gameCompleted = true;
        for (int i = 0; i < minigamesCompleted.Length; i++)
        {
            
            if (minigamesCompleted[i])
            {
                lights[i].gameObject.SetActive(true);
                messyLibrary[i].gameObject.SetActive(false);
                cleanLibrary[i].gameObject.SetActive(true);
            }

            if (!minigamesCompleted[i])
            {
                gameCompleted = false;
            }
        }

        if (gameCompleted)
        {
            lights[4].gameObject.SetActive(true);
        }
    }

    
}
