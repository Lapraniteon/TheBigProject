using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class AngerBarScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public void UpdateAngerBar(float amount)
    {
        //slider.value = amount;
        slider.DOValue(amount, .1f);
    }
    private void OnEnable()
    {
        KingSeaScript.KingSeaTakesDamage += UpdateAngerBar;
    }
    
    private void OnDisable()
    {
        KingSeaScript.KingSeaTakesDamage -= UpdateAngerBar;
    }
}
