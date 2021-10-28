﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.Instantiate(this.playerPrefab.name, transform.position, Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadDungeon() 
    {
        PhotonNetwork.LoadLevel("Dungeon");   
    }
}