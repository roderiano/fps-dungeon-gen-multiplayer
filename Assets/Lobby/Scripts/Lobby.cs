using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(LobbyUIController))]
public class Lobby : MonoBehaviourPunCallbacks
{
    private LobbyUIController lobbyUIController;

    
    #region MonoBehaviour
        void Start()
        {
            lobbyUIController = GetComponent<LobbyUIController>();
        
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = lobbyUIController.GetNickname();
        }

        /// <summary>
        /// Simulate connect to play offline
        /// </summary>
        public void JoinOffline() 
        {
            PhotonNetwork.OfflineMode = true;
        }
        
        /// <summary>
        /// Connect to master 
        /// </summary>
        public void JoinOnline() 
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// Switch ready flag on own PlayerLobbyInstance and check to load sanctuary scene
        /// </summary>    
        public void SwitchReadyFlagAndCheckToLoadSanctuary()
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
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel("Sanctuary");
            }
        }
    
    #endregion
    
    

    #region PunCallbacks
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
    #endregion

}