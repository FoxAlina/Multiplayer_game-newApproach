using TMPro;
using UnityEngine;
using Photon.Pun;

public class RoomsManager : MonoBehaviourPunCallbacks
{
    string joinCode;
    [SerializeField] TMP_InputField joinInputField;
    [SerializeField] TMP_InputField createInputField;

    [SerializeField] TextMeshProUGUI gameMessageText;

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInputField.text))
        {
            gameMessageText.text = "Please input the game code.";
            return;
        }

        PhotonNetwork.CreateRoom(createInputField.text);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinInputField.text))
        {
            gameMessageText.text = "Please input the game code.";
            return;
        }

        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
