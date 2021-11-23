using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerLobbyInstance : MonoBehaviour
{

    public PhotonView photonView;

    public bool ready;

    void Start()
    {
        ready = false;
        photonView = GetComponent<PhotonView>();

        if(photonView.IsMine)
        {
            photonView.RPC("SetParent", RpcTarget.AllBuffered);
            photonView.RPC("SetNickName", RpcTarget.AllBuffered, photonView.Owner.NickName);
            photonView.RPC("SetStatusLabel", RpcTarget.AllBuffered, ready);
        }
    }

    /// <summary>
    /// Remote procedure call to change ready flag
    /// </summary>
    [PunRPC]    
    public void SetReady()
    {
        ready = !ready;
        photonView.RPC("SetStatusLabel", RpcTarget.AllBuffered, ready);
    }

    /// <summary>
    /// Remote procedure call to set parent of this UI GameObject
    /// </summary>    
    [PunRPC]
    void SetParent()
    {
        transform.SetParent(GameObject.Find("PlayerList").transform);
    }

    /// <summary>
    /// Remote procedure call to set NickName of this UI GameObject
    /// </summary>
    /// <param name="nickName">Own NickName</param>
    [PunRPC]
    void SetNickName(string nickName)
    {
        transform.Find("Nickname").GetComponent<Text>().text = nickName;
    }

    
    /// <summary>
    /// Remote procedure call to set own label status style by _ready flag
    /// </summary>
    /// <param name="nickName">Own NickName</param>
    [PunRPC]
    void SetStatusLabel(bool _ready)
    {
        
        Text statusText = transform.Find("Status").GetComponent<Text>();
        statusText.text = _ready ? "READY" : "NOT READY";
        statusText.color = _ready ? Color.green : Color.red;
    }

}
