using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// Jone Sainz Egea 27/03/2025
public class BreakableBox : HittableElement
{
    [SerializeField] private Animator anim;

    public override void OnHit()
    {
        Debug.Log("La caja se ha roto.");
        if (anim != null)
        {
            anim.SetTrigger("breakBox");

            VisualEffect smokePoof = VFXPoolManager.Instance.GetVFX();
            smokePoof.transform.position = transform.position;
            smokePoof.Play();

            float effectDuration = 1f; 
            StartCoroutine(ReturnVFXToPool(smokePoof, effectDuration));

            // Desactivar la caja en lugar de destruirla (opcional para reciclaje)
            gameObject.SetActive(false);
        }
    }
    private IEnumerator ReturnVFXToPool(VisualEffect vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        VFXPoolManager.Instance.ReturnVFX(vfx);
    }
}
