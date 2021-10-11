using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{


    public InputField playerNameInputField;

    void Start()
    {
        playerNameInputField.text = GetPlayerName();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        PhotonNetwork.NickName = GetPlayerName();
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "0.0.0";
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom() 
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("SampleScene");
        }   
    }

    public void SetPlayerName()
    {
        PlayerPrefs.SetString("PlayerName", playerNameInputField.text);
    }

    public string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName");
    }
}
