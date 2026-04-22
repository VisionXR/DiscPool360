using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;
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
        
        audioData.PlayAudio(AudioClipType.ButtonClick);

        //var options = new InviteOptions();
        //GroupPresence.LaunchInvitePanel(options).OnComplete(message =>
        //{

        //    if (message.IsError)
        //    {
        //        Debug.Log(message.GetError().Message);
        //    }
        //    else
        //    {
        //        Debug.Log(" Invite Panel success...");
        //    }
        //});
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
        sendPlayerData.RPC_StartGame(userData.myCoins);
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
