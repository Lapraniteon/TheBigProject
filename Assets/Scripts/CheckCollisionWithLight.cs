using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class CheckCollisionWithLight : MonoBehaviour
{

    [Header("Detection")]
    
    public bool detectionEnabled = true;
    
    private Light _light;

    [Space]
    [SerializeField] private Transform cone;
    [SerializeField] private Collider coneCollider;
    [SerializeField] private float colliderRange;
    
    [Space]
    [NaughtyAttributes.Layer]
    [SerializeField] private int layerMask;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!detectionEnabled)
            return;

        UpdateCollider(); // Update collider scale
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        Debug.Log("Player Enter Light");
    }

    private void UpdateCollider()
    {
        float baseWidth = 2f * colliderRange * Mathf.Tan(_light.innerSpotAngle / 2f * Mathf.Deg2Rad);
        cone.localScale = new Vector3(baseWidth, baseWidth, colliderRange);
    }
}
