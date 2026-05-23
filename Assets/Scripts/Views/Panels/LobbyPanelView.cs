using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class LobbyPanelView : MonoBehaviour
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

            roomNameText.text = "Room ID: " + networkOutPutData.runner.SessionInfo.Name;
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

            ResetTexts();
            ResetTime();
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
                else if (mp != null)
                {
                    player1Image.sprite = mp.playerProperties.myImage;
                    player1Name.text = mp.playerProperties.myName;
                }
                else if (mp == null)
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
                else if (op != null)
                {
                    player2Image.sprite = op.playerProperties.myImage;
                    player2Name.text = op.playerProperties.myName;

                }
                else if (op == null)
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
                        hostStartObject.SetActive(false);
                        clientStartObject.SetActive(false);
                        clientWaitingObject.SetActive(true);
                    }

                }
                else if (!isHostJoined && isClientJoined)
                {

                    if (!hostWaitingOBject.activeInHierarchy)
                    {
                        hostStartObject.SetActive(false);
                        clientStartObject.SetActive(false);
                        hostWaitingOBject.SetActive(true);
                    }
                }
                else if (!isHostJoined && !isClientJoined)
                {

                    if (!hostWaitingOBject.activeInHierarchy)
                    {
                        hostStartObject.SetActive(false);
                        clientStartObject.SetActive(false);
                        hostWaitingOBject.SetActive(true);
                        clientWaitingObject.SetActive(false);
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

            Debug.Log("Room id is " + roomId);
            string wixBaseUrl = "https://visionxr.co.in/join";

            // Map CoinsType enum onto your lowercase Wix assets configuration dictionary keys
            string gameMode = "default";
            switch (userData.myCoins)
            {
                case CoinsType.EightPool:
                    gameMode = "8pool";
                    break;
                case CoinsType.FivePool:
                    gameMode = "5pool";
                    break;
                case CoinsType.TenSnooker:
                    gameMode = "10snooker";
                    break;
                case CoinsType.SixSnooker:
                    gameMode = "6snooker";
                    break;
                case CoinsType.ColorChallenge:
                    gameMode = "colorchallenge";
                    break;
            }

            // Escape Room ID to keep URL formatting valid
            string escapedRoomId = UnityWebRequest.EscapeURL(roomId);

            // Construct link payload string
            string shareUrl = $"{wixBaseUrl}?room={roomId}&game={gameMode}&playerName={userData.MyName}";

            Debug.Log($"[Invite System] Outbound link generated for mode '{userData.myCoins}': {shareUrl}");

            // Call Native platform window share card handler sheet instantly
            new NativeShare()
                .SetSubject("DiscPool 360 Challenge")
                .SetText($"Can you beat me?  Click to join my room in Disc Pool 360: {shareUrl}")
                .Share();
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

            Debug.Log(" Last id is " + gameData.GetFirstTurnId());

            if (gameData.GetFirstTurnId() == -1 || gameData.GetFirstTurnId() == 2)
            {
                sendPlayerData.RPC_StartGame(1, (int)userData.myCoins);
            }
            else
            {
                sendPlayerData.RPC_StartGame(2, (int)userData.myCoins);
            }
            
        }

        public void ExitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();

            gameData.ExitGame();
        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState("Pause", true);
        }
    }
}

[System.Serializable]
public class DubResponse
{
    public string shortLink; // The generated link (e.g., dub.sh/abc123)
}
