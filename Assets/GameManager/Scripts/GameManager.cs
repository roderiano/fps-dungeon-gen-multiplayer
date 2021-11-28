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
    /// Switch ready flag to load dungeon by source player PhotonView and check if can load scene
    /// </summary>
    /// <param name="sourcePlayerPhotonView">Player PhotonView who switched the flag</param>
    public void SwitchPlayerReadyFlagToDungeon(PhotonView sourcePlayerPhotonView) 
    {
        if(playersReadyToDungeon.Contains(sourcePlayerPhotonView.Owner))
            playersReadyToDungeon.Remove(sourcePlayerPhotonView.Owner);
        else
            playersReadyToDungeon.Add(sourcePlayerPhotonView.Owner);

        
        // Load dungeon
        if(PhotonNetwork.CurrentRoom.PlayerCount == playersReadyToDungeon.Count)
            PhotonNetwork.LoadLevel("Dungeon");
    }
}
