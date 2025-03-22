using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterAppearanceManager : MonoBehaviour
{
    public Renderer characterRenderer;
    public AppearanceChangeData appearanceData;

    void Start()
    {
        ApplyAppearance(appearanceData);
    }

    public void ChangeAppearance(AppearanceChangeData newAppearance)
    {
        if (newAppearance != null)
        {
            ApplyAppearance(newAppearance);
            appearanceData = newAppearance;
        }
    }

    private void ApplyAppearance(AppearanceChangeData appearance)
    {
        if (characterRenderer != null && appearance != null)
        {
            Material[] materials = characterRenderer.materials;

            if (materials.Length > 1)
            {
                materials[0] = appearance.appearanceMaterial;
                characterRenderer.materials = materials;
            }
        }
    }
}
