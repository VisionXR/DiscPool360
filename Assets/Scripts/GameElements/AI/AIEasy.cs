using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.VisionXR.GameElements
{

    public class AIEasy : MonoBehaviour,IAIBehaviour
    {
        [Header(" Scriptable Objects")]
        public AIDataSO aIData;
        public InputDataSO inputData;
        public BoardDataSO boardData;
        public StrikerDataSO strikerData;
        public CoinDataSO coinData;
        public UIDataSO uiData;
        public GameDataSO gameData;


        [Header(" Local Game Objects")]
        public GameObject Striker;
        public GameObject PlatformObject;
        public PlayerCoin playerCoin = PlayerCoin.Stripe; 
        public List<CoinInfo> hitCoinList = new List<CoinInfo>();
        public List<GameObject> holes;
        public LineRenderer debugLine;
        public GameObject AllAssets;


        [Header(" Local Variables")]
        public EmotionChanger emotionChanger;
        public int emotionIndex;
        private Platform platform;
        public float forceAdder = 0.2f;
        public float strikerOffset = 0.005f / 2;
        [SerializeField] private Sprite AIIcon;        
        [SerializeField] private float CutOffAngle = 15;
        [SerializeField] private  int MyId = 2;


        // local variables
        public bool isExcecuting = false;      
        private Vector3 dir;
        private float force;
       

        
      
        IEnumerator Start()
        {
           
            PlatformObject = GameObject.FindGameObjectWithTag("Platform");
            platform = PlatformObject.GetComponent<Platform>();
            Striker = GameObject.FindGameObjectWithTag("Striker");
            debugLine = GameObject.Find("DebugLine").GetComponent<LineRenderer>();
            AllAssets = GameObject.Find("AllAssets");
            holes = boardData.Holes;

            yield return new WaitForSeconds(1);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
       
            isExcecuting = false;

            aIData.PlayHandAnimation("Hello", true);
            yield return new WaitForSeconds(5);
            aIData.PlayHandAnimation("Hello", false);
        }

        void OnEnable()
        {
            gameData.PlayAgainEvent += FindObjectsAgain;
            aIData.CoinInformationReceivedEvent += OnHitListReceived;
        }

        void OnDisable()
        {
            gameData.PlayAgainEvent -= FindObjectsAgain;
            aIData.CoinInformationReceivedEvent -= OnHitListReceived;
        }

        private void FindObjectsAgain(int id)
        {
            StartCoroutine(WaitAndLoad());
        }

        private IEnumerator WaitAndLoad()
        {
            yield return new WaitForSeconds(0.1f);

            PlatformObject = GameObject.FindGameObjectWithTag("Platform");
            platform = PlatformObject.GetComponent<Platform>();
            Striker = GameObject.FindGameObjectWithTag("Striker");
            debugLine = GameObject.Find("DebugLine").GetComponent<LineRenderer>();
            holes = boardData.Holes;

        }

        public void ExecuteShot(PlayerCoin coin)
        {

            if (!isExcecuting)
            {
                isExcecuting = true;
                playerCoin = coin;
                hitCoinList.Clear();


                if (uiData.currentGameMode == GameMode.Pool)
                {

                    CoinSorter.instance.SortAllPoolCoins(MyId, playerCoin, holes, Striker.transform.position);
                }
                else if (uiData.currentGameMode == GameMode.Snooker)
                {
                    CoinSorter.instance.SortAllSnookerCoins(MyId, playerCoin, holes, Striker.transform.position);
                }

            }

            // ExecuteFoul();
        }

        public void ExecuteFoul()
        {
            Debug.Log("AI Foul Shot Executed");
          //  StartCoroutine(Strike(holes[0].transform.position,1.5f));

            Vector3 direction = (holes[0].transform.position  - Striker.transform.position).normalized;
            Striker.GetComponent<StrikerMovement>().RotateToOld(direction);
            Striker.GetComponent<StrikerShooting>().Fire(1);

        }
        

        private void OnHitListReceived(int id, List<CoinInfo> list)
        {
            
            isExcecuting = false;
            if (MyId == id)
            {
                
                hitCoinList = list;
                StartCoroutine(StartExecutingStrike());
            }
        }

        private IEnumerator StartExecutingStrike()
        {

            yield return new WaitForSeconds(aIData.calculatingShotTime);
            yield return StartCoroutine(HitCoin());
        }

        private IEnumerator HitCoin()
        {
          
            yield return new WaitForSeconds(1);

            CoinInfo currentSelectedCoin = hitCoinList[0];

            // Strike if angle is within range
            if (currentSelectedCoin.angle < CutOffAngle)
            {
                
                // Non-linear weighting for angle contribution
                float a = Mathf.Clamp01(currentSelectedCoin.angle / CutOffAngle);
                // Choose one curve:
                float w = a * a ;                  

                force = currentSelectedCoin.distance + w * forceAdder + 0.3f;

                ShowDebugLines(hitCoinList[0],true);            
                yield return StartCoroutine(Strike(currentSelectedCoin.finalPosition, force));
            }
            else
            {
                // Set force and striker position
                force = currentSelectedCoin.distance + 1;
            
                ShowDebugLines(hitCoinList[0], false);
                yield return StartCoroutine(Strike(currentSelectedCoin.Coin.transform.position, force));
            }

        }

        private IEnumerator Strike(Vector3 finalPosition, float strikeForce)
        {
            aIData.StartLeftHandRotation(true);
            aIData.PlayLeftHandAnimation("LeftGrip", true);
            yield return new WaitForSeconds(0.5f);

            platform.TurnOnBoardHighlight();
            StartCoroutine(RotatePlatform(Striker.transform.position, transform.position));

            yield return new WaitForSeconds(1f);

            platform.TurnOffBoardHighlight();
            aIData.StartLeftHandRotation(false);
            aIData.PlayLeftHandAnimation("LeftGrip", false);
            yield return new WaitForSeconds(0.5f);

            aIData.StartRightHandMove(true);
            aIData.PlayHandAnimation("RightGrip",true);

            yield return new WaitForSeconds(1.1f);

            Vector3 finalPosLocal = PlatformObject.transform.TransformPoint(debugLine.GetPosition(1));
            Vector3 direction = (finalPosLocal - Striker.transform.position).normalized;


            Striker.GetComponent<StrikerMovement>().RotateToOld(direction);
            aIData.StartRightHandRotation(direction);
            yield return new WaitForSeconds(0.5f);

            aIData.PlayHandAnimation("RightThumbsUp",true);
            Striker.GetComponent<StrikerShooting>().StartArrowChange();


            yield return new WaitForSeconds(1.1f);

            Striker.GetComponent<StrikerShooting>().StopArrowChange();
            aIData.PlayHandAnimation("RightThumbsUp",false);

            emotionChanger.SetEmotionEyes(emotionIndex);

            yield return new WaitForSeconds(1f);

            Striker.GetComponent<StrikerShooting>().Fire(strikeForce);

            yield return new WaitForSeconds(1.1f);

            aIData.StartRightHandMove(false);
            aIData.PlayHandAnimation("RightGrip", false);
        

            hitCoinList.Clear();
        }

        private IEnumerator RotatePlatform(Vector3 strikerPosition, Vector3 aiPosition)
        {
            AllAssets.transform.SetParent(platform.transform);

            coinData.TurnOffRigidBodies();

            // Center of rotation (platform pivot)
            Vector3 center = PlatformObject.transform.position;

            // Direction vectors from center (planar)
            Vector3 vStriker = strikerPosition - center;
            Vector3 vAI = aiPosition - center;
            vStriker.y = 0f;
            vAI.y = 0f;

            // Abort if one is degenerate
            if (vStriker.sqrMagnitude < 0.0001f || vAI.sqrMagnitude < 0.0001f)
            {
                yield break;
            }

            vStriker.Normalize();
            vAI.Normalize();

            // Signed angle from striker vector to AI vector (Y axis)
            float angle = Vector3.SignedAngle(vStriker, vAI, Vector3.up);

            // If already nearly aligned skip
            if (Mathf.Abs(angle) < 0.5f)
            {
                yield break;
            }

            Quaternion startRot = PlatformObject.transform.rotation;
            Quaternion targetRot = startRot * Quaternion.AngleAxis(angle, Vector3.up);

            float duration = 1.0f;
            float t = 0f;
            while (t < duration)
            {
                PlatformObject.transform.rotation = Quaternion.Slerp(startRot, targetRot, t / duration);
                t += Time.deltaTime;
                yield return null;
            }


            PlatformObject.transform.rotation = targetRot;

            PlatformObject.transform.localEulerAngles = new Vector3(0f, PlatformObject.transform.localEulerAngles.y, 0f);

            coinData.TurnOnRigidBodies();

            AllAssets.transform.SetParent(null);
        }

        public void ShowDebugLines(CoinInfo coinInfo, bool canIHit)
        {


            if (debugLine != null)
            {
                // Set random color
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                debugLine.startColor = randomColor;
                debugLine.endColor = randomColor;

                // Set thickness
                debugLine.startWidth = 0.005f;
                debugLine.endWidth = 0.005f;

                // Convert world positions to local positions relative to Platform
                Vector3 strikerPosLocal = PlatformObject.transform.InverseTransformPoint(coinInfo.strikerPosition);

                Vector3 holePosLocal = PlatformObject.transform.InverseTransformPoint(coinInfo.Hole.transform.position);

                Vector3 finalPosLocal;
                if (canIHit)
                {
                    finalPosLocal = PlatformObject.transform.InverseTransformPoint(coinInfo.finalPosition);

                }
                else
                {
                    finalPosLocal = PlatformObject.transform.InverseTransformPoint(coinInfo.Coin.transform.position);

                }


                debugLine.positionCount = 3;
                debugLine.SetPosition(0, strikerPosLocal);
                debugLine.SetPosition(1, finalPosLocal);
                debugLine.SetPosition(2, holePosLocal);

              //  debugLine.enabled = true;
            }

        }
    }
}

