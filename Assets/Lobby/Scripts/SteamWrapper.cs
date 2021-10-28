using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Leguar.TotalJSON;

[RequireComponent(typeof(LobbyUIController))]
public class SteamWrapper : MonoBehaviour
{
    public SteamUser owner = null;
    
    private string token;

    public void Authenticate(string token, string steamID)
    {
        this.token = token;
        StartCoroutine(GetUser(steamID, SetOwner));
    }

    // TODO: Add callback to add UserToFrendList ou to request friendlist
    IEnumerator GetUser(string steamID, System.Action<SteamUser> callback)
    {
        string url = string.Format("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}", token, steamID);
        UnityWebRequest userRequest = UnityWebRequest.Get(url);
 
        yield return userRequest.SendWebRequest();

        if (userRequest.result == UnityWebRequest.Result.ConnectionError || userRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Failed to get user. Try again later..."); 
        }
        else
        { 
            JSON response = JSON.ParseString(userRequest.downloadHandler.text);
            JSON user = response.GetJSON("response").GetJArray("players").GetJSON(0);
            
            // Get person name and avatar URL
            string personName = user.GetString("personaname");
            string avatarURL = user.GetString("avatarfull"); 
            
            // Download avatar texture
            UnityWebRequest avatarRequest = UnityWebRequestTexture.GetTexture(avatarURL);
            yield return avatarRequest.SendWebRequest();
            Sprite avatar = DownloadHandlerTexture.GetContent(avatarRequest).ToSprite();

            SteamUser steamUser = new SteamUser(steamID, personName, avatar);

            callback(steamUser);
        }
    }

    private void SetOwner(SteamUser steamUser)
    {
        // Set owner
        owner = steamUser;
        
        // Save settings
        PlayerPrefs.SetString("token", token);
        PlayerPrefs.SetString("steamID", steamUser.steamID);
    }

    public void GenerateToken() 
    {
        Application.OpenURL("https://steamcommunity.com/dev/apikey");
    }
}

public class SteamUser
{
    public string personName, steamID; 
    public Sprite avatar;
    public SteamUser(string steamID, string personName, Sprite avatar) 
    {
        this.steamID = steamID;
        this.personName = personName;
        this.avatar = avatar;
    }
}
