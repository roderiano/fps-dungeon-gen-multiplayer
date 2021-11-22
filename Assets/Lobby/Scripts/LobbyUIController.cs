using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public enum MenuSection 
{
    MainMenu = 0,
    LobbyMenu = 1
}

public class LobbyUIController : MonoBehaviour
{
    [Header("Main Menu")]
    public InputField nicknameInputField;
    public InputField roomNameInputField;

    public Transform mainMenuPanel;

    [Header("Lobby Menu")]
    public Transform playerListTransform;
    public Transform lobbyMenuPanel;
    public Color readyColor, notReadyColor;

    void Awake()
    {
        nicknameInputField.text = PlayerPrefs.GetString("Nickname");
        SwitchMenuSection(MenuSection.MainMenu);
    }

    void Update() 
    {
        AdjustPlayerListView();
    }
    

    /// <summary>
    /// Save nickname on PlayerPrefs
    /// </summary>
    public void SaveNickName() 
    {
        PlayerPrefs.SetString("Nickname", nicknameInputField.text);
    }

    /// <summary>
    /// Get nickname from nicknameInputField
    /// </summary>
    public string GetNickname() 
    {
        return nicknameInputField.text;
    }

    /// <summary>
    /// Get room name from roomNameInputField
    /// </summary>
    public string GetRoomName() 
    {
        return roomNameInputField.text;
    }


    /// <summary>
    /// Switch menu section
    /// </summary>
    /// <param name="section">Menu section</param>
    public void SwitchMenuSection(MenuSection section) 
    {
        switch (section)
        {
            case MenuSection.LobbyMenu:
                lobbyMenuPanel.gameObject.SetActive(true);
                mainMenuPanel.gameObject.SetActive(false);
                break;
            case MenuSection.MainMenu:
                lobbyMenuPanel.gameObject.SetActive(false);
                mainMenuPanel.gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Adjust grid frorm player list view
    /// </summary>
    private void AdjustPlayerListView() 
    {
        Rect playerListRect = playerListTransform.GetComponent<RectTransform>().rect;
        GridLayoutGroup grid = playerListTransform.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(playerListRect.width, (playerListRect.height - grid.spacing.y) / 4);
    }
    

    public void UpdateReadyButton(bool ready)
    {
        lobbyMenuPanel.Find("ReadyButton").GetComponent<Image>().color =  ready ? notReadyColor : readyColor;
        lobbyMenuPanel.Find("ReadyButton/Text").GetComponent<Text>().text = ready ? "NOT READY" : "READY";
    }
}
