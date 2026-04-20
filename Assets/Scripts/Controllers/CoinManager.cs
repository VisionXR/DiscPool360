using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;
    public CoinDataSO coinData;

    [Header("Game Objects")]
    public GameObject AllAssets;
    public GameObject poolCoinsPrefab;
    public GameObject snookerCoinsPrefab;

    // Local variables
    public float placeRadius = 0.1f;
    public float coinLift = 0.01f;
    public LayerMask coinLayerMask;
    private GameObject currentCoins;

    private void OnEnable()
    {
        coinData.CreateCoinsEvent += CreateCoins;
        coinData.DestroyCoinsEvent += DestroyCoins;

        coinData.CoinFellOnGroundEvent += PlaceOnBoard;
    }

    private void OnDisable()
    {
        coinData.CreateCoinsEvent -= CreateCoins;
        coinData.DestroyCoinsEvent -= DestroyCoins;

        coinData.CoinFellOnGroundEvent -= PlaceOnBoard;
    }

    private IEnumerator WaitAndPlace(GameObject coin)
    {
        yield return new WaitForSeconds(1);
        GameObject board = boardData.Board;
        float coinRadius = boardData.CoinRadius;

        int steps = 16;

        if (board != null)
        {
            Vector3 boardPosition = board.transform.position;
            bool placed = false;

            for (int i = 0; i < steps; i++)
            {
                float angle = i * Mathf.PI * 2f / steps;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * placeRadius;
                Vector3 candidatePos = boardPosition + offset + Vector3.up * (coinLift);

                if (CanPlaceAt(candidatePos, coinRadius))
                {
                    coin.transform.position = candidatePos;
                    placed = true;
                    break;
                }
            }

            if (!placed)
            {
                coin.transform.position = boardPosition + Vector3.up * (coinLift);
            }
        }

    }

    private void PlaceOnBoard(GameObject coin)
    {
        Rigidbody rb = coin.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        StartCoroutine(WaitAndPlace(coin));
    }

    private void CreateCoins(GameMode mode, Transform coinTransform)
    {
        // 1. Destroy existing coins if they exist
        if (currentCoins != null)
        {
            Destroy(currentCoins);
            // Clean up memory from the previous coin set
            Resources.UnloadUnusedAssets();
        }

        // 2. Construct the path based on the Enum name 
        // This assumes your files are named "Coins/Pool.prefab" and "Coins/Snooker.prefab"
        string resourcePath = "Coins/";
        
        if(mode == GameMode.Pool)
            resourcePath += "PoolCoins";
        else if(mode == GameMode.Snooker)
            resourcePath += "SnookerCoins";
        else
            Debug.LogError($"[CreateCoins] Unsupported GameMode: {mode}");
    

        // 3. Load from Resources
        GameObject coinPrefab = Resources.Load<GameObject>(resourcePath);

        if (coinPrefab != null)
        {
            // 4. Instantiate and set properties
            currentCoins = Instantiate(coinPrefab, coinTransform.position, coinTransform.rotation);

            currentCoins.transform.localScale = Vector3.one * 0.5f;
            currentCoins.transform.SetParent(AllAssets.transform);
        }
        else
        {
            Debug.LogError($"[CreateCoins] Could not find prefab at Resources/{resourcePath}");
        }
    }

    private void DestroyCoins()
    {
        if(currentCoins != null)
        {
            Destroy(currentCoins);
            coinData.AvailableCoinsInGame.Clear();
        }
    }

    public bool CanPlaceAt(Vector3 targetPosition, float coinRadius)
    {
        // Check for overlapping colliders at the target position (ignore board and striker)
        Collider[] overlaps = Physics.OverlapSphere(targetPosition, coinRadius * 1.1f, coinLayerMask);
        for (int i = 0; i < overlaps.Length; i++)
        {
            var col = overlaps[i];
            if (col == null) continue;

            // Any other collider blocks placement
            return false;
        }

        return true;
    }
}
