using UnityEngine;
using System;
using DG.Tweening;

public class KingSeaScript : MonoBehaviour
{
    public int kingSeaHealth;
    [SerializeField]private int kingSeaMaxHealth;
    [SerializeField]private int snowballDamage;
    [SerializeField]private int healthPortionsForSwitchingWeaponSide;
    [SerializeField]private int leftArmHealthTrigger;
    [SerializeField]private int rightArmHealthTrigger;
    [SerializeField]private GameObject leftArm;
    [SerializeField]private GameObject rightArm;
    
    [SerializeField]private int kingSeaMaxHealthIncrease;
    private int _numberOfPlayers;
    [SerializeField]private GameManager gameManager;
    
    [SerializeField]
    private Transform[] spawnPoints;

    public static event Action <float> KingSeaTakesDamage;
    public static event Action SwitchingShieldPosition;
    
    private int _threshold;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _numberOfPlayers = gameManager.players.Count;
        if (_numberOfPlayers < 1)
        {
            for (int i = 1; i < _numberOfPlayers; i++)
            {
                kingSeaMaxHealth += kingSeaMaxHealthIncrease;
            }
        }
        
        kingSeaHealth = kingSeaMaxHealth;
        _threshold = kingSeaHealth - healthPortionsForSwitchingWeaponSide;
        
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
    }

    public void TakingDamage()
    {
        // Take damage only if its calculated that the shield would be in front of the current firing trajectory, otherwise dont
        
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
        
        if (kingSeaHealth <= _threshold)
        {
            _threshold -= healthPortionsForSwitchingWeaponSide;
            SwitchSides();
        }

        if (kingSeaHealth == leftArmHealthTrigger)
        {
            leftArm.SetActive(true);
        }

        if (kingSeaHealth == rightArmHealthTrigger)
        {
            rightArm.SetActive(true);
        }
    }

    private void SwitchSides()
    {
        Debug.Log("Shield is moving");
        SwitchingShieldPosition?.Invoke();
    }
    
    private void Win()
    {
        Debug.Log("You win!");
        //TODO what happens when the anger meter goes down
    }
}
