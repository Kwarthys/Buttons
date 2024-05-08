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
            //raycast
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.down, 10, interactionLayers);
            if(hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                Interactor interactor = hit.transform.gameObject.GetComponentInParent<Interactor>();
                if(interactor != null)
                {
                    currentInteractor = interactor;
                    interactor.onInteractionStart(mousePos);
                }
            }
            else
            {
                Debug.Log("Missed");
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
