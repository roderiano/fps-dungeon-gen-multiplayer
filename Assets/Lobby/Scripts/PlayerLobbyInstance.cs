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

    [PunRPC]    
    public void SetReady()
    {
        ready = !ready;
        photonView.RPC("SetStatusLabel", RpcTarget.AllBuffered, ready);
    }

    [PunRPC]
    void SetParent()
    {
        transform.SetParent(GameObject.Find("PlayerList").transform);
    }

    [PunRPC]
    void SetNickName(string nickName)
    {
        transform.Find("Nickname").GetComponent<Text>().text = nickName;
    }

    [PunRPC]
    void SetStatusLabel(bool _ready)
    {
        
        Text statusText = transform.Find("Status").GetComponent<Text>();
        statusText.text = _ready ? "READY" : "NOT READY";
        statusText.color = _ready ? Color.green : Color.red;
    }

}
