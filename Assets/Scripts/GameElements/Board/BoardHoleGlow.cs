using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHoleGlow : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public CoinDataSO coinData;

    [Header("Hole Glow Settings")]
    public List<GameObject> holeGlowObjects;
    [SerializeField] private float glowDuration = 1f;

    // Dictionary to track independent coroutines for each glow object
    private Dictionary<GameObject, Coroutine> activeGlows = new Dictionary<GameObject, Coroutine>();

    private void OnEnable()
    {
        if (coinData != null)
            coinData.CoinPocketedIntoHoleEvent += SetGlow;
    }

    private void OnDisable()
    {
        if (coinData != null)
            coinData.CoinPocketedIntoHoleEvent -= SetGlow;

        // Clean up all active routines on disable
        StopAllCoroutines();
        activeGlows.Clear();
    }

    private void SetGlow(GameObject hole)
    {
        GameObject glow = GetGlowForHole(hole.name);

        if (glow != null)
        {
            // If this specific hole is already mid-glow, stop the old timer
            if (activeGlows.ContainsKey(glow) && activeGlows[glow] != null)
            {
                StopCoroutine(activeGlows[glow]);
            }

            // Start a new independent timer for this hole
            activeGlows[glow] = StartCoroutine(FlashGlow(glow, glowDuration));
        }
    }

    private IEnumerator FlashGlow(GameObject glowObj, float duration)
    {
        glowObj.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (glowObj != null)
        {
            glowObj.SetActive(false);
        }

        // Remove from tracking once finished
        if (activeGlows.ContainsKey(glowObj))
        {
            activeGlows.Remove(glowObj);
        }
    }

    private GameObject GetGlowForHole(string holeName)
    {
        if (string.IsNullOrEmpty(holeName) || holeGlowObjects == null || holeGlowObjects.Count == 0)
        {
            return null;
        }

        // Optimization: Try to parse trailing digit (Hole1, Hole2, etc.)
        if (holeName.Length >= 5 && holeName.StartsWith("Hole"))
        {
            char lastChar = holeName[holeName.Length - 1];
            if (char.IsDigit(lastChar))
            {
                int num = lastChar - '0';
                int index = num - 1;
                if (index >= 0 && index < holeGlowObjects.Count)
                {
                    return holeGlowObjects[index];
                }
            }
        }

        // Fallback explicit mapping if naming convention varies
        switch (holeName)
        {
            case "Hole1": return holeGlowObjects.Count > 0 ? holeGlowObjects[0] : null;
            case "Hole2": return holeGlowObjects.Count > 1 ? holeGlowObjects[1] : null;
            case "Hole3": return holeGlowObjects.Count > 2 ? holeGlowObjects[2] : null;
            case "Hole4": return holeGlowObjects.Count > 3 ? holeGlowObjects[3] : null;
            case "Hole5": return holeGlowObjects.Count > 4 ? holeGlowObjects[4] : null;
            case "Hole6": return holeGlowObjects.Count > 5 ? holeGlowObjects[5] : null;
            default: return null;
        }
    }

    // Call this only if you need to clear the board visually (e.g., game reset)
    public void ResetAllGlowsManually()
    {
        StopAllCoroutines();
        activeGlows.Clear();
        foreach (var glow in holeGlowObjects)
        {
            if (glow != null) glow.SetActive(false);
        }
    }
}