using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerRespawning : MonoBehaviour
{
    public void EliminatePlayer(PlayerController player)
    {
        Debug.Log("Player Enter Light");
        
        StartCoroutine(EliminatePlayerCoroutine(player));
    }

    private IEnumerator EliminatePlayerCoroutine(PlayerController player)
    {
        player.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(3f);
        
        player.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        CheckCollisionWithLight.OnDetectPlayer += EliminatePlayer;
    }

    private void OnDisable()
    {
        CheckCollisionWithLight.OnDetectPlayer -= EliminatePlayer;
    }
}
