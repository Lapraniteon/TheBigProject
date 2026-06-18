using UnityEngine;

public class MiniGameManagerTest : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("minigame started");
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
