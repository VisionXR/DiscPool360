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
    public UserDataSO userData;

    [Header("Game Objects")]
    public GameObject AllAssets;
    public string resourceFolderPath = "Coins/";

    // Local variables
    public AudioSource coinFellIntoHole;
    public float placeRadius = 0.1f;
    public float coinLift = 0.01f;
    public LayerMask coinLayerMask;
    private GameObject currentCoins;

    private void OnEnable()
    {
        coinData.CreateCoinsEvent += CreateCoins;
        coinData.DestroyCoinsEvent += DestroyCoins;

        coinData.CoinFellOnGroundEvent += PlaceOnBoard;

        coinData.CoinPocketedIntoHoleEvent += PlayHoleAudio;
    }

    private void OnDisable()
    {
        coinData.CreateCoinsEvent -= CreateCoins;
        coinData.DestroyCoinsEvent -= DestroyCoins;

        coinData.CoinFellOnGroundEvent -= PlaceOnBoard;

        coinData.CoinPocketedIntoHoleEvent -= PlayHoleAudio;
    }

    private void PlayHoleAudio(GameObject coin)
    {
        coinFellIntoHole.Play();
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

        if (currentCoins != null)
        {
            Destroy(currentCoins);

        }

        string path = resourceFolderPath + "Coins" + userData.myCoins;

        Debug.Log(path);
        GameObject coinsPrefab = Resources.Load<GameObject>(path);

        currentCoins = Instantiate(coinsPrefab, coinTransform.transform.position, coinTransform.transform.rotation);
        currentCoins.transform.localScale = Vector3.one * 0.5f;
        currentCoins.transform.SetParent(AllAssets.transform);


    }

    private void DestroyCoins()
    {
        if (currentCoins != null)
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
