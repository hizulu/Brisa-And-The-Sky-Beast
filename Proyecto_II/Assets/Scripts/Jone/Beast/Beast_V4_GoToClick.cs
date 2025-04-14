using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Beast_V4_GoToClick : MonoBehaviour
{
    [SerializeField] LayerMask clickableLayers;
    [SerializeField] ParticleSystem clickEffect;
    
    PlayerInputActions input;

    private float lookRotationSpeed = 8f;


    private NavMeshAgent agent;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        input = new PlayerInputActions();
        AssignInputs();
    }

    private void Update()
    {
        if (agent.velocity == Vector3.zero)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    private void MoveToClick()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, clickableLayers))
        {
            agent.destination = hit.point;
            if(clickEffect != null)
            {
                Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
            }
        }
    }

    private void AssignInputs()
    {
        //input.Player.MoveBeast.performed += ctx => MoveToClick();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
