using com.VisionXR.HelperClasses;
using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "NetworkInputDataSO", menuName = "ScriptableObjects/NetworkInputDataSO", order = 1)]
    public class NetworkInputDataSO : ScriptableObject
    {
        // User Data

      
        // Actions
        public Action<ServerRegion, string, Action, Action<string>> CreateRoomEvent;
        public Action<ServerRegion, string, Action, Action<string>> JoinRoomEvent;
        public Action<ServerRegion, string, Action, Action<string>> JoinLobbyEvent;

        public Action LeaveRoomEvent;

        // Methods
        public void CreateRoom(ServerRegion region, Action OnSuccess, Action<string> OnFailed)
        {
            CreateRoomEvent?.Invoke(region, string.Empty, OnSuccess, OnFailed);
        }

        public void JoinRoom(ServerRegion region, string roomName, Action OnSuccess, Action<string> OnFailed)
        {
            JoinRoomEvent?.Invoke(region, roomName, OnSuccess, OnFailed);
        }

        public void JoinLobby(ServerRegion region, string lobbyName, Action LobbySuccessEvent, Action<string> LobbyFailedEvent)
        {
            JoinLobbyEvent?.Invoke(region, lobbyName, LobbySuccessEvent, LobbyFailedEvent);
        }

        public void LeaveRoom()
        {
            LeaveRoomEvent?.Invoke();
        }

    }
}
