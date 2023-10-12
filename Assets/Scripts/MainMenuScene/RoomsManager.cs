using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI joinCodeText;
    string joinCode;
    [SerializeField] TMP_InputField inputField;

    [SerializeField] TextMeshProUGUI gameMessageText;
    [SerializeField] GameObject playButton;

    private UnityTransport transport;
    private const int MaxPlayers = 6;

    private async void Awake()
    {
        transport = FindObjectOfType<UnityTransport>();

        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //playButton.SetActive(false);
    }

    public async void CreateGame()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);
        joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        GameCodeHolder.gameCode = joinCode;

        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        //NetworkManager.Singleton.StartHost();
        GameCodeHolder.playerType = PlayerType.HOST;

        gameMessageText.text = "Game created. You can enter.";
        //playButton.SetActive(true);
        LoadScene.StartGame("SampleScene");
    }

    public async void JoinGame()
    {
        try
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                gameMessageText.text = "Please input the game code.";
                return;
            }

            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(inputField.text);
            GameCodeHolder.gameCode = inputField.text;

            transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

            //NetworkManager.Singleton.StartClient();
            GameCodeHolder.playerType = PlayerType.CLIENT;

            gameMessageText.text = "Game joined. You can enter.";
            //playButton.SetActive(true);
            LoadScene.StartGame("SampleScene");
        }
        catch (RelayServiceException e)
        {
            {
                Debug.Log(e.Message);

                gameMessageText.text = e.Message;
            }
        }
    }


}
