using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private List<Player> playersReadyToDungeon = new List<Player>();

    void Start()
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Instantiate(this.playerPrefab.name, transform.position, Quaternion.identity, 0);
    }

    /// <summary>
    /// Load dungeon level
    /// </summary>
    public void LoadDungeon() 
    {
        PhotonNetwork.LoadLevel("Dungeon");   
    }

    /// <summary>
    /// Switch ready flag to load dungeon by eventData.selectedObject
    /// </summary>
    /// <param name="eventData"></param>
    public void SwitchReadyToDungeon(BaseEventData eventData) 
    {
        PhotonView photonView = eventData.selectedObject.GetComponent<PhotonView>();

        if(playersReadyToDungeon.Contains(photonView.Owner))
            playersReadyToDungeon.Remove(photonView.Owner);
        else
            playersReadyToDungeon.Add(photonView.Owner);
    }
}
