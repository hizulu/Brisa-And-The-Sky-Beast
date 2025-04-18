using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 17/04/2025
// Clase que gestiona el estado de montar a la bestia, en la que la bestia pasa a estar bajo el control del jugador
public class BeastMountedState : BeastState
{
    private Vector3 lastPosition;
    private bool wasWalking = false;
    private float stillTimer = 0f;
    private const float movementThreshold = 0.01f;
    private const float stopDelay = 0.2f; // Tiempo mínimo sin moverse antes de detener animación

    public override void OnEnter(Beast beast)
    {
        Debug.Log("Entering BeastMountedState, Brisa taking over Beast control...");
        beast.agent.enabled = false;

        // Subir al jugador para que al colocar la bestia no atraviese el suelo
        float alturaBestia = 4.5f;
        beast.playerTransform.position += new Vector3(0, alturaBestia, 0);

        //beast.rb.constraints = RigidbodyConstraints.FreezeAll;


        beast.transform.SetParent(beast.mountPoint);
        beast.transform.localPosition = Vector3.zero;
        beast.transform.localRotation = Quaternion.identity;

        lastPosition = beast.transform.position;
        beast.anim.SetBool("isWalking", false);
    }
    public override void OnUpdate(Beast beast)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) //TODO: sustituirlo por NEW INPUT SYSTEM
        {
            beast.TransitionToState(new BeastFreeState());
        }

        float distanceMoved = Vector3.Distance(beast.transform.position, lastPosition);
        bool isMoving = distanceMoved > movementThreshold;

        if (isMoving)
        {
            stillTimer = 0f;

            if (!wasWalking)
            {
                beast.anim.SetBool("isWalking", true);
                wasWalking = true;
            }
        }
        else
        {
            stillTimer += Time.deltaTime;

            if (wasWalking && stillTimer >= stopDelay)
            {
                beast.anim.SetBool("isWalking", false);
                wasWalking = false;
            }
        }

        lastPosition = beast.transform.position;
    }
    public override void OnExit(Beast beast)
    {
        Debug.Log("Leaving BeastMountedState, AI taking over Beast control");
        beast.transform.SetParent(null);
        beast.playerTransform.position -= new Vector3(0, 2f, 0);
        beast.agent.enabled = true;
        //beast.rb.constraints = RigidbodyConstraints.None;

        beast.anim.SetBool("isWalking", false);
    }
}
