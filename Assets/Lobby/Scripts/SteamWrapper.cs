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
    public List<SteamUser> friendList = new List<SteamUser>();
    
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

        // Refresh friend list
        StartCoroutine(RefreshFriendList(steamUser.steamID));
    }

    IEnumerator RefreshFriendList(string steamID)
    {
        friendList.Clear();

        string url = string.Format("http://api.steampowered.com/ISteamUser/GetFriendList/v0001/?key={0}&steamid={1}&relationship=friend", token, steamID);
        UnityWebRequest friendListRequest = UnityWebRequest.Get(url);
 
        yield return friendListRequest.SendWebRequest();

        if (friendListRequest.result == UnityWebRequest.Result.ConnectionError || friendListRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Failed to get friend list. Try again later..."); 
        }
        else
        { 
            JSON response = JSON.ParseString(friendListRequest.downloadHandler.text);
            JArray friends = response.GetJSON("friendslist").GetJArray("friends");

            for(int i = 0; i < friends.Length; i++)
            {
                JSON friend = friends.GetJSON(i);
                string id = friend.GetString("steamid");
                StartCoroutine(GetUser(id, AddFriend));
            }   
        }
    }

    private void AddFriend(SteamUser steamUser)
    {
        friendList.Add(steamUser);
        Object.FindObjectOfType<LobbyUIController>().AddFriend(steamUser);
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
