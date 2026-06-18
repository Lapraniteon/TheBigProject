using UnityEngine;

public class LibraryScript : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
    }
}
