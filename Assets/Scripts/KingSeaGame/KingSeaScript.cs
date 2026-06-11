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

    public static event Action <float> KingSeaTakesDamage;
    public static event Action SwitchingShieldPosition;
    
    private int threshold;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kingSeaHealth = kingSeaMaxHealth;
        threshold = kingSeaHealth - healthPortionsForSwitchingWeaponSide;
    }

    private void TakingDamage()
    {
        kingSeaHealth -= snowballDamage;
        float amount = (float) kingSeaHealth / (float) kingSeaMaxHealth;
        KingSeaTakesDamage?.Invoke(amount);
        CheckHealth();
    }

    private void Attacking()
    {
        //TODO The platforms split when King Sea attacks
    }
    
    private void CheckHealth()
    {
        if (kingSeaHealth <= 0)
        {
            Win();
        }
        
        if (kingSeaHealth <= threshold)
        {
            threshold -= healthPortionsForSwitchingWeaponSide;
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

    // private void OnTriggerEnter(Collider collision)
    // {
    //     Debug.Log("trigger hit");
    //     TakingDamage();
    //     Destroy(collision.gameObject);
    // }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Snowball"))
        {
            TakingDamage();
            // Destroy(collision.gameObject);
        }
    }
}
