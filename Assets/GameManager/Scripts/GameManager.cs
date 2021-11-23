using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Instantiate(this.playerPrefab.name, transform.position, Quaternion.identity, 0);
    }

    void LoadDungeon() 
    {
        PhotonNetwork.LoadLevel("Dungeon");   
    }
}
