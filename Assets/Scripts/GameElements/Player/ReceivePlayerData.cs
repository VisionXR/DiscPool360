using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivePlayerData : NetworkBehaviour
{
    [Header("Scriptable Objects")]
    public UserDataSO UserData;
    public GameDataSO gameData;
    public CoinDataSO coinData;
    public BoardDataSO boardData;
    public StrikerDataSO strikerData;
    public PlayerDataSO playerData;
    public UIDataSO uiData;
    public TableDataSO tableData;

    [Header("local Objects")]
    public Player player;
    public PlayerNetworkData networkData;
    public PlayerInput playerInput;
    public PlayerFoul playerFoul;
    public LayerMask hitMask;



    // Networked snapshots (platform-local space for positions/rotations/velocities)
    [Header("structures")]
    [Networked, OnChangedRender(nameof(OnGameSnapShotReceived))][HideInInspector] public GameSnapshot currentGameSnapshot { get; set; }
    [Networked, OnChangedRender(nameof(OnPlatformDataReceived))][HideInInspector] public PlatformSnapshot platforSnapshotData { get; set; }
    [Networked, OnChangedRender(nameof(OnStrikerDataReceived))][HideInInspector] public StrikerSnapshot strikerSnapshot { get; set; }



    public int currentSentFrameNumber;
    public int currentReceivedFrameNumber;
    public int currentRunningFrameNumber;
    public int currentStrikerNumber;



    public bool canISendSnapShot;
    public bool canIReceiveSnapShot;
    public bool canIReceiveData;
    public bool canISendData;
    public bool canISendStrikerData;



    private List<Rigidbody> coinRbs;
    private Rigidbody strikerRigidbody;
    public Rigidbody platformRb;
    public GameObject platform;
    public GameObject allAssets;


    private StrikerShooting strikerShooting;
    private int previousTurnId = -1;

    private void OnEnable()
    {
        gameData.TurnChangeEvent += OnTurnChanged;
        playerInput.PlatformRotatedEvent += RotatePlatform;
        tableData.PlatformRotationChangedEvent += OnPlatformRotaionReceived;
    }

    private void OnDisable()
    {
        gameData.TurnChangeEvent -= OnTurnChanged;
        playerInput.PlatformRotatedEvent -= RotatePlatform;
        tableData.PlatformRotationChangedEvent -= OnPlatformRotaionReceived;
    }

    private void OnTurnChanged(int id)
    {

        if (strikerRigidbody == null)
        {
            var strikerGo = strikerData.currentStriker;
            if (strikerGo != null)
            {
                strikerRigidbody = strikerGo.GetComponent<Rigidbody>();
                strikerShooting = strikerGo.GetComponent<StrikerShooting>();
            }
        }

        if (platform == null)
        {
            platform = tableData.GetPlatform().gameObject;
            if (platform != null)
            {
                platformRb = platform.GetComponent<Rigidbody>();
            }
        }

        if (allAssets == null)
        {
            allAssets = tableData.GetAllAssets();
        }

        if (player.playerProperties.myId == id && HasStateAuthority)
        {
            SendStrikerData();
        }

    }
    private void OnPlatformRotaionReceived(Vector3 rotation)
    {
        if (HasStateAuthority)
        {
            PlatformSnapshot platformSnapshot = new PlatformSnapshot();
            platformSnapshot.Rotation = rotation;
            SetPlatformSnapShotData(platformSnapshot);
        }
    }

    private void OnStrikerDataReceived()
    {
        if (canIReceiveData || HasStateAuthority)
        {
            return;
        }

        strikerRigidbody.position = strikerSnapshot.Position;
        strikerRigidbody.transform.eulerAngles = strikerSnapshot.Rotation;

    }
    private void OnPlatformDataReceived()
    {

        if (canIReceiveData || HasStateAuthority)
        {
            return;
        }

        if (!strikerData.isFoul)
        {
            strikerData.TurnOffRigidBody();
        }

        coinData.TurnOffRigidBodies();

        allAssets.transform.SetParent(platform.transform);
        Vector3 localAngles = platforSnapshotData.Rotation;
        Quaternion incomingLocalRot = Quaternion.Euler(localAngles);
        platform.transform.rotation = incomingLocalRot;
        allAssets.transform.SetParent(null);

        if (!strikerData.isFoul)
        {
            strikerData.TurnOnRigidBody();
        }

        coinData.TurnOnRigidBodies();

    }

    private void RotatePlatform()
    {
        PlatformSnapshot platformSnapshot = new PlatformSnapshot();
        platformSnapshot.Rotation = platform.transform.eulerAngles;
        SetPlatformSnapShotData(platformSnapshot);
    }

    public void SendStrikerData()
    {
        canISendStrikerData = true;
    }

    public void SendData()
    {

        canISendStrikerData = false;
        canISendData = true;
        currentSentFrameNumber = 0;
        currentStrikerNumber = 0;

    }

    public void ReceiveData()
    {

        canIReceiveData = true;
        currentRunningFrameNumber = 0;
        currentReceivedFrameNumber = 0;

    }

    public void PlayerStrikeStarted(float force, Vector3 direction)
    {
        
        strikerShooting.TurnOffArrow();
        strikerShooting.SetForceAndDir(force, direction);
        ReceiveData();
    }
    public void PlayerStrikeForceStarted(float force)
    {
     
        strikerShooting.SetStrikerForce(force);
    }
    public void PlayerStrikeEnded()
    {
        Reset();
    }

    public void SetPlatformSnapShotData(PlatformSnapshot newPlatformSnapshot)
    {
        platforSnapshotData = newPlatformSnapshot;
    }

    public void SetGameSnapShotdata(GameSnapshot newgameSnapshot)
    {
        currentGameSnapshot = newgameSnapshot;
    }

    public void SetStrikerSnapShotData(StrikerSnapshot newStrikerSnapshot)
    {
        strikerSnapshot = newStrikerSnapshot;
    }


    private void OnGameSnapShotReceived()
    {

        if (canIReceiveData && !HasStateAuthority)
        {

            canIReceiveSnapShot = true;
        }
    }

    public void ReceiveCoinRotationData(float val) { }

    public void ReceiveAIData(string data) { }

    private void FixedUpdate()
    {
        // receiving
        if (canIReceiveData)
        {
            currentRunningFrameNumber++;
        }


        if (canIReceiveData && canIReceiveSnapShot)
        {
            ProcessGameSnapShot();
            canIReceiveSnapShot = false;
        }


        // sending game data
        if (canISendData && HasStateAuthority)
        {
            currentSentFrameNumber++;
            if (currentSentFrameNumber % playerData.SendRate == 0)
            {
                SetGameSnapShotdata(GetGameSnapshot());
            }

        }

        // sending game data
        if (canISendStrikerData && HasStateAuthority)
        {
            StrikerSnapshot strikerDataToSend = new StrikerSnapshot
            {
                Position = strikerRigidbody.position,
                Rotation = strikerRigidbody.transform.eulerAngles,
                Velocity = strikerRigidbody.linearVelocity
            };
            SetStrikerSnapShotData(strikerDataToSend);
        }

    }


    private GameSnapshot GetGameSnapshot()
    {
        GameSnapshot snapshot = new GameSnapshot();


        snapshot.FrameNumber = currentSentFrameNumber;

        StrikerSnapshot strikerData = new StrikerSnapshot
        {
            Position = strikerRigidbody.position,
            Rotation = strikerRigidbody.transform.eulerAngles,
            Velocity = strikerRigidbody.linearVelocity
        };

        snapshot.Striker = strikerData;

        // Build coin snapshots
        for (int i = 0; i < coinData.AvailableCoinsInGame.Count; i++)
        {
            var coinRb = coinData.AvailableCoinsInGame[i];

            CoinSnapshot coinSnapshot = new CoinSnapshot
            {
                Position = coinRb.position,
                Rotation = coinRb.transform.eulerAngles,
                Velocity = coinRb.linearVelocity
            };

            snapshot.Coins.Set(i, coinSnapshot);
        }

        return snapshot;
    }

    public void ProcessGameSnapShot()
    {
        // Validate frame ordering

        if (currentGameSnapshot.FrameNumber < currentReceivedFrameNumber)
        {
            return;
        }


        currentReceivedFrameNumber = currentGameSnapshot.FrameNumber;
        int frameDelta = currentRunningFrameNumber - currentReceivedFrameNumber;


        float dt = Time.fixedDeltaTime * (frameDelta + playerData.DelayRate);

        // Striker: platform-local -> estimate in local -> convert to world
        StrikerSnapshot s = currentGameSnapshot.Striker;

        Vector3 localStrikerPos = s.Position;
        Vector3 localStrikerVel = s.Velocity;
        Vector3 localStrikerEuler = s.Rotation;

        Vector3 estimatedLocalPos = GetEstimatedReflectedPoint(localStrikerPos, localStrikerVel, playerData.strikerK, dt, boardData.StrikerRadius);
        Vector3 estimatedLocalVel = localStrikerVel * Mathf.Exp(-playerData.strikerK * dt);

        if (!strikerData.isFoul)
        {

            StartCoroutine(LerpToTarget(strikerRigidbody, estimatedLocalPos, estimatedLocalVel, localStrikerEuler, playerData.DelayRate * Time.fixedDeltaTime));
        }
        else
        {

            strikerRigidbody.position = localStrikerPos;
            strikerRigidbody.transform.eulerAngles = localStrikerEuler;
        }

        // Coins: platform-local -> estimate in local -> convert to world for each
        coinRbs = coinData.AvailableCoinsInGame;
        for (int i = 0; i < coinRbs.Count; i++)
        {
            Rigidbody rb = coinRbs[i];
            if (rb == null) continue;

            CoinSnapshot c = currentGameSnapshot.Coins[i];

            Vector3 localCoinPos = c.Position;
            Vector3 localCoinVel = c.Velocity;
            Vector3 localCoinEuler = c.Rotation;

            Vector3 estimatedLocalCoinPos = GetEstimatedReflectedPoint(localCoinPos, localCoinVel, playerData.coinK, dt, boardData.CoinRadius);
            Vector3 estimatedLocalCoinVel = localCoinVel * Mathf.Exp(-playerData.coinK * dt);


            StartCoroutine(LerpToTarget(rb, estimatedLocalCoinPos, estimatedLocalCoinVel, localCoinEuler, playerData.DelayRate * Time.fixedDeltaTime));
        }
    }

    private IEnumerator LerpToTarget(Rigidbody rb, Vector3 targetPos, Vector3 targetVel, Vector3 targetRot, float duration)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 start = rb.position;
        Vector3 startRot = rb.transform.eulerAngles;
        float elapsed = 0;

        while (elapsed < duration)
        {
            rb.position = Vector3.Lerp(start, targetPos, elapsed / duration);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.position = targetPos;
        rb.transform.eulerAngles = targetRot;
        rb.linearVelocity = targetVel;
 

    }
    public Vector3 GetEstimatedReflectedPoint(Vector3 startPos, Vector3 velocity, float k, float dt, float radius)
    {
        Vector3 direction = velocity.normalized;
        float decayFactor = 1 - Mathf.Exp(-k * dt);
        float totalDistance = (velocity.magnitude / k) * decayFactor;

        // First raycast
        if (Physics.SphereCast(startPos, radius, direction, out RaycastHit hit1, totalDistance, hitMask))
        {
            if (hit1.collider.CompareTag("Edge"))
            {
                float distToHit1 = hit1.distance - radius;
                float remainingDist1 = totalDistance - distToHit1;

                Vector3 reflectedDir1 = Vector3.Reflect(direction, hit1.normal);

                // Second raycast from hit point in reflected direction
                if (Physics.SphereCast(hit1.point - direction * radius, boardData.StrikerRadius, reflectedDir1, out RaycastHit hit2, remainingDist1, hitMask))
                {
                    if (hit2.collider.CompareTag("Edge"))
                    {
                        float distToHit2 = hit2.distance - radius;
                        float remainingDist2 = remainingDist1 - distToHit2;

                        Vector3 reflectedDir2 = Vector3.Reflect(reflectedDir1, hit2.normal);
                        return hit2.point - reflectedDir1 * radius + reflectedDir2 * remainingDist2;
                    }
                    else
                    {
                        return hit1.point - direction * radius + reflectedDir1 * remainingDist1;
                    }
                }
                else
                {
                    return hit1.point - direction * radius + reflectedDir1 * remainingDist1;
                }
            }
        }

        // No bounce
        return startPos + direction * totalDistance;
    }

    private void Reset()
    {
        canIReceiveData = false;
        canISendData = false;
        canISendSnapShot = false;
        canIReceiveSnapShot = false;
        canISendStrikerData = false;

        currentRunningFrameNumber = 0;
        currentReceivedFrameNumber = 0;
        currentSentFrameNumber = 0;
    }

}
