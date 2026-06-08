using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirectionalPad : MonoBehaviour
{
    
    public static Dictionary<string, int> directions = new()
    {
        {"North", 0},
        {"East", 0},
        {"South", 0},
        {"West", 0}
    };

    public Vector2 ChosenDirection()
    {
        int maxValue = directions.Max(kvp => kvp.Value);

        var maxDirections = directions
            .Where(kvp => kvp.Value == maxValue)
            .ToList();

        if (maxDirections.Count > 1) // A tie.
            return Vector2.zero;

        switch (maxDirections.First().Key)
        {
            case "North":
                return Vector2.up;
            case "East":
                return Vector2.right;
            case "South":
                return Vector2.down;
            case "West":
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }

    public void ChangeDirectionNumber(string key, int count)
    {
        directions[key] += count;
        Debug.Log($"N: {directions["North"]}, E: {directions["East"]}, S: {directions["South"]}, W: {directions["West"]}");
        Debug.Log(ChosenDirection());
    }
}
