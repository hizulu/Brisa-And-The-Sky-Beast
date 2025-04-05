#region Bibliotecas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/* NOMBRE CLASE: Character Appearance Manager
 * AUTOR: Lucía García López
 * FECHA: 25/03/2025
 * DESCRIPCIÓN: Script que se encarga de cambiar la apariencia del personaje.
 * VERSIÓN: 1.1 Cambio de baseMap de las cejas.
 */

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

                // Como las cejas son un atlas, no se puede cambiar el material, hay que cambiar el BaseMap.
                Texture2D newBaseMap = appearance.eyebrowsBaseMap;
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
