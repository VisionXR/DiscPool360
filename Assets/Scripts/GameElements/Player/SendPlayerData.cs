using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using System.Collections;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class SendPlayerData : NetworkBehaviour
    {

        [Header("Scriptable Objects")]
        public UserDataSO userDataSO;
        public GameDataSO gameData;
        public CoinDataSO coinData;
        public NetworkOutputDataSO outputData;
     

        [Header("local Objects")]
        public Player player;
        
        public PlayerNetworkData networkData;

        // Local Variables
        public int currentFrameNumber;
        public bool canISendSnapShot;
        private Coroutine sendStrikerDataRoutine;
       


        public IEnumerator Start()
        {
            yield return new WaitForSeconds(10);
       
        }
        private void OnEnable()
        {
          
   
        }

        private void OnDisable()
        {
            

            StopAllCoroutines();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_StartGame(int playerId,int coinsId)
        {
            
            outputData.StartGame(playerId,coinsId);
        }


    
        public IEnumerator SendAvatarData()
        {
            while (true)
            {
                yield return new WaitForSeconds(4.0f * Time.fixedDeltaTime);

               // networkData.SetAvatarData(player.localAvatarTransform.GetAvatarData());
            }
        }

    }

}
                                                                        