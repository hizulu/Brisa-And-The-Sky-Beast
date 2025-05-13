using System.Collections;
using UnityEngine;

/*
 * NOMBRE CLASE: MinimapChangeIcon
 * AUTOR: Lucia García López
 * FECHA: 19/04/2025
 * DESCRIPCIÓN: Script que gestiona el cambio de icono en el minimapa entre Brisa y la Bestia. 
 *              Si estan juntos, el icono de Brisa cambia a un icono diferente y el de la Bestia se desactiva
 * VERSIÓN: 1.0 
 * 1.1 Se añade la opción de cambiar el icono de Brisa a un icono diferente cuando está con la Bestia.
 * 1.2 Se activa la barra de vida de la Bestia cuando está libre.
 */

public class MinimapChangeIcon : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject brisaIcon;
    [SerializeField] private GameObject beastIcon;
    [SerializeField] private Sprite brisaIconSprite;
    [SerializeField] private Sprite togetherIconSprite;
    [SerializeField] private Beast beastScript;
    [SerializeField] private float transitionDuration = 0.5f;

    [SerializeField] private GameObject beastHealthBar;

    private BeastTrapped beastTrapped;

    private SpriteRenderer brisaRenderer;
    private SpriteRenderer beastRenderer;
    private Coroutine transitionCoroutine;
    private bool isTogether = false;
    #endregion

    private void Awake()
    {
        beastTrapped = FindAnyObjectByType<BeastTrapped>();
    }

    private void Start()
    {
        brisaRenderer = brisaIcon.GetComponent<SpriteRenderer>();
        beastRenderer = beastIcon.GetComponent<SpriteRenderer>();
        if(!beastTrapped.beasIsFree)
            beastHealthBar.SetActive(false);
    }

    private void Update()
    {
        if (brisaIcon.activeSelf)
        {
            if (beastTrapped.beasIsFree)
            {
                beastHealthBar.SetActive(true);
                bool shouldBeTogether = beastScript.IsPlayerWithinInteractionDistance();

                if (shouldBeTogether != isTogether)
                {
                    isTogether = shouldBeTogether;
                    if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
                    transitionCoroutine = StartCoroutine(TransitionIcons(isTogether));
                }
            }
        }
    }

    //Método para cambiar el icono de Brisa al icono de la Bestia y viceversa.
    private IEnumerator TransitionIcons(bool together)
    {
        float elapsed = 0f;
        float startAlphaBrisa = brisaRenderer.color.a;
        float startAlphaBeast = beastRenderer.color.a;
        float endAlpha = 0f;
        Vector3 startScale = brisaIcon.transform.localScale;
        Vector3 targetScale = together ? startScale * 2f : Vector3.one;

        // Fade out
        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            SetAlpha(brisaRenderer, Mathf.Lerp(startAlphaBrisa, endAlpha, t));
            SetAlpha(beastRenderer, Mathf.Lerp(startAlphaBeast, together ? 0f : 1f, t));
            brisaIcon.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Midpoint
        //Si estan juntos se cambia el icono a togetherIconSprite, si no se cambia a brisaIconSprite
        brisaRenderer.sprite = together ? togetherIconSprite : brisaIconSprite;
        beastIcon.SetActive(!together);

        // Fade in
        elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            SetAlpha(brisaRenderer, Mathf.Lerp(0f, 1f, t));
            if (!together)
                SetAlpha(beastRenderer, Mathf.Lerp(0f, 1f, t));
            brisaIcon.transform.localScale = Vector3.Lerp(targetScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(brisaRenderer, 1f);
        if (!together)
        {
            beastIcon.SetActive(true);
            SetAlpha(beastRenderer, 1f);
        }
    }

    //Método para cambiar el alpha del icono
    private void SetAlpha(SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }
}
