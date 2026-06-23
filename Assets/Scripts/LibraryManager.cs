using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
    }
}
