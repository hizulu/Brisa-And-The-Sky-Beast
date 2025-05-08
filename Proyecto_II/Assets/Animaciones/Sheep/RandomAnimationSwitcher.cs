using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationSwitcher : MonoBehaviour
{
 
    public Animator animator;
    public float minDelay = 2f;
    public float maxDelay = 5f;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        StartCoroutine(SwitchAnimationRoutine());
    }

    private System.Collections.IEnumerator SwitchAnimationRoutine()
    {
        while (true)
        {
            int randomChoice = Random.Range(0, 2); // 0 or 1
            animator.SetInteger("RandomSelector", randomChoice);
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
