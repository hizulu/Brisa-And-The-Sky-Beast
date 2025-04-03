using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAppearanceManager : MonoBehaviour
{
    public Renderer characterRenderer;
    private AppearanceChangeData currentAppearance;
    private AppearanceChangeData appearanceData;

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

            if (materials.Length >= 4)
            {
                // Crear una nueva instancia del material principal
                materials[0] = new Material(appearance.appearanceMainMaterial);

                // Cambiar el Base Map del material en la posición 3
                Texture2D newBaseMap = appearance.eyebrowsBaseMap; // Suponiendo que tienes una textura en appearance
                if (newBaseMap != null)
                {
                    materials[3].SetTexture("_BaseMap", newBaseMap);
                }

                // Aplicar los materiales modificados
                characterRenderer.materials = materials;
            }
        }
    }
}
