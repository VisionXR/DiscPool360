using Fusion;
using NUnit.Framework;
using System;
using UnityEngine;

namespace com.VisionXR.HelperClasses
{
    [Serializable]
    public class OculusAvatarData
    {
        public byte[] data;
    }

    [Serializable]
    public struct StrikerSnapshot : INetworkStruct
    {
        [Networked] public Vector3Compressed Position { get; set; }
        [Networked] public Vector3Compressed Rotation { get; set; }

        [Networked] public Vector3Compressed Velocity { get; set; }

    }


    [Serializable]
    public struct CoinSnapshot : INetworkStruct
    {
        [Networked] public Vector3Compressed Position { get; set; }
        [Networked] public Vector3Compressed Rotation { get; set; }
        [Networked] public Vector3Compressed Velocity { get; set; }
    }

    [Serializable]
    public struct CoinTransformData : INetworkStruct
    {
        [Networked] public Vector3Compressed Position { get; set; }
        [Networked] public Vector3Compressed Rotation { get; set; }
    
    }

    [Serializable]
    public struct PlatformSnapshot : INetworkStruct
    {
        [Networked] public Vector3Compressed Rotation { get; set; }
    }

    [Serializable]
    public struct ActiveCoinsData : INetworkStruct
    {
      
       [Networked, Capacity(22)] public NetworkArray<NetworkBool> Status => default;
       [Networked, Capacity(22)] public NetworkArray<CoinTransformData> AllCoinsData => default;


    }


    [Serializable]
    public struct GameSnapshot : INetworkStruct
    {
        [Networked] public int FrameNumber { get; set; }
        [Networked] public StrikerSnapshot Striker { get; set; }

        // You can adjust 22 if you want a smaller/larger max coin count
        [Networked, Capacity(22)] public NetworkArray<CoinSnapshot> Coins => default;
    }

    // Networked player properties: use Fusion Networked properties.
    // Strings must use NetworkString with a fixed capacity. Unity Sprite cannot be networked; use URL.
    [Serializable]
    public struct NetworkPlayerProperties : INetworkStruct
    {
        [Networked] public int MyId { get; set; }
        [Networked] public string MyOculusID { get; set; }

        // Choose capacities suitable for your expected lengths
        [Networked] public NetworkString<_64> MyName { get; set; }
        [Networked] public NetworkString<_512> ImageURL { get; set; }

        [Networked] public PlayerControl MyPlayerControl { get; set; }
        [Networked] public PlayerType MyPlayerType { get; set; }
        [Networked] public PlayerCoin MyCoin { get; set; }
        [Networked] public AIDifficulty MyAiDifficulty { get; set; }
    }



    [Serializable]
    public struct NetworkAvatarData : INetworkStruct
    {
        [Networked] public Vector3Compressed HeadPos { get; set; }
        [Networked] public Vector3Compressed HeadRot { get; set; }

        [Networked] public Vector3Compressed LeftHandPos { get; set; }
        [Networked] public Vector3Compressed LeftHandRot { get; set; }

        [Networked] public Vector3Compressed RightHandPos { get; set; }
        [Networked] public Vector3Compressed RightHandRot { get; set; }

    }
}
