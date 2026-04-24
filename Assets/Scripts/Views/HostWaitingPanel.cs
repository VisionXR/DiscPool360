using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostWaitingPanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public AudioDataSO audioData;
    public PlayerDataSO playerData;
    public NetworkOutputDataSO networkOutPutData;
    public UIDataSO uiData;
    public GameDataSO gameData;
    public UserDataSO userData;

    [Header("Player Objects")]
    public Image player1Image;
    public Image player2Image;

    public TMP_Text player1Name; 
    public TMP_Text player2Name;

    [Header("Local Objects")]
    public GameObject QuitLobbyPanel;
    public TMP_Text timeText;
    public TMP_Text roomNameText;

    public GameObject hostStartObject;
    public GameObject clientStartObject;
    public GameObject clientWaitingObject;
    public GameObject hostWaitingOBject;

    public TMP_Text clientNotReadyText;   
    public TMP_Text hostNotReadyText;

    // local variables
    private Coroutine timeRoutine;
    private float elapsedTime = 0f; // Variable to store the elapsed time
    public bool isHostJoined = false;
    public bool isClientJoined = false;


    private void OnEnable()
    {
        ResetTexts();
        ResetTime();
        // Start the coroutine when the GameObject becomes enabled
        // Ensure only one instance of the coroutine is running
        if (timeRoutine == null)
        {
            timeRoutine = StartCoroutine(ShowTime());
        }

        networkOutPutData.HostReadyEvent += ResetTime;
        networkOutPutData.ClientReadyEvent += ResetTime;

        roomNameText.text = "Room ID: "+networkOutPutData.runner.SessionInfo.Name;
    }

    private void OnDisable()
    {
        // Stop the coroutine when the GameObject becomes disabled
        // This prevents errors if the GameObject is destroyed or deactivated
        if (timeRoutine != null)
        {
            StopCoroutine(timeRoutine);
            timeRoutine = null;
        }

        networkOutPutData.HostReadyEvent -= ResetTime;
        networkOutPutData.ClientReadyEvent -= ResetTime;


        networkOutPutData.SetHostReady(false);
        networkOutPutData.SetClientReady(false);
        isHostJoined = false;
        isClientJoined = false;
    }

    private IEnumerator ShowTime()
    {
        while (true)
        {
            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // Format the time as "MM:SS" with leading zeros if necessary
            // "D2" ensures two digits, padding with a leading zero if the number is less than 10
            timeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);

          

            Player mp = playerData.GetPlayerById(1);
            if (mp != null && !isHostJoined)
            {
                
                player1Image.sprite = mp.playerProperties.myImage;
                player1Name.text = mp.playerProperties.myName;
                isHostJoined = true;
                ResetTime();
            }
            else if(mp != null)
            {
                player1Image.sprite = mp.playerProperties.myImage;
                player1Name.text = mp.playerProperties.myName;
            }
            else if(mp == null)
            {
 
                player1Name.text = "";
                player2Name.text = "";
                isHostJoined = false;
            }

            Player op = playerData.GetPlayerById(2);
            if (op != null && !isClientJoined)
            {

                player2Image.sprite = op.playerProperties.myImage;
                player2Name.text = op.playerProperties.myName;

                isClientJoined = true;
                ResetTime();
            }
            else if(op!= null)
            {
                player2Image.sprite = op.playerProperties.myImage;
                player2Name.text = op.playerProperties.myName;

            }
            else if(op == null) 
            {
                player2Image.sprite = null;
                player2Name.text = "";

                isClientJoined = false;
            }

            if (isHostJoined && isClientJoined)
            {
                if (networkOutPutData.IsHostReady() && networkOutPutData.IsClientReady())
                {
                    if (networkOutPutData.isHost)
                    {
                       
                        ShowHostStartButton();
                    }
                    else
                    {
                       
                        ShowClientStartButton();
                    }
                }
                else if (networkOutPutData.IsHostReady() && !networkOutPutData.IsClientReady())
                {                
                    clientNotReadyText.gameObject.SetActive(true);
                }
                else if (!networkOutPutData.IsHostReady() && networkOutPutData.IsClientReady())
                {               
                    hostNotReadyText.gameObject.SetActive(true);
                }
            }
            else if (isHostJoined && !isClientJoined)
            {
                
                if (!clientWaitingObject.activeInHierarchy)
                {
                    
                    clientWaitingObject.SetActive(true);
                }
              
            }
            else if (!isHostJoined && isClientJoined)
            {
                
                if (!hostWaitingOBject.activeInHierarchy)
                {
                    hostWaitingOBject.SetActive(true);
                }
            }

            // Wait for the next frame before continuing the loop
            yield return new WaitForEndOfFrame();
        }

       
    }

    // Optional: Method to reset the timer
    public void ResetTime()
    {
        
       
        hostStartObject.SetActive(false);
        clientStartObject.SetActive(false);
        hostWaitingOBject.SetActive(false);
        clientWaitingObject.gameObject.SetActive(false);
       

        hostNotReadyText.gameObject.SetActive(false);
        clientNotReadyText.gameObject.SetActive(false);
    }

    private void ResetTexts()
    {
        elapsedTime = 0f;
        timeText.text = "00:00"; // Reset display immediately        
        player1Image.sprite = null;
        player1Name.text = "";
        player2Image.sprite = null;
        player2Name.text = "";
    }


  
    public void LaunchInvitePanel()
    {
        string roomId = networkOutPutData.runner.SessionInfo.Name;
        // Start the process of generating and sharing the link
        StartCoroutine(GenerateAndShare(roomId));
    }

    IEnumerator GenerateAndShare(string roomID)
    {
        string apiURL = "https://api.tinyurl.com/create";
        string deepLink = "discpool://" + roomID;

        // TinyURL's simple payload
        string jsonPayload = $"{{\"url\": \"{deepLink}\", \"domain\": \"tinyurl.com\"}}";

        UnityWebRequest request = new UnityWebRequest(apiURL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        // Use your TinyURL Token here
        request.SetRequestHeader("Authorization", "Bearer " + userData.linkAPI);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Response format: {"data": {"tiny_url": "https://tinyurl.com/xyz"}}
            string shortLink = ExtractTinyUrl(request.downloadHandler.text);

            Debug.Log("Generated TinyURL: " + shortLink);

            new NativeShare()
                .SetSubject("DiscPool PvP Invite")
                .SetText($"Join my match in DiscPool! Tap to play: {shortLink}")
                .Share();
        }
        else
        {
            Debug.LogError("TinyURL Error: " + request.error);
        }
    }

    private string ExtractTinyUrl(string json)
    {
        // Simple helper to grab the "tiny_url" field
        int start = json.IndexOf("tiny_url\":\"") + 11;
        int end = json.IndexOf("\"", start);
        return json.Substring(start, end - start).Replace("\\/", "/");
    }


    public void ShowHostStartButton()
    {

        ResetTime();
        hostStartObject.SetActive(true);
    }

    public void ShowClientStartButton()
    {
        ResetTime();
        clientStartObject.SetActive(true);
    }

    public void StartGameBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        SendPlayerData sendPlayerData = playerData.GetMainPlayer().GetComponent<SendPlayerData>();
        if (gameData.firstTurnId == -1 || gameData.firstTurnId == 2)
        {
            sendPlayerData.RPC_StartGame(1, userData.myCoins);
        }
        else
        {
            sendPlayerData.RPC_StartGame(2, userData.myCoins);
        }
        gameObject.SetActive(false);
    }

    public void ExitBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        gameData.ExitGame();
        gameObject.SetActive(false);
    }

    public void BackBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        QuitLobbyPanel.SetActive(true);
    }
}

[System.Serializable]
public class DubResponse
{
    public string shortLink; // The generated link (e.g., dub.sh/abc123)
}
