using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Photon.Pun;

[System.Serializable] public class ObjectInteractionEvent : UnityEvent<PhotonView> {}

public class InteractableObject : MonoBehaviour
{
    public ObjectInteractionEvent interactionCallback;

    /// <summary>
    /// Remote procedure call to interact with the InteractableObject
    /// </summary>
    /// <param name="photonViewID">ViewID of source interaction player</param>
    [PunRPC]
    public void Interact(int photonViewID)
    {
        interactionCallback.Invoke(GetPhotonViewByViewID(photonViewID));
    }

    /// <summary>
    /// Get PhotonView by ViewID
    /// </summary>
    /// <param name="photonViewID">Target PhotonView ID</param>
    /// <returns>PhotonView of view id</returns>
    private PhotonView GetPhotonViewByViewID(int photonViewID)
    {
        PhotonView[] pvs = Object.FindObjectsOfType<PhotonView>();
        
        foreach (PhotonView pv in pvs)
            if(pv.ViewID == photonViewID)
                return pv;

        return null;
    }

}
