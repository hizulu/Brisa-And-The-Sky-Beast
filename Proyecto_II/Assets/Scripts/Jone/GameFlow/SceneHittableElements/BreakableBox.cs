using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// Jone Sainz Egea 27/03/2025
public class BreakableBox : HittableElement
{
    [SerializeField] private Animator anim;

    private BreakableEffectHandler effectHandler;

    [Range(0f, 1f)] [SerializeField] private float scaleFactor = 0.65f;

    private void Start()
    {
        effectHandler = new BreakableEffectHandler(VFXPoolManager.Instance.GetVFX());
    }

    public override void OnHit()
    {
        Debug.Log("La caja se ha roto.");
        if (anim != null)
        {
            anim.SetTrigger("breakBox");
        }
        this.gameObject.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
        effectHandler.PlayEffect(transform.position);
        StartCoroutine(ReturnVFXToPool(1f));

        Invoke("RemoveBox", 0.5f);
    }

    private void RemoveBox()
    {
        gameObject.SetActive(false);
    }
    private IEnumerator ReturnVFXToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
        VisualEffect visualEffect = effectHandler.GetVisualEffect();
        VFXPoolManager.Instance.ReturnVFX(visualEffect);
    }
}
