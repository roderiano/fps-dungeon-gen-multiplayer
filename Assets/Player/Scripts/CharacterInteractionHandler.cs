using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class CharacterInteractionHandler : MonoBehaviour
{
    
    private PhotonView photonView;
    private Transform cameraTransform;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        cameraTransform = transform.Find("Body/Main Camera");
    }

    void Update()
    {
        if(photonView.IsMine)
            DetectInteractableObject();     
    }

    /// <summary>
    /// Detect interactable object
    /// </summary>
    void DetectInteractableObject() 
    {
        RaycastHit hit;

        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject != null)
            {
                InteractableObject interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();
                
                if(interactableObject && Input.GetKeyDown(KeyCode.E))
                    CallInteraction(interactableObject);
            }
        }
    }

    /// <summary>
    /// Call interaction function of InteractableObject
    /// </summary>
    /// <param name="interactableObject">Object to interact</param>
    void CallInteraction(InteractableObject interactableObject) 
    {
        PhotonView interactableObjectPhotonView = interactableObject.gameObject.GetComponent<PhotonView>();
        interactableObjectPhotonView.RPC("Interact", RpcTarget.AllBuffered, photonView.ViewID);
    }
}
