using System.Collections;
using UnityEngine;
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
            StartCoroutine(Respawn(other.gameObject.GetComponent<PlayerController>()));
        }
    }

    private IEnumerator Respawn(PlayerController playerController)
    {
        Debug.Log("Respawn Detected");
        playerController.RespawnActive(true);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        playerController.RespawnActive(false);
    }
}
