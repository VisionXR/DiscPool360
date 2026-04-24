using com.VisionXR.ModelClasses;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
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

        [Header("Local Objects")]
        public Destination multiPlayerDestination;
        public bool isLoggedIn = false;
        public bool isLink = false;
        public DestinationPanel destinationPanel;

        private void Awake()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;

            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }
            else
            {

            }

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

            string roomId = ParseDeepLink(url);

            multiPlayerDestination.roomName = roomId;

            StartCoroutine(WaitAndConnect());

        }

        private IEnumerator WaitAndConnect()
        {
            while (!isLoggedIn)
            {
                yield return new WaitForSeconds(1);

            }

            uiData.ResetAllPanels();
            destinationPanel.gameObject.SetActive(true);
            destinationPanel.ConnectToDestination(multiPlayerDestination);
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
            playerSettings.SetUserNameAndId("Guest_Player",UnityEngine.Random.Range(0, 9999).ToString());
           

            // If in Editor, use a fixed string so you always log into the same test account
            // If on Mobile, use the unique Device ID
            string customId = Application.isEditor ? "Editor_Test_User" : SystemInfo.deviceUniqueIdentifier;

            var request = new LoginWithCustomIDRequest
            {
                CustomId = customId,
                CreateAccount = true,
                TitleId = PlayFabSettings.TitleId
            };

            isLoggedIn = true;

            PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabSuccess, OnPlayFabFailure);
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
                Debug.Log("Disc Pool: Google Login Successful!");

                // 1. First, set local UI data (Name and Image)
                string name = Social.localUser.userName;
                string googleID = Social.localUser.id;
               

                playerSettings.SetUserNameAndId(name, googleID);
                StartCoroutine(LoadProfileImage());
                isLoggedIn = true;

              
                achievementData.GetAllAchievemnets();
                RequestTokenAndLoginToPlayFab();

                if(!isLink)
                {
                    uiData.TriggerHomeEvent();
                }
            }

        }

        private void RequestTokenAndLoginToPlayFab()
        {
           

            PlayGamesPlatform.Instance.RequestServerSideAccess(true, (authCode) =>
            {
                Debug.Log("Disc pool 360: Received Auth Code from Google Play Games Services."+authCode);

                if (string.IsNullOrEmpty(authCode)) return;


                Debug.Log("Disc pool 360: Received Google Auth Code, logging into PlayFab...");
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
           

            cloudData.PlayFabLoginSuccess();

            //// OPTIONAL: Update PlayFab display name to match Google name
            //UpdatePlayFabDisplayName(Social.localUser.userName);
        }

        private void OnPlayFabFailure(PlayFabError error)
        {
            Debug.Log("Disc Pool: PlayFab Login Error: " + error.GenerateErrorReport());

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
    }
}