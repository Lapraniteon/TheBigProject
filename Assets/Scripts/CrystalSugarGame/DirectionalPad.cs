using System.Collections.Generic;
using UnityEngine;

public class DirectionalPad : MonoBehaviour
{
    
    public Dictionary<string, int> Directions = new()
    {
        {"North", 0},
        {"East", 0},
        {"South", 0},
        {"West", 0}
    };
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDirectionNumber(string key, int count)
    {
        Directions[key] += count;
        Debug.Log($"N: {Directions["North"]}, E: {Directions["East"]}, S: {Directions["South"]}, W: {Directions["West"]}");
    }
}
