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
    //public LineRenderer actualLineRenderer;
    //public LineRenderer estimatedLineRenderer;


    // Networked snapshots (platform-local space for positions/rotations/velocities)
    [Header("structures")]
    [Networked, OnChangedRender(nameof(OnPoolSnapShotReceived))][HideInInspector] public PoolGameSnapshot poolGameSnapshot { get; set; }
    [Networked, OnChangedRender(nameof(OnPlatformDataReceived))][HideInInspector] public PlatformSnapshot platforSnapshotData { get; set; }
    [Networked, OnChangedRender(nameof(OnSnookerSnapShotReceived))][HideInInspector] public SnookerGameSnapshot snookerGameSnapshot { get; set; }
    [Networked, OnChangedRender(nameof(OnStrikerSnapShotReceived))][HideInInspector] public StrikerSnapshot strikerSnapshot { get; set; }

    public int currentSentFrameNumber;
    public int currentReceivedFrameNumber;
    public int currentRunningFrameNumber;
    public int currentStrikerNumber;



    public bool canISendSnapShot;
    public bool canIReceiveSnapShot;
    public bool canIReceive;
    public bool canISend;
    public bool canISendStrikerData;


    private List<Rigidbody> coinRbs;
    private Rigidbody strikerRigidbody;
    public Rigidbody platformRb;
    public GameObject platform;
    public GameObject allAssets;

    private int previousTurnId = -1;

    private void OnEnable()
    {
        gameData.TurnChangeEvent += OnTurnChanged;

        playerInput.PlatformRotatedEvent += RotatePlatform;
        playerFoul.StrikerFoulPlacedEvent += HandleFoul;

        tableData.PlatformRotationChangedEvent += OnPlatformRotaionReceived;
    }

    private void OnDisable()
    {
        gameData.TurnChangeEvent -= OnTurnChanged;

        playerInput.PlatformRotatedEvent -= RotatePlatform;
        playerFoul.StrikerFoulPlacedEvent -= HandleFoul;

        tableData.PlatformRotationChangedEvent -= OnPlatformRotaionReceived;
    }

    private void OnPlatformRotaionReceived(Vector3 rotation)
    {
        if(HasStateAuthority)
        {
            PlatformSnapshot platformSnapshot = new PlatformSnapshot();
            platformSnapshot.Rotation = rotation;
            SetPlatformSnapShotData(platformSnapshot);
        }
    }

    private void HandleFoul(StrikerSnapshot snapshot)
    {
        if (HasStateAuthority)
        {
            SetStrikerSnapShotData(snapshot);
        }
    }

    private void OnTurnChanged(int id)
    {
        Reset();

        if (strikerRigidbody == null)
        {
            var strikerGo = GameObject.FindGameObjectWithTag("Striker");
            if (strikerGo != null)
            {
                strikerRigidbody = strikerGo.GetComponent<Rigidbody>();
            }
        }

        if (platform == null)
        {
            platform = GameObject.FindGameObjectWithTag("Platform");
            if (platform != null)
            {
                platformRb = platform.GetComponent<Rigidbody>();
            }
        }

        if(allAssets == null)
        {
            allAssets = GameObject.FindGameObjectWithTag("AllAssets");
        }

        if (player.playerProperties.myId == id && player.playerProperties.myPlayerControl == PlayerControl.Local)
        {
            canISendStrikerData = true;
        }
    }

    private void RotatePlatform()
    {
        PlatformSnapshot platformSnapshot = new PlatformSnapshot();
        platformSnapshot.Rotation = platform.transform.eulerAngles;
        SetPlatformSnapShotData(platformSnapshot);
    }

    public void SendData(int id)
    {
        if (player.playerProperties.myId == id && player.playerProperties.myPlayerControl == PlayerControl.Local)
        {

            canISend = true;
            canISendStrikerData = false;
            currentSentFrameNumber = 0;
            currentStrikerNumber = 0;
        }
    }

    public void ReceiveData()
    {
        if ( player.playerProperties.myPlayerControl == PlayerControl.Remote)
        {
            canIReceive = true;
            currentRunningFrameNumber = 0;
            currentReceivedFrameNumber = 0;
        }
    }   

    private void OnPlatformDataReceived()
    {

        if (canIReceive || HasStateAuthority)
        {
            return;
        }

        allAssets.transform.SetParent(platform.transform);
        Vector3 localAngles = platforSnapshotData.Rotation;
        Quaternion incomingLocalRot = Quaternion.Euler(localAngles);     
        platform.transform.rotation = incomingLocalRot;
        allAssets.transform.SetParent(null);

    }

    public void StrikeForceChanged(float force,Vector3 dir)
    {
        StrikerShooting strikerShooting = strikerRigidbody.GetComponent<StrikerShooting>();
        strikerShooting.SetStrikerData(force,dir);
    }

    public void PlayerStrikeStarted(float force, Vector3 direction)
    {
        StrikerShooting strikerShooting = strikerRigidbody.GetComponent<StrikerShooting>();
        strikerShooting.StopArrowChange();
        strikerShooting.TurnOffArrow();
        strikerShooting.SetForceAndDir(force, direction);

        ReceiveData();

    }
    public void PlayerStrikeForceStarted()
    {
        StrikerShooting strikerShooting = strikerRigidbody.GetComponent<StrikerShooting>();
        strikerShooting.StartArrowChange();

    }
    public void PlayerStrikeEnded()
    {
        Reset();

        if(strikerData.isFoul)
        {
            strikerData.PlaceStriker();
        }

       // StopAllCoroutines();

    }

    public void SetStrikerSnapShotData(StrikerSnapshot newStrikerSnapShot)
    {
        strikerSnapshot = newStrikerSnapShot;
    }

    public void SetPlatformSnapShotData(PlatformSnapshot newPlatformSnapshot)
    {
        platforSnapshotData = newPlatformSnapshot;
    }

    public void SetPoolGameSnapShotdata(PoolGameSnapshot newgameSnapshot)
    {
        poolGameSnapshot = newgameSnapshot;
       
    }
    public void SetSnookerGameSnapShotdata(SnookerGameSnapshot newgameSnapshot)
    {
        snookerGameSnapshot = newgameSnapshot;
    }

    private void OnPoolSnapShotReceived()
    {

        if (canIReceive && !HasStateAuthority)
        {

            canIReceiveSnapShot = true;
        }
    }
    private void OnSnookerSnapShotReceived()
    {

        if (canIReceive && !HasStateAuthority)
        {
            canIReceiveSnapShot = true;
        }
    }

    private void OnStrikerSnapShotReceived()
    {

        if (!canIReceive && !HasStateAuthority)
        {          
            
            strikerRigidbody.transform.eulerAngles = (Vector3)(strikerSnapshot.Rotation);
            strikerRigidbody.position = (Vector3)strikerSnapshot.Position;
        }
    }

    public void ReceiveCoinRotationData(float val) { }
    public void ReceiveAvatarData(NetworkAvatarData data) { }
    public void ReceiveAIData(string data) { }

    private void FixedUpdate()
    {
        // receiving
        if (canIReceive)
        {
            currentRunningFrameNumber++;
        }

        if(canISendStrikerData && HasStateAuthority)
        {
            currentStrikerNumber++;
            if(currentStrikerNumber % playerData.SendRate == 0)
            {
                StrikerSnapshot newStrikerSnapShot = new StrikerSnapshot
                {
                    Rotation = strikerRigidbody.transform.eulerAngles,
                    Position = strikerRigidbody.position
                };
                SetStrikerSnapShotData(newStrikerSnapShot);
            }
        }

        if (canIReceive && canIReceiveSnapShot)
        {
            if (uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Pool)
            {
                ProcessPoolGameSnapShot();
            }
            else
            {
                ProcessSnookerGameSnapShot();
            }
            canIReceiveSnapShot = false;
        }


        // sending
        if (canISend && HasStateAuthority)
        {
            currentSentFrameNumber++;
            if (currentSentFrameNumber % playerData.SendRate == 0)
            {
                if (uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Pool)
                {
                    SetPoolGameSnapShotdata(GetPoolGameSnapshot());
                }
                else
                {
                    SetSnookerGameSnapShotdata(GetSnookerGameSnapshot());
                }
            }

           
        }
    }

    private PoolGameSnapshot GetPoolGameSnapshot()
    {
        PoolGameSnapshot snapshot = new PoolGameSnapshot();


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

    private SnookerGameSnapshot GetSnookerGameSnapshot()
    {
        SnookerGameSnapshot snapshot = new SnookerGameSnapshot();


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

    public void ProcessPoolGameSnapShot()
    {
        // Validate frame ordering

        if (poolGameSnapshot.FrameNumber < currentReceivedFrameNumber)
        {
            return;
        }


        currentReceivedFrameNumber = poolGameSnapshot.FrameNumber;
        int frameDelta =   currentRunningFrameNumber - currentReceivedFrameNumber;


        float dt = Time.fixedDeltaTime * (frameDelta + playerData.DelayRate);

        // Striker: platform-local -> estimate in local -> convert to world
        StrikerSnapshot s = poolGameSnapshot.Striker;

        Vector3 localStrikerPos = s.Position;
        Vector3 localStrikerVel = s.Velocity;
        Vector3 localStrikerEuler = s.Rotation;

        Vector3 estimatedLocalPos = GetEstimatedReflectedPoint(localStrikerPos, localStrikerVel, playerData.strikerK, dt, boardData.StrikerRadius);
        Vector3 estimatedLocalVel = localStrikerVel * Mathf.Exp(-playerData.strikerK * dt);

        if (!strikerData.isFoul)
        {
            strikerRigidbody.isKinematic = true;
            StartCoroutine(LerpToTarget(strikerRigidbody, estimatedLocalPos, estimatedLocalVel, localStrikerEuler, playerData.DelayRate * Time.fixedDeltaTime));
        }

        // Coins: platform-local -> estimate in local -> convert to world for each
        coinRbs = coinData.AvailableCoinsInGame;
        for (int i = 0; i < coinRbs.Count; i++)
        {
            Rigidbody rb = coinRbs[i];
            if (rb == null) continue;

            CoinSnapshot c = poolGameSnapshot.Coins[i];

            Vector3 localCoinPos = c.Position;
            Vector3 localCoinVel = c.Velocity;
            Vector3 localCoinEuler = c.Rotation;

            Vector3 estimatedLocalCoinPos = GetEstimatedReflectedPoint(localCoinPos, localCoinVel, playerData.coinK, dt, boardData.CoinRadius);
            Vector3 estimatedLocalCoinVel = localCoinVel * Mathf.Exp(-playerData.coinK * dt);

            rb.isKinematic = true;
            StartCoroutine(LerpToTarget(rb, estimatedLocalCoinPos, estimatedLocalCoinVel, localCoinEuler, playerData.DelayRate * Time.fixedDeltaTime));
        }
    }
    public void ProcessSnookerGameSnapShot()
    {
        // Validate frame ordering

        if (snookerGameSnapshot.FrameNumber < currentReceivedFrameNumber)
        {
            return;
        }


        currentReceivedFrameNumber = snookerGameSnapshot.FrameNumber;
        int frameDelta =  currentRunningFrameNumber - currentReceivedFrameNumber;


        float dt = Time.fixedDeltaTime * (frameDelta + playerData.DelayRate);


        // Striker: platform-local -> estimate in local -> convert to world
        StrikerSnapshot s = snookerGameSnapshot.Striker;

        Vector3 localStrikerPos = s.Position;
        Vector3 localStrikerVel = s.Velocity;
        Vector3 localStrikerEuler = s.Rotation;

        Vector3 estimatedLocalPos = GetEstimatedReflectedPoint(localStrikerPos, localStrikerVel, playerData.strikerK, dt, boardData.StrikerRadius);
        Vector3 estimatedLocalVel = localStrikerVel * Mathf.Exp(-playerData.strikerK * dt);


        if (!strikerData.isFoul)
        {
            strikerRigidbody.isKinematic = true;
            StartCoroutine(LerpToTarget(strikerRigidbody, estimatedLocalPos, estimatedLocalVel, localStrikerEuler, playerData.DelayRate * Time.fixedDeltaTime));
        }

        // Coins: platform-local -> estimate in local -> convert to world for each
        coinRbs = coinData.AvailableCoinsInGame;
        for (int i = 0; i < coinRbs.Count; i++)
        {
            Rigidbody rb = coinRbs[i];

            CoinSnapshot c = snookerGameSnapshot.Coins[i];

            Vector3 localCoinPos = c.Position;
            Vector3 localCoinVel = c.Velocity;
            Vector3 localCoinEuler = c.Rotation;

            Vector3 estimatedLocalCoinPos = GetEstimatedReflectedPoint(localCoinPos, localCoinVel, playerData.coinK, dt, boardData.CoinRadius);
            Vector3 estimatedLocalCoinVel = localCoinVel * Mathf.Exp(-playerData.coinK * dt);


            rb.isKinematic = true;
            StartCoroutine(LerpToTarget(rb, estimatedLocalCoinPos, estimatedLocalCoinVel, localCoinEuler, playerData.DelayRate * Time.fixedDeltaTime));
        }
    }

    private IEnumerator LerpToTarget(Rigidbody rb, Vector3 targetPos, Vector3 targetVel, Vector3 targetRot, float duration)
    {
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
        rb.isKinematic = false;
        rb.linearVelocity = targetVel;
       
    }
    public Vector3 GetEstimatedReflectedPoint(Vector3 startPos, Vector3 velocity, float k, float dt, float radius)
    {
        Vector3 direction = velocity.normalized;
        float decayFactor = 1 - Mathf.Exp(-k * dt);
        float totalDistance = (velocity.magnitude / k) * decayFactor;

        // First raycast
        if (Physics.SphereCast(startPos, radius, direction, out RaycastHit hit1, totalDistance,hitMask))
        {
            if (hit1.collider.CompareTag("Edge"))
            {
                float distToHit1 = hit1.distance - radius;
                float remainingDist1 = totalDistance - distToHit1;

                Vector3 reflectedDir1 = Vector3.Reflect(direction, hit1.normal);

                // Second raycast from hit point in reflected direction
                if (Physics.SphereCast(hit1.point - direction * radius, boardData.StrikerRadius, reflectedDir1, out RaycastHit hit2, remainingDist1,hitMask))
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
        canIReceive = false;
        canISend = false;
        canISendSnapShot = false;
        canIReceiveSnapShot = false;

        currentRunningFrameNumber = 0;
        currentReceivedFrameNumber = 0;
        currentSentFrameNumber = 0;
    }

}
