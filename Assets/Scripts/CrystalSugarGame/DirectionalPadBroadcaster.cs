using System;
using UnityEngine;

public class DirectionalPadBroadcaster : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DirectionalPadDirection")) 
            return;

        other.GetComponent<DirectionalPadButton>()?.Enter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("DirectionalPadDirection")) 
            return;
        
        other.GetComponent<DirectionalPadButton>()?.Exit();
    }
}
