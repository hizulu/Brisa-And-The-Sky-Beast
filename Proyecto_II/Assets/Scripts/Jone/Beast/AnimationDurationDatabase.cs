using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 02/05/2025
// Singleton que guarda la duración de animaciones por nombre a partir de un AnimatorController
public class AnimationDurationDatabase
{
    private static AnimationDurationDatabase _instance;
    public static AnimationDurationDatabase Instance => _instance ??= new AnimationDurationDatabase();

    // Diccionario que guarda el nombre del clip de la animación y su duración
    private readonly Dictionary<string, float> _clipDurations = new Dictionary<string, float>();

    // Para evitar cargar varias veces los mismos clips
    private readonly HashSet<RuntimeAnimatorController> _loadedControllers = new HashSet<RuntimeAnimatorController>();

    // Carga las duraciones de un AnimatorController si no se ha hecho ya
    public void RegisterAnimatorClips(RuntimeAnimatorController controller)
    {
        if (controller == null || _loadedControllers.Contains(controller))
            return;

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (!_clipDurations.ContainsKey(clip.name))
            {
                _clipDurations[clip.name] = clip.length;
            }
        }

        _loadedControllers.Add(controller);
    }

    // Obtiene la duración de una animación por nombre.
    public float GetClipDuration(string clipName)
    {
        if (_clipDurations.TryGetValue(clipName, out float duration))
        {
            return duration;
        }

        Debug.LogWarning($"Duración no encontrada para animación: {clipName}");
        return 0f;
    }
}
