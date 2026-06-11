using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class DirectionalPadButton : MonoBehaviour
{

    private DirectionalPad _pad;
    
    [SerializeField] [Label("Direction")] private string key;

    private void Start()
    {
        _pad = transform.parent.GetComponent<DirectionalPad>();
        
        if (_pad == null) 
            Debug.LogError("Directional Pad Button needs a Directional Pad parent.");
    }

    public void Enter()
    {
        _pad.ChangeDirectionNumber(key, 1);
    }

    public void Exit()
    {
        _pad.ChangeDirectionNumber(key, -1);
    }
}
