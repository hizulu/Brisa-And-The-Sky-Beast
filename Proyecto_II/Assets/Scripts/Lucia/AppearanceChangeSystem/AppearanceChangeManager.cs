using UnityEngine;

/* NOMBRE CLASE: Character Appearance Manager
 * AUTOR: Lucía García López
 * FECHA: 25/03/2025
 * DESCRIPCIÓN: Script que se encarga de cambiar la apariencia del personaje.
 * VERSIÓN: 1.1 Cambio de baseMap de las cejas.
 * 1.2 Cambio con Dither Shader.
 */

public class CharacterAppearanceManager : MonoBehaviour
{
    [Header("Renderer Settings")]
    [SerializeField] private Renderer characterRenderer;

    // Índices de materiales
    private const int BODY_MATERIAL_INDEX = 0;
    private const int EYES_MATERIAL_INDEX = 2;
    private const int EYEBROWS_MATERIAL_INDEX = 3;

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
        if (appearance.bodyBaseMap != null && materials.Length > BODY_MATERIAL_INDEX)
        {
            SetMaterialTexture(materials[BODY_MATERIAL_INDEX], "_BaseTex", appearance.bodyBaseMap);
        }

        // Cambiar textura de ojos
        if (appearance.eyesBaseMap != null && materials.Length > EYES_MATERIAL_INDEX)
        {
            SetMaterialTexture(materials[EYES_MATERIAL_INDEX], "_BaseTex", appearance.eyesBaseMap);
        }

        // Cambiar textura de cejas
        if (appearance.eyebrowsBaseMap != null && materials.Length > EYEBROWS_MATERIAL_INDEX)
        {
            SetMaterialTexture(materials[EYEBROWS_MATERIAL_INDEX], "_BaseTex", appearance.eyebrowsBaseMap);
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

    public void RefreshAppearance()
    {
        if (currentAppearance != null)
        {
            ApplyAppearance(currentAppearance);
        }
    }

    // Método para debug
    public void LogMaterialInfo()
    {
        if (characterRenderer == null) return;

        Material[] materials = characterRenderer.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            Debug.Log($"Material {i}: {materials[i].name} | Shader: {materials[i].shader.name} | Has _BaseTex: {materials[i].HasProperty("_BaseTex")}");
        }
    }
}