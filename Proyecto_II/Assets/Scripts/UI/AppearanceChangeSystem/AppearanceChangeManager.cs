using UnityEngine;

/* NOMBRE CLASE: Appearance Change Manager
 * AUTOR: Lucía García López
 * FECHA: 25/03/2025
 * DESCRIPCIÓN: Script que se encarga de cambiar la apariencia del personaje.
 * VERSIÓN: 1.1 Cambio de baseMap de las cejas.
 * 1.2 Cambio con Dither Shader.
 */

public class AppearanceChangeManager : MonoBehaviour
{
    [Header("Renderer Settings")]
    [SerializeField] private Renderer characterRenderer;

    // Índices de materiales
    private const int bodyMaterialIndex = 0;
    private const int eyesMaterialIndex = 2;
    private const int eyebrowsMaterialIndex = 3;

    private AppearanceChangeData currentAppearance;

    public void ChangeAppearance(AppearanceChangeData newAppearance)
    {
        if (newAppearance != null && characterRenderer != null)
        {
            ApplyAppearance(newAppearance);
            currentAppearance = newAppearance;
        }
    }

    private void ApplyAppearance(AppearanceChangeData appearance)
    {
        // Obtener copia de los materiales actuales
        Material[] materials = characterRenderer.materials;

        // Cambiar textura del cuerpo
        if (appearance.bodyBaseMap != null && materials.Length > bodyMaterialIndex)
        {
            SetMaterialTexture(materials[bodyMaterialIndex], "_BaseTex", appearance.bodyBaseMap);
        }

        // Cambiar textura de ojos
        if (appearance.eyesBaseMap != null && materials.Length > eyesMaterialIndex)
        {
            SetMaterialTexture(materials[eyesMaterialIndex], "_BaseTex", appearance.eyesBaseMap);
        }

        // Cambiar textura de cejas
        if (appearance.eyebrowsBaseMap != null && materials.Length > eyebrowsMaterialIndex)
        {
            SetMaterialTexture(materials[eyebrowsMaterialIndex], "_BaseTex", appearance.eyebrowsBaseMap);
        }

        // Aplicar los cambios
        characterRenderer.materials = materials;
    }

    private void SetMaterialTexture(Material material, string propertyName, Texture texture)
    {
        if (material != null && material.HasProperty(propertyName))
        {
            material.SetTexture(propertyName, texture);
        }
        else
        {
            Debug.LogWarning($"Material {material.name} no tiene la propiedad {propertyName} o es nulo");
        }
    }
}