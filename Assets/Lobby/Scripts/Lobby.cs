using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(LobbyUIController))]
public class Lobby : MonoBehaviourPunCallbacks
{
    private LobbyUIController lobbyUIController;

    void Start()
    {
        lobbyUIController = GetComponent<LobbyUIController>();
       
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = lobbyUIController.GetNickname();
    }

    public void JoinOffline() 
    {
        PhotonNetwork.OfflineMode = true;
    }
    
    public void JoinOnline() 
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(lobbyUIController.GetRoomName() != "" ? lobbyUIController.GetRoomName() : "0", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        lobbyUIController.SwitchMenuSection(MenuSection.LobbyMenu);
        GameObject playerLobbyInstanceGO = PhotonNetwork.Instantiate("PlayerLobbyInstance", transform.position, transform.rotation);
    }

    public void SwitchReady()
    {

        PhotonView[] pvs = Object.FindObjectsOfType<PhotonView>();
        int playerReadyCount = 0;

        foreach(PhotonView pv in pvs)
        {
            PlayerLobbyInstance instance = pv.gameObject.GetComponent<PlayerLobbyInstance>();
            if(pv.IsMine)
                pv.RPC("SetReady", RpcTarget.AllBuffered);

            if(instance.ready) 
                playerReadyCount++;

            lobbyUIController.UpdateReadyButton(instance.ready);
        }
            
        if(PhotonNetwork.CurrentRoom.PlayerCount == playerReadyCount)
            PhotonNetwork.LoadLevel("Sanctuary");

    }

}