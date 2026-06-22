using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class AuthManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO playerSettings;
        public UIDataSO uiData;
        public CloudDataSO cloudData;
        public AchievementsDataSO achievementData;
        public DestinationSO destinationData;
        public LeaderBoardSO leaderBoardData;
        public PurchaseDataSO purchaseData;

        [Header("Local Objects")]
        public GameObject tutorialManager;
        public Destination multiPlayerDestination;
        public string tutorialState;
        public DestinationPanelView destinationPanelView;
        public ChangeDestinationView changeDestinationPanelView;
        public TMP_Text errorText;
        public Sprite GuestPlayerIcon;
        public bool isLoggedIn = false;
        public bool isLink = false;
        private bool isFirstTime = true;


        private void Awake()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;

            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }

        }

        private void OnDisable()
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        private IEnumerator Start()
        {
            isLoggedIn = false;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            yield return new WaitForSeconds(0.5f); // Small delay to ensure everything is initialized


            if(PlayerPrefs.HasKey("IsGoogleLogin"))
            {
                Login();
            }
            else
            {
                uiData.uiManager.ChangeState("Login", true);
            }

        }

        public void Login()
        {
            if (Application.isEditor)
            {
                EditorLogin();
            }
            else
            {
                GoogleLogin();
            }
        }



        private void OnDeepLinkActivated(string url)
        {

            isLink = true;
            string linkurl = ParseDeepLink(url);

            if(string.IsNullOrEmpty(linkurl))
            {
                return;
            }


            UrlLinkData newData = ConvertStringToLinkData(linkurl);
            uiData.SetGameType(GameType.MultiPlayer);

            string fullInput = newData.r;
            string actualRoomName = fullInput;
            ServerRegion targetRegion = ServerRegion.any;

            // Ensure the input has at least 2 characters before splitting
            if (!string.IsNullOrEmpty(fullInput) && fullInput.Length >= 2)
            {
                string regionCodeStr = fullInput.Substring(0, 2);
                actualRoomName = fullInput.Substring(2);

                // 2. Convert the 2-digit string to an integer
                if (int.TryParse(regionCodeStr, out int regionIndex))
                {
                    // 3. Explicitly cast the integer to your ServerRegion enum
                    // (Note: This assumes the parsed integer maps directly to your enum indexes)
                    targetRegion = (ServerRegion)regionIndex;
                }
                else
                {
                    Debug.LogWarning($"Could not parse '{regionCodeStr}' into an integer. Defaulting to 'any'.");
                }
            }
            else
            {
                Debug.LogWarning("Room code entered is too short! Using fallback handling.");
            }

            multiPlayerDestination.roomName = actualRoomName;
            multiPlayerDestination.region = targetRegion;
            multiPlayerDestination.gameMode = (GameMode)(int.Parse(newData.g));
            multiPlayerDestination.time = newData.t;

            if (isFirstTime)
            {
                isFirstTime = false;

            }
            else
            {

                changeDestinationPanelView.SetDestination(multiPlayerDestination);
                uiData.uiManager.GoToState(StateName.ChangeDestinationState);
                isLink = false;
            }
        }


        public string ParseDeepLink(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            try
            {
                string prefix = "discpool://";
                if (!url.StartsWith(prefix)) return null;

                string jsonPart = url.Substring(prefix.Length);


                return jsonPart;
            }
            catch (Exception e)
            {
                Debug.LogError($"Deep Link Parse Error: {e.Message}");
                return null;
            }
        }

        private void EditorLogin()
        {
            // Simplified Editor Mock
            playerSettings.SetUserNameAndId("Guest_Player", UnityEngine.Random.Range(0, 9999).ToString());
            playerSettings.SetProfileUrl("");
            playerSettings.SetUserProfileImage(GuestPlayerIcon);
            StartCoroutine(ConnectToPlayfab(5));
            uiData.SetLoginType(LoginType.Google);
            PlayerPrefs.SetString("IsGoogleLogin", "true");
            PlayerPrefs.Save();
            ProcessGameFlow();

        }

        public void GuestLogin()
        {
            playerSettings.SetUserNameAndId("Guest_"+UnityEngine.Random.Range(0, 9999).ToString(), SystemInfo.deviceUniqueIdentifier);
            playerSettings.SetProfileUrl(""); // Set to empty or a default guest icon URL
            playerSettings.SetUserProfileImage(GuestPlayerIcon);
            uiData.uiManager.ChangeState("Login", false);
            uiData.SetLoginType(LoginType.Guest);
            ProcessGameFlow();
        }


        public void GoogleLogin()
        {

            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

        }

        internal void ProcessAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
               
                // 1. First, set local UI data (Name and Image)
                string name = Social.localUser.userName;
                string googleID = Social.localUser.id;
                string imageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();


                playerSettings.SetUserNameAndId(name, googleID);
                playerSettings.SetProfileUrl(imageUrl);    
        
                StartCoroutine(ConnectToPlayfab(5));               
                StartCoroutine(FetchAllData());
                StartCoroutine(LoadProfileImage());

                uiData.SetLoginType(LoginType.Google);
                uiData.uiManager.ChangeState("Login", false);
                PlayerPrefs.SetString("IsGoogleLogin", "true");
                PlayerPrefs.Save();
                ProcessGameFlow();
           
            }

            else
            {
                Debug.LogError($"Google Play Games Authentication Failed: {status}");
                uiData.uiManager.ChangeState("Login", true);
                StartCoroutine(ShowError());
                // Handle failure (e.g., show an error message to the user)
            }

        }

        private IEnumerator ShowError()
        {
           
            errorText.text = "Google Play requires a profile profile setup.\n" +
                              "If the window didn't show up, please clear your device's Google Play Games app cache or continue securely as a Guest!";           
            yield return new WaitForSeconds(3);
            errorText.text = "";
        }

        private void ProcessGameFlow()
        {
            if (!isLink)
            {

                isFirstTime = false;


                if (!PlayerPrefs.HasKey("Tutorial"))
                {
                    tutorialManager.SetActive(true);
                    uiData.uiManager.ChangeState("Tutorial", true);
                    uiData.uiManager.GoToState(StateName.Tutorial);
                    PlayerPrefs.SetString("Tutorial", "true");
                }
                else
                {

                    uiData.uiManager.GoToState(StateName.HomeState);
                }

            }
            else
            {

                destinationPanelView.SetDestination(multiPlayerDestination);
                uiData.uiManager.ChangeState("Link", true);
                isLink = false;
            }
        }

        private IEnumerator FetchAllData()
        {
            
            purchaseData.GetAllItems();      
            yield return new WaitForSeconds(1);
            purchaseData.GetPurchasedItems();
            yield return new WaitForSeconds(1);
            achievementData.GetAllAchievemnets();
            yield return new WaitForSeconds(1);
            leaderBoardData.GetMyPoints();
            
        }

        private void RequestTokenAndLoginToPlayFab()
        {


            PlayGamesPlatform.Instance.RequestServerSideAccess(true, (authCode) =>
            {
                
                if (string.IsNullOrEmpty(authCode)) return;
                // Use LoginWithGooglePlayGamesServices instead of LoginWithGoogleAccount
                var request = new LoginWithGooglePlayGamesServicesRequest
                {
                    ServerAuthCode = authCode,
                    CreateAccount = true,
                    TitleId = PlayFabSettings.TitleId
                };

                PlayFabClientAPI.LoginWithGooglePlayGamesServices(request, OnPlayFabSuccess, OnPlayFabFailure);
            });
        }

        private void OnPlayFabSuccess(LoginResult result)
        {

            isLoggedIn = true;
            cloudData.PlayFabLoginSuccess();

            //// OPTIONAL: Update PlayFab display name to match Google name
            //UpdatePlayFabDisplayName(Social.localUser.userName);
        }

        private void OnPlayFabFailure(PlayFabError error)
        {
            

            cloudData.PlayFabLoginFailure();
        }

        private IEnumerator LoadProfileImage()
        {
            float timeout = 5f;
            while (Social.localUser.image == null && timeout > 0)
            {
                timeout -= Time.deltaTime;
                yield return null;
            }

            if (Social.localUser.image != null)
            {
                playerSettings.SetUserProfileImage(ConvertTextureToSprite(Social.localUser.image));

            }
        }

        public Sprite ConvertTextureToSprite(Texture2D texture)
        {
            if (texture == null) return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        public UrlLinkData ConvertStringToLinkData(string queryString)
        {
            // Create a new instance of your class to populate
            UrlLinkData data = new UrlLinkData();

            // 1. Split the string by '&' to get each individual parameter pair
            string[] pairs = queryString.Split('&');

            foreach (string pair in pairs)
            {
                // 2. Split each pair by '=' to separate the key from the value
                string[] keyValue = pair.Split('=');

                // Ensure we actually have a valid key and value pair to avoid errors
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    // 3. Match the key and assign the value to the correct class property
                    switch (key)
                    {
                        case "r":
                            data.r = value;
                            break;
                        case "g":
                            data.g = value;
                            break;
                        case "t":
                            data.t = value;
                            break;
                    }
                }
            }

            return data;
        }

        private IEnumerator ConnectToPlayfab(float timeoutDuration)
        {

            isLoggedIn = false;
            // Loop until the data is loaded OR we hit the timeout limit
            while (!isLoggedIn)
            {
                if(Application.isEditor)
                {
                    // If in Editor, use a fixed string so you always log into the same test account
                    // If on Mobile, use the unique Device ID
                    string customId = Application.isEditor ? "Editor_Test_User" : SystemInfo.deviceUniqueIdentifier;

                    var request = new LoginWithCustomIDRequest
                    {
                        CustomId = customId,
                        CreateAccount = true,
                        TitleId = PlayFabSettings.TitleId
                    };

                    PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabSuccess, OnPlayFabFailure);
                }
                else
                {
                    RequestTokenAndLoginToPlayFab();
                }

                yield return new WaitForSeconds(timeoutDuration); // Wait for the next frame
            }


            isLoggedIn = true;
        }

    }
  
}