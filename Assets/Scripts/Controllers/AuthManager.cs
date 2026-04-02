using com.VisionXR.ModelClasses;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class AuthManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO playerSettings;
        public UIDataSO uiData;

        void Start()
        {
            if(Application.isEditor)
            {
                EditorLogin();
            }
            else
            {
                GoogleLogin();
            }
        }


        private void EditorLogin()
        {
            // Simplified Editor Mock
            playerSettings.SetUserNameAndId("Guest_Player",UnityEngine.Random.Range(0, 9999).ToString());
            uiData.TriggerHomeEvent();
            

        }


        public void GoogleLogin()
        {
                 Debug.Log("Trying to login!");
                PlayGamesPlatform.Activate();
                PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            

        }

        internal void ProcessAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                Debug.Log("Disc Clash: Google Login Successful!");

                // 1. First, set local UI data (Name and Image)
                string name = Social.localUser.userName;
                string googleID = Social.localUser.id;
                StartCoroutine(LoadProfileImage());

                playerSettings.SetUserNameAndId(name, googleID);
                uiData.TriggerHomeEvent();

            }

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
                Debug.Log("Disc Clash: Profile Image Loaded!");
            }
        }

        public Sprite ConvertTextureToSprite(Texture2D texture)
        {
            if (texture == null) return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}