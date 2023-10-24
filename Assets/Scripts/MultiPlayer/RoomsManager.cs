using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomsManager : MonoBehaviourPunCallbacks
{
    string joinCode;
    [SerializeField] TMP_InputField joinInputField;
    [SerializeField] TMP_InputField createInputField;

    [SerializeField] TextMeshProUGUI gameMessageText;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInputField.text))
        {
            gameMessageText.text = "Please input the game code.";
            return;
        }

        GameCodeHolder.gameCode = createInputField.text;
        PhotonNetwork.CreateRoom(createInputField.text, new Photon.Realtime.RoomOptions { BroadcastPropsChangeToAll = true });
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinInputField.text))
        {
            gameMessageText.text = "Please input the game code.";
            return;
        }

        GameCodeHolder.gameCode = joinInputField.text;
        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public override void OnJoinedRoom()
    {
        SetPlayerIcon();

        PhotonNetwork.LoadLevel("GameScene");
    }
    void SetPlayerIcon()
    {
        int iconIndex = 0;
        int players = PhotonNetwork.CurrentRoom.PlayerCount;
        if (players != 0)
            iconIndex = players - 1;

        //playerProperties.Add("playerAvatar", iconIndex);
        playerProperties["playerAvatar"] = iconIndex;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("playerAvatar"))
        {
            int index = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];
            Debug.Log(index);
        }
    }
}
