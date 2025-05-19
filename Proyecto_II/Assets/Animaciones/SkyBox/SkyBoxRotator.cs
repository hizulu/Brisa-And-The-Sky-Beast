using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 18/05/2025
public class SkyBoxRotator : MonoBehaviour
{
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private float rotationSpeed = 1.0f;

    void Update()
    {
        float rotation = Time.time * rotationSpeed;
        skyboxMaterial.SetFloat("_Rotation", rotation);
    }
}
