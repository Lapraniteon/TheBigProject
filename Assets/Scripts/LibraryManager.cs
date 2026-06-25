using System;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    
    public GameObject[] lights;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
        Boolean[] minigamesCompleted = GameManager.Instance.minigamesWon;
        ActivateProgressLights(minigamesCompleted);
    }

    private void ActivateProgressLights(Boolean[] minigamesCompleted)
    {
        bool gameCompleted = true;
        for (int i = 0; i < minigamesCompleted.Length; i++)
        {
            
            if (minigamesCompleted[i])
            {
                lights[i].gameObject.SetActive(true);
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
