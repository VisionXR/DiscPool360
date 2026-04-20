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
        public DominantHand myDominantHand;
        public float bgMusicVolume = 1f;
        public int myBoard = 0;
        public int myCoins = 0;
        public ServerRegion myServerRegion = ServerRegion.any;

        public List<Sprite> AIImages;
        public Action<int> BoardChangedEvent;
        public Action CreateSameBoardEvent;

        // Events

        public void SetMyName(string Name)
        {
            MyName = Name;
        }

        public void SetUserNameAndId(string userName, string Id)
        {
            Debug.Log("Setting User Name and Id: " + userName + " | " + Id);
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

        public string GetMyName()
        {
            return MyName;
        }

        public string GetMyImage()
        {
            return MyImageUrl;
        }

       public void SetVolume(float volume)
        {
            bgMusicVolume = volume;
        }

        public void SetDominantHand(DominantHand hand)
        {
            myDominantHand = hand;
        }

        public void SetBoard(int id)
        {
            myBoard = id;
            BoardChangedEvent?.Invoke(id);
        }

        public void SetMyCoins(int id)
        {
            myCoins = id;
        }

        public void CreateSameBoard()
        {

            CreateSameBoardEvent?.Invoke();
        }

        public void SetServerRegion(ServerRegion region)
        {
            myServerRegion = region;
        }
    }
}
