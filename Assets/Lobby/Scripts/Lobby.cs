using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(SteamWrapper))]
[RequireComponent(typeof(LobbyUIController))]
public class Lobby : MonoBehaviourPunCallbacks
{
    
    private SteamWrapper steamWrapper;
    private LobbyUIController lobbyUIController;

    void Start()
    {
        steamWrapper = GetComponent<SteamWrapper>();
        lobbyUIController = GetComponent<LobbyUIController>();

        StartCoroutine(WaitAuthenticationToStartLobby());
    }

    /// <summary>
    /// Wait for SteamWrapper authentication to active lobby UI
    /// </summary>
    IEnumerator WaitAuthenticationToStartLobby()
    {
        lobbyUIController.ActiveSteamConfiguration();

        while(steamWrapper.owner == null)
            yield return new WaitForSeconds(0.1f);
        
        lobbyUIController.ActiveLobby();
    }

    /// <summary>
    /// Call SteamWrapper to authenticate
    /// </summary>
    public void Authenticate() 
    {
        steamWrapper.Authenticate(lobbyUIController.GetToken(), lobbyUIController.GetSteamID());
    }

    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = steamWrapper.owner.personName;
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
            PhotonNetwork.LoadLevel("Sanctuary");
        }   
    }
}
