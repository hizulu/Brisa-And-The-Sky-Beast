using System.Collections;
using UnityEngine;

public class MinimapChangeIcon : MonoBehaviour
{
    [SerializeField] private GameObject brisaIcon;
    [SerializeField] private GameObject beastIcon;
    [SerializeField] private Sprite brisaIconSprite;
    [SerializeField] private Sprite togetherIconSprite;
    [SerializeField] private Beast beastScript;
    [SerializeField] private float transitionDuration = 0.5f;

    private BeastTrapped beastTrapped;

    private SpriteRenderer brisaRenderer;
    private SpriteRenderer beastRenderer;
    private Coroutine transitionCoroutine;
    private bool isTogether = false;

    private void Awake()
    {
        beastTrapped = FindAnyObjectByType<BeastTrapped>();
    }

    private void Start()
    {
        brisaRenderer = brisaIcon.GetComponent<SpriteRenderer>();
        beastRenderer = beastIcon.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (brisaIcon.activeSelf)
        {
            if (beastTrapped.beasIsFree)
            {
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

    private void SetAlpha(SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }
}
