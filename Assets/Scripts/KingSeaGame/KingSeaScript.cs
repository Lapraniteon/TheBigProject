using UnityEngine;
using System;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;

public class KingSeaScript : MonoBehaviour
{
    public int kingSeaHealth;
    [SerializeField]private int kingSeaMaxHealth;
    [SerializeField]private int snowballDamage;
    [SerializeField]private int healthPortionsForSwitchingWeaponSide;
    [SerializeField] [Tooltip("(percentage")] private int leftArmHealthTrigger;
    [SerializeField] [Tooltip("(percentage")] private int rightArmHealthTrigger;
    [SerializeField]private GameObject leftArm;
    [SerializeField]private GameObject rightArm;
    
    [SerializeField]private int kingSeaMaxHealthIncrease;
    private int _numberOfPlayers;

    public Transform[] spawnPoints;

    public static event Action <float> KingSeaTakesDamage;
    public static event Action SwitchingShieldPosition;
    
    private int _threshold;

    public bool HasWon { get; private set; }

    private EventInstance kingSeaLaughingEvent;

    private float _healthMultiplier;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Scale HP
        _numberOfPlayers = GameManager.Instance.players.Count;

        _healthMultiplier = 1f + (_numberOfPlayers - 1) * .75f;
        kingSeaMaxHealth = (int)(kingSeaMaxHealth * _healthMultiplier);
        
        kingSeaHealth = kingSeaMaxHealth;
        _threshold = kingSeaHealth - (int)(healthPortionsForSwitchingWeaponSide * _healthMultiplier);

        kingSeaLaughingEvent = RuntimeManager.CreateInstance("event:/SFX/KingSea/Laughing");
        
        RuntimeManager.PlayOneShot("event:/SFX/KingSea/Anger 2");
        
        GameManager.Instance.PlayersToSpawnPoints(spawnPoints);
    }

    public void TakingDamage()
    {
        // Take damage only if its calculated that the shield would be in front of the current firing trajectory, otherwise dont
        
        kingSeaHealth -= snowballDamage;
        float amount = (float) kingSeaHealth / (float) kingSeaMaxHealth;
        RuntimeManager.PlayOneShot("event:/SFX/KingSea/Hurt", transform.position);
        KingSeaTakesDamage?.Invoke(amount);
        CheckHealth();
    }
    
    private void CheckHealth()
    {
        if (kingSeaHealth <= 0 && !HasWon)
        {
            Win();
        }
        
        if (kingSeaHealth <= _threshold)
        {
            _threshold -= (int)(healthPortionsForSwitchingWeaponSide * _healthMultiplier);
            SwitchSides();
        }

        if (kingSeaHealth == (int)(leftArmHealthTrigger * _healthMultiplier))
        {
            leftArm.SetActive(true);
        }

        if (kingSeaHealth == (int)(rightArmHealthTrigger * _healthMultiplier))
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
        HasWon = true;
        
        Debug.Log("You win!");
        
        RuntimeManager.PlayOneShot("event:/BGM/MUS_VictorySting");
        
        DOTween.Sequence()
            .AppendInterval(3f)
            .AppendCallback(() => GameManager.Instance.FinishedMinigame())
            .Play();
    }

    public void Laugh()
    {
        if (!GameManager.IsFmodEventPlaying(kingSeaLaughingEvent))
        {
            kingSeaLaughingEvent.start();
        }
    }
}
