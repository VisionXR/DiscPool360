using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoulPlacementTest : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PlayerDataSO playersData;
    public StrikerDataSO strikerData;
    public BoardDataSO boardData;

    [Header("Local Objects")]
    public SnookerLogic snookerLogic;
    public List<GameObject> pocketedCoins;

    [Header("Key Bindings (New Input System)")]
    public Key PlaceStrikerKey = Key.F;
    public Key PlaceCoinKey = Key.C;
    public LayerMask placementLayerMask; // Layer mask to specify which layers to check for placement

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null)
            return;

        // Striker rotation (continuous)
        if (kb[PlaceStrikerKey].wasPressedThisFrame)
        {
            Player mp = playersData.GetMainPlayer();
            if (mp != null)
            {
                PlayerFoul playerFoul = mp.GetComponent<PlayerFoul>();
                if (playerFoul != null)
                {
                    Debug.Log("Placing striker on board...");
                    playerFoul.PlaceStrikerOnBoard();
                    // CanPlaceAt(strikerData.currentStriker.transform.position, boardData.StrikerRadius*2);
                }

            }
        }

        // Striker rotation (continuous)
        if (kb[PlaceCoinKey].wasPressedThisFrame)
        {
            Debug.Log("Respotting colors...");
            snookerLogic.RespotColors(pocketedCoins);
        }
    }

    public void CanPlaceAt(Vector3 targetPosition, float strikerRadius)
    {
        // Check for overlapping colliders at the target position (ignore board and striker)
        Collider[] overlaps = Physics.OverlapSphere(targetPosition, strikerRadius,placementLayerMask);

        
        Debug.Log("Checking placement at " + targetPosition + " with radius " + strikerRadius + ". Overlaps found: " + overlaps.Length);
        for (int i = 0; i < overlaps.Length; i++)
        {
            var col = overlaps[i];
            if (col == null) continue;

            Debug.Log("Cannot place at " + targetPosition + " due to collider: " + col.name);
        }
      
    }
}
