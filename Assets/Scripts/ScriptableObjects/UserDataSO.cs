using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "UserDataSO", menuName = "ScriptableObjects/UserDataSO", order = 1)]
    public class UserDataSO : ScriptableObject
    {
        // User Data

        public string MyName;
        public Sprite MyProfileImage;
        public string MyImageUrl;
        public string MyOculusId;
        public float bgMusicVolume = 1f;
        public int myBoard = 0;
        public CoinsType myCoins;
        public ServerRegion myServerRegion = ServerRegion.any;
        public string linkAPI = "801d8b68de6c89601e1787f26b272080";
        public List<Sprite> AIImages;
        public Action<int> BoardChangedEvent;
        public Action CreateSameBoardEvent;

        // Methods

      
        public void SetUserNameAndId(string userName, string Id)
        {     
            MyName = userName;
            MyOculusId = Id;
        }

        public void SetProfileUrl(string url)
        {
            MyImageUrl = url;

        }
        public void SetUserProfileImage(Sprite s)
        {
            MyProfileImage = s;

        }

        public void SetBoard(int id)
        {
            myBoard = id;
            BoardChangedEvent?.Invoke(id);
        }

        public void SetMyCoins(int id)
        {
            myCoins = (CoinsType)id;
        }

        public void CreateSameBoard()
        {

            CreateSameBoardEvent?.Invoke();
        }
    }
}
