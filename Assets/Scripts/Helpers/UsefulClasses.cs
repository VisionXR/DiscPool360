using System.Collections.Generic;
using System;
using UnityEngine;
using Fusion;

namespace com.VisionXR.HelperClasses
{
    [Serializable]
    public class DestroyCoinsList
    {
        public List<string> coins = new List<string>();
    }


    [Serializable]
    public class NetworkPlayer
    {
         public int myId;
         public ulong myOculusID;
         public int myStrikerId;
         public string myName;
         public PlayerCoin myCoin;
         public Team myTeam;
         public PlayerType myPlayerType;
         public AIDifficulty aIDifficulty;
         public string imageURL;
         
    }


    [Serializable]
    public class CoinData
    {
       
        public string Position;
        public string Rotation;
  
    }

    [Serializable]
    public class TransformData
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }
    [Serializable]
    public class StrikerData
    {
       
        public string Position;
        public string Rotation;
     
    }

    [Serializable]
    public class SnapShotData
    {
        public int frameNumber;
        public List<coinSnapShotData> allCoinsSnapShotData;
        public coinSnapShotData strikerSnapShotData;
    }

    [Serializable]
    public class coinSnapShotData
    {
        
        public string Position;
        public string Rotation;
        public string Velocity;

    }

        [Serializable]
    public class AudioData
    {       
        public AudioType audioType;
        public float volume = 1;

    }

    [Serializable]
    public class StartGame
    {
        public string time;
        public int id;
    }


    [Serializable]
    public class GameResult
    {
        public int currentTurnId;
        public int winningPlayerId;
        public Team winningTeam;
        public bool isVictory;
    }

    [Serializable]
    public class AIBotAnimationDetails
    {
        public string time;
        public int myId;
        public int eventId;
        public string coinPosition;
        public string strikerPosition;
        public string strikerRotation;
    }
 

    [Serializable]
    public class PlayerData
    {
        public int StrikerId;
        public int BoardId;
        public bool isPassThroughOn;
        public ServerRegion region;

    }
    [Serializable]
    public class AvatarData
    {
         public Vector3Compressed HeadPos { get; set; }
         public Vector3Compressed HeadRot { get; set; }

         public Vector3Compressed LeftHandPos { get; set; }
         public Vector3Compressed LeftHandRot { get; set; }

         public Vector3Compressed RightHandPos { get; set; }
         public Vector3Compressed RightHandRot { get; set; }

    }
    [Serializable]
    public class NotificationMessage
    {
    
        public int MyBoard;
        public AIDifficulty difficulty;
        public string roomName;
        public string playerName;
    }



    [Serializable]
    public class LinkData
    {
        public GameType gameType;
        public GameMode gameMode;
        public string roomName;
    }


    [Serializable]
    public class AssetData
    {
        public string skuName;
        public bool isPurchased = false;
        public string Price;
    }

    [Serializable]
    public class ProfileImage
    {
        public Sprite image;
    }

        [Serializable]
    public class Friend
    {
        public ulong FriendID;
        public string FriendName;
     
    }

    [Serializable]
    public class AvailableRooms
    {
        public string  roomName;
        public int MyBoard;
        public AIDifficulty aiDifficulty;
    }

    [Serializable]
    public class PlayerProperties
    {
        public int myId;
        public string myName;
        public string myOculusID;
        public string imageURL;
        public Sprite myImage;
     

        public PlayerControl myPlayerControl;
        public PlayerType myPlayerType;
        public PlayerCoin myCoin;
        public AIDifficulty myAiDifficulty;

    }

    [Serializable]
    public class CoinInfo
    {
        public GameObject StrikerPos;
        public GameObject Coin;
        public GameObject Hole;
        public float angle;
        public Vector3 FinalPos;
        public bool isBlockedH = false;
        public bool isBlockedC = false;
        public float distance;
        public GameObject blockedCoinAlongStriker;
        public GameObject blockedCoinAlongHole;
    }

    [Serializable]
    public class UserData
    {

        public string lastLoginDate;
        public int totalLogins;

        public int spTotalGames;
        public int spTotalWins;
        public int spPoolEasyWins;
        public int spPoolMediumWins;
        public int spPoolHardWins;
        public int spSnookerEasyWins;
        public int spSnookerMediumWins;
        public int spSnookerHardWins;

        public int mpTotalGames;
        public int mpTotalWins;
        public int mpPoolWins;
        public int mpSnookerWins;

    }
    [Serializable]

    public class AchievementInfo
    {
        public int id;
        public Sprite icon;
        public string name;
        public string apiName;
        public string achievementName;
        public string description;

        public GameType type;
        public GameMode mode;
        public BoardType boardType;
        public AIDifficulty difficulty;
        public AchievementSection achievementSection;


        public AchievementType achievementType;
        public bool isAchieved;
        public int actual;
        public int target;

    }


}
