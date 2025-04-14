using UnityEngine;

[ExecuteInEditMode]
public class ToonPostProcessEffect : MonoBehaviour
{
    public Material toonMaterial;

    // Se llama despu�s de que la c�mara haya renderizado la escena
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (toonMaterial != null)
        {
            // Aplica el material con el Toon Shader
            Graphics.Blit(source, destination, toonMaterial);
        }
        else
        {
            // Si no se asigna material, simplemente pasa la imagen sin cambios
            Graphics.Blit(source, destination);
        }
    }
}
