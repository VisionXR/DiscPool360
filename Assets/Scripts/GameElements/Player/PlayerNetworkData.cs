using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace com.VisionXR.HelperClasses
{
    public class PlayerNetworkData : NetworkBehaviour
    {
        [Header("Scriptable Objects")]
        public NetworkInputDataSO networkInputData;
        public NetworkOutputDataSO networkOutputData;
        public TableDataSO tableData;
        public GameDataSO gamedata;
        public CoinDataSO coinData;
        public UIDataSO uiData;
        public PlayerDataSO playerData;
        public StrikerDataSO strikerData;
        public UserDataSO UserData;
        public InputDataSO inputData;

        [Header("local Objects")]
        public Player player;
       
        public ReceivePlayerData receivePlayerData;


        [Header("Network Variables")]
        [Networked, OnChangedRender(nameof(OnActiveCoinNetworkDataReceived))] public ActiveCoinsData activeCoinsNetworkData { get; set; }


        // Replaced string JSON with strongly-typed networked struct
        [Networked, OnChangedRender(nameof(OnPlayerPropertiesReceived))] public NetworkPlayerProperties NetPlayerProps { get; set; }

      

        public override void Spawned()
        {
            base.Spawned();

            if (!HasStateAuthority)
            {

                OnPlayerPropertiesReceived();
            }
        }

        private void OnEnable()
        {
            inputData.GrabInputEvent += GrabInput ;
            inputData.TapInputEvent += TapInput;
            inputData.PlatformHiglightEvent += PlatformHighlight;
        }

        private void OnDisable()
        {
            inputData.GrabInputEvent -= GrabInput;
            inputData.TapInputEvent -= TapInput;
            inputData.PlatformHiglightEvent -= PlatformHighlight;
        }

        private void PlatformHighlight(bool val)
        {
            if (HasStateAuthority)
            {
                RPC_PlatformHighlight(val);
            }
        }

        private void TapInput(DominantHand hand, bool arg2)
        {
            if(HasStateAuthority)
            {
                RPC_TapInput(hand, arg2);
            }
        }

        private void GrabInput(DominantHand hand, bool arg2)
        {
            if(HasStateAuthority)
            {
                RPC_GrabInput(hand, arg2);
            }
        }

        #region setters
        public void SetPlayerData(PlayerProperties p)
        {
            PlayerControl myControl = PlayerControl.Local;
            if (!HasStateAuthority)
            {
                myControl = PlayerControl.Remote;
            }

            // Map PlayerProperties to NetworkPlayerProperties
            NetPlayerProps = new NetworkPlayerProperties
            {
                MyId = p.myId,
                MyName = p.myName,
                MyOculusID = p.myOculusID,
                ImageURL = p.imageURL,
                MyPlayerControl = myControl,
                MyPlayerType = p.myPlayerType,
                MyCoin = p.myCoin,
                MyAiDifficulty = p.myAiDifficulty
            };

            gameObject.name = "Player" + NetPlayerProps.MyId;
            tableData.SetTableRotation(p.myId);
     


        }

        public void SetActiveCoinsNetworkData(ActiveCoinsData data)
        {
            activeCoinsNetworkData = data;
        }



        // Getters
        private void OnActiveCoinNetworkDataReceived()
        {
            for (int i = 0; i < coinData.AvailableCoinsInGame.Count; i++)
            {              
                Rigidbody rb = coinData.AvailableCoinsInGame[i];            
                rb.gameObject.SetActive(activeCoinsNetworkData.Status[i]);
                networkOutputData.UpdateCoins();
            }


        }
        #endregion

        #region Receivers
        public void OnPlayerPropertiesReceived()
        {
            // Build local PlayerProperties from networked struct
            var props = new PlayerProperties
            {
                myId = NetPlayerProps.MyId,
                myName = NetPlayerProps.MyName.ToString(),
                myOculusID = NetPlayerProps.MyOculusID,
                imageURL = NetPlayerProps.ImageURL.ToString(),
                myPlayerControl = HasStateAuthority ? PlayerControl.Local : PlayerControl.Remote,
                myPlayerType = NetPlayerProps.MyPlayerType,
                myCoin = NetPlayerProps.MyCoin,
                myAiDifficulty = NetPlayerProps.MyAiDifficulty
            };

            // Optional: enable voice for human local player
            // if (HasStateAuthority && props.myPlayerType == PlayerType.Human) { myRecorder.RecordingEnabled = true; }

            // Apply to Player component if available
            if (player != null)
            {

                player.SetProperties(props);
            }

            gameObject.name = "Player" + props.myId;

            gameObject.transform.position = tableData.PlayerTransforms[props.myId - 1].position;
            gameObject.transform.rotation = tableData.PlayerTransforms[props.myId - 1].rotation;


            if (!HasStateAuthority)
            {

                if (NetPlayerProps.ImageURL != "")
                {
                    StartCoroutine(LoadAvatarImage(props.imageURL));
                }
            }
            else
            {
                player.playerProperties.myImage = UserData.MyProfileImage;
                playerData.PlayerImageReceived(player.playerProperties.myId, UserData.MyProfileImage);
            }

        }



        #endregion

      

        #region RPCS



        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_HostReady()
        {
            networkOutputData.SetHostReady(true);        
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_ClientReady()
        {
           networkOutputData.SetClientReady(true);          
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_GrabInput(DominantHand hand,bool value)
        {
            if (!HasStateAuthority)
            {
                if(hand == DominantHand.Right)
                {
                   
                }
                else
                {
                   
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_TapInput(DominantHand hand, bool value)
        {
            if (!HasStateAuthority)
            {
                if (hand == DominantHand.Right)
                {

                }
                else
                {

                }
            }
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PlatformHighlight(bool value)
        {
            if (!HasStateAuthority)
            {
               if(tableData.platform != null)
                {
                    if(value)
                    {
                        tableData.platform.TurnOnBoardHighlight();
                    }
                    else
                    {
                        tableData.platform.TurnOffBoardHighlight();
                    }
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_SetWinner(int id)
        {
            if (!HasStateAuthority)
            {
                networkOutputData.SetWinner(id);
            }
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_SetFoul(int id)
        {
            if (!HasStateAuthority)
            {
                strikerData.HandleFoul(id);
            }
        }



        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_FoulComplete()
        {
            if (!HasStateAuthority)
            {
                GameObject striker = strikerData.currentStriker;
                striker.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_SetAssignedCoins(PlayerCoin coin1, PlayerCoin coin2)
        {
            if (!HasStateAuthority)
            {
                networkOutputData.SetPlayerCoins(coin1, coin2);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_ChangeTurn(int id)
        {
            if (!HasStateAuthority)
            {
                gamedata.ChangeTurn(id);
            }
                     
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_UpdateSnookerScore(int requiredColorIndex, SnookerPhase phase, int p1Score, int p2Score)
        {
            if (!HasStateAuthority)
            {
                networkOutputData.UpdateSnookerScore(requiredColorIndex, phase, p1Score, p2Score);
            }
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PlayerStrikeStarted(float force,Vector3 dir)
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.PlayerStrikeStarted(force, dir);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
        public void RPC_StrikeForceChanged(float force,Vector3 dir)
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.StrikeForceChanged(force,dir);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PlayerStrikeForceStarted()
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.PlayerStrikeForceStarted();
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PlayerStrikeEnded()
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.PlayerStrikeEnded();
            }
        }




        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, Channel = RpcChannel.Reliable)]
        public void RPC_SendAIData(string data)
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.ReceiveAIData(data);
            }
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, Channel = RpcChannel.Unreliable)]
        public void RPC_SendCoinRotation(float val)
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.ReceiveCoinRotationData(val);
            }
        }
        #endregion

        private IEnumerator LoadAvatarImage(string url)
        {
            Debug.Log("Url " + url);
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to download avatar: " + uwr.error);
                    yield break;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                player.playerProperties.myImage = s;
                playerData.PlayerImageReceived(player.playerProperties.myId, s);
            }
        }
    }
}