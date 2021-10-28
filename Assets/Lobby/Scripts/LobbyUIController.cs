using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [Header("Steam Configuration")]
    public GameObject steamConfiguration;
    public InputField tokenInputField, steamIDInputField;

    [Header("Lobby")]
    public GameObject lobby;
    public Image avatarImage;
    public Text nicknameText;

    /// <summary>
    /// Active SteamConfiguration UI
    /// </summary>
    public void ActiveSteamConfiguration() 
    {
        tokenInputField.text = PlayerPrefs.GetString("token");
        steamIDInputField.text = PlayerPrefs.GetString("steamID");

        steamConfiguration.SetActive(true);
        lobby.SetActive(false);
    }

    /// <summary>
    /// Active Lobby UI
    /// </summary>
    public void ActiveLobby() 
    {
        SteamWrapper steamWrapper = Object.FindObjectOfType<SteamWrapper>();
        nicknameText.text = steamWrapper.owner.personName;
        avatarImage.sprite = steamWrapper.owner.avatar;
        
        steamConfiguration.SetActive(false);
        lobby.SetActive(true);
    }

    /// <summary>
    /// Get token from TokenInputField
    /// </summary>
    public string GetToken() 
    {
        return tokenInputField.text;
    }

    /// <summary>
    /// Get token from SteamIDInputField
    /// </summary>
    public string GetSteamID() 
    {
        return steamIDInputField.text;
    }
}
