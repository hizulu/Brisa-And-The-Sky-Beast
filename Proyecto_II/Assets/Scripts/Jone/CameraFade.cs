using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* NOMBRE CLASE: CameraFade
 * AUTOR: Jone Sainz Egea
 * FECHA: 27/03/2025
 * DESCRIPCIÓN: Script base que se encarga del fade in/fade out de la cámara
 * VERSIÓN: 1.0 funcionamiento del fade in/fade out básico
 */
public class CameraFade : MonoBehaviour
{
    [SerializeField] float speedScale = 1f;
    [SerializeField] Color fadeColor = Color.black;

    // Rather than Lerp or Slerp, we allow adaptability with a configurable curve
    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));

    private float alpha = 0f;
    private Texture2D texture;
    private int direction = 0;
    private float time = 0f;

    private void Start()
    {
        alpha = 0f;
        CreateTexture();
    }

    private void CreateTexture()
    {
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }

    public void DoFadeInOut()
    {
        if (direction == 0)
        {
            if (alpha >= 1f) // Fully faded out
            {
                alpha = 1f;
                time = 0f;
                direction = 1;
            }
            else // Fully faded in
            {
                alpha = 0f;
                time = 1f;
                direction = -1;
            }

        }
    }
    public void OnGUI()
    {
        if (alpha > 0f) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        if (direction != 0)
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = Curve.Evaluate(time);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            if (alpha <= 0f || alpha >= 1f) direction = 0;
        }
    }
}
