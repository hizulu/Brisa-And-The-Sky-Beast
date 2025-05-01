using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 30/04/2025
[RequireComponent(typeof(Renderer))]
public class MaterialColorAnimator : MonoBehaviour
{
    [SerializeField] private int materialIndex = 0;

    [SerializeField]
    private Color animatedColor = new Color(0.8f, 0f, 0.35f, 1f);

    private Material _material;

    void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        materials[materialIndex] = new Material(materials[materialIndex]); // Instanciar solo ese material
        renderer.materials = materials;
        _material = materials[materialIndex];

        animatedColor = new Color(0.8f, 0f, 0.35f, 1f);
    }

    void Update()
    {
        if (_material != null)
            _material.color = animatedColor;
    }
}
