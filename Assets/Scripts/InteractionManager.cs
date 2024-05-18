using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask interactionLayers;

    private Interactor currentInteractor = null;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(Physics.Raycast(mousePos, Vector3.forward, out RaycastHit hit , 50f, interactionLayers))
            {
                Interactor interactor = hit.transform.gameObject.GetComponentInParent<Interactor>();
                if(interactor != null)
                {
                    currentInteractor = interactor;
                    interactor.onInteractionStart(mousePos);
                }
            }
        }
        else if(currentInteractor != null)
        {
            if(Input.GetMouseButtonUp(0))
            {
                currentInteractor.onInteractionEnd();
                currentInteractor = null;
            }
            else
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentInteractor.onInteractionUpdate(mousePos);
            }
        }
    }
}
