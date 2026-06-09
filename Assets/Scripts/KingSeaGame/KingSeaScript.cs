using UnityEngine;
using System;

public class KingSeaScript : MonoBehaviour
{
    public int kingSeaHealth;
    [SerializeField] private int kingSeaMaxHealth;
    [SerializeField] private int snowballDamage;
    [SerializeField] private int healthPortions;
    public static event Action <float> KingSeaTakesDamage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kingSeaHealth = kingSeaMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TakingDamage()
    {
        kingSeaHealth -= snowballDamage;
        float amount = (float) kingSeaHealth / (float) kingSeaMaxHealth;
        KingSeaTakesDamage?.Invoke(amount);
        CheckHealth();
    }
    
    private void CheckHealth()
    {
        if (kingSeaHealth <= 0)
        {
            Win();
        }
        int threshold = kingSeaHealth - healthPortions;
        if (kingSeaHealth <= threshold)
        {
            threshold -= healthPortions;
            SwitchSides();
        }
    }

    private void SwitchSides()
    {
        Debug.Log("King Sea is the switching the shield");
        //TODO The boss will change the position of sword and shield
    }
    
    private void Win()
    {
        Debug.Log("You win!");
        //TODO what happens when the anger meter goes down
    }

    // private void OnTriggerEnter(Collider collision)
    // {
    //     Debug.Log("trigger hit");
    //     TakingDamage();
    //     Destroy(collision.gameObject);
    // }
    
    private void OnCollisionEnter(Collision collision)
    {
        TakingDamage();
        Destroy(collision.gameObject);
    }
}
