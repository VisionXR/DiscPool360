using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnookerPhase { RedsPhase, ColorsPhase }
public enum ShotExpectation { ExpectRed, ExpectColor }
public enum SnookerShotResult { ContinueTurn, ChangeTurn, Win, Foul }

public class SnookerLogic : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public CoinDataSO coinData;
    public BoardDataSO boardData;
    public GameDataSO gameData;

    [Header("State Objects")]
    public LayerMask placementLayerMask; // Layer mask to specify which layers to check for placement   
    public float glowDuration = 2f;
    public SnookerPhase phase = SnookerPhase.RedsPhase;
    public ShotExpectation expectation = ShotExpectation.ExpectRed;
    public int requiredColorIndex = 0; // 0 = Red, 1..N = colors in order


    public void StartGame()
    {
        phase = SnookerPhase.RedsPhase;
        expectation = ShotExpectation.ExpectRed;
        requiredColorIndex = 0;
        gameData.ResetSnookerScore();
    }

    // - While reds remain: stay in RedsPhase and flip expectation Red <-> Color
    // - After reds: move to ColorsPhase; do not flip; specific color is required via requiredColorIndex
    public void ChangePhase()
    {
        if (RedsRemaining())
        {
            phase = SnookerPhase.RedsPhase;
            expectation = (expectation == ShotExpectation.ExpectRed)
                ? ShotExpectation.ExpectColor
                : ShotExpectation.ExpectRed;
            return;
        }

        phase = SnookerPhase.ColorsPhase;
        if (requiredColorIndex < 1) requiredColorIndex = 1; // start at Yellow
        if (coinData.colors != null && requiredColorIndex > coinData.colors.Count)
            requiredColorIndex = coinData.colors.Count;
    }

    public void SetPhase()
    {
        if (RedsRemaining())
        {
            phase = SnookerPhase.RedsPhase;
            expectation = ShotExpectation.ExpectRed;
            return;
        }

        phase = SnookerPhase.ColorsPhase;
        if (requiredColorIndex < 1) requiredColorIndex = 1;
        if (coinData.colors != null && requiredColorIndex > coinData.colors.Count)
            requiredColorIndex = coinData.colors.Count;
    }

    // Helper for SinglePlayerManager to assign expected PlayerCoin to current player
    public PlayerCoin GetExpectedPlayerCoin()
    {
        if (phase == SnookerPhase.RedsPhase)
        {
            return expectation == ShotExpectation.ExpectRed ? PlayerCoin.Red : PlayerCoin.Color;
        }

        if (requiredColorIndex <= 0)
            return PlayerCoin.Red;

        int colorListIndex = requiredColorIndex - 1;
        if (coinData.colors == null || colorListIndex < 0 || colorListIndex >= coinData.colors.Count)
            return PlayerCoin.Red;

        var go = coinData.colors[colorListIndex];
        var id = go != null ? go.GetComponent<CoinId>() : null;
        if (id == null) return PlayerCoin.Red;

        switch (id.coinType)
        {
            case CoinType.Yellow: return PlayerCoin.Yellow;
            case CoinType.Green:  return PlayerCoin.Green;
            case CoinType.Brown:  return PlayerCoin.Brown;
            case CoinType.Blue:   return PlayerCoin.Blue;
            case CoinType.Pink:   return PlayerCoin.Pink;
            case CoinType.Black:  return PlayerCoin.Black;
            default:              return PlayerCoin.Red;
        }
    }

    public bool RedsRemaining()
    {
        var reds = coinData.reds;
        if (reds == null) return false;
        for (int i = 0; i < reds.Count; i++)
        {
            var c = reds[i];
            if (c != null && c.activeInHierarchy) return true;
        }
        return false;
    }

    // Validate a shot and update internal state.
    public SnookerShotResult ValidateShot(List<GameObject> pocketedCoins, bool isFoul)
    {
        if (isFoul)
        {
            RespotColors(pocketedCoins);
            return SnookerShotResult.Foul;
        }

        if (pocketedCoins == null || pocketedCoins.Count == 0)
        {
            return SnookerShotResult.ChangeTurn;
        }

        if (phase == SnookerPhase.RedsPhase)
        {
           
            return ValidateRedsPhaseShot(pocketedCoins);
        }
        else
        {
            return ValidateColorsPhaseShot(pocketedCoins);
        }
    }

    private SnookerShotResult ValidateRedsPhaseShot(List<GameObject> pocketed)
    {
        bool pottedRed = ContainsTag(pocketed, "Red");
        bool pottedAnyColor = ContainsTag(pocketed, "Color");

        if (expectation == ShotExpectation.ExpectRed)
        {
            if (pottedRed)
            {
                // Score: 1 point per red potted
                int redCount = 0;
                for (int i = 0; i < pocketed.Count; i++)
                {
                    var go = pocketed[i];
                    if (go != null && go.CompareTag("Red")) redCount++;
                }
                if (redCount > 0) AddScoreToCurrent(redCount); // 1 per red

                // Any color potted in same stroke is respotted (no score for colors here)
                if (pottedAnyColor) RespotColors(pocketed);

                return SnookerShotResult.ContinueTurn;
            }
            else
            {
                RespotColors(pocketed);
                return SnookerShotResult.ChangeTurn;
            }
        }
        else // ExpectColor
        {
            if (pottedAnyColor)
            {
                // Score sum of all colors potted this stroke (they will be respotted)
                int colorPoints = 0;
                for (int i = 0; i < pocketed.Count; i++)
                {
                    var go = pocketed[i];
                    if (IsColor(go)) colorPoints += GetSnookerPoints(go);
                }
                if (colorPoints > 0) AddScoreToCurrent(colorPoints);

                RespotColors(pocketed);
                return SnookerShotResult.ContinueTurn;
            }
            else
            {
                return SnookerShotResult.ChangeTurn;
            }
        }
    }

    // Colors phase: requiredColorIndex 0 => Red; 1..N => colors in coinData.colors order
    private SnookerShotResult ValidateColorsPhaseShot(List<GameObject> pocketed)
    {
        if (coinData.colors == null) return SnookerShotResult.ChangeTurn;

        if (requiredColorIndex < 0) requiredColorIndex = 0;
        if (requiredColorIndex > coinData.colors.Count) requiredColorIndex = coinData.colors.Count;

        bool pottedRequired = false;
        bool pottedOtherColor = false;

        if (requiredColorIndex == 0)
        {
            pottedRequired = ContainsTag(pocketed, "Red");
            pottedOtherColor = ContainsAnyColorExcept(pocketed, except: null);
        }
        else
        {
            int colorIndex = requiredColorIndex - 1;
            if (colorIndex < 0 || colorIndex >= coinData.colors.Count) return SnookerShotResult.ChangeTurn;

            GameObject required = coinData.colors[colorIndex];
            if (required == null) return SnookerShotResult.ChangeTurn;

            pottedRequired = pocketed.Contains(required);
            pottedOtherColor = ContainsAnyColorExcept(pocketed, required);
            bool pottedRed = ContainsTag(pocketed, "Red");
            pottedOtherColor = pottedOtherColor || pottedRed;

            if (pottedRequired && !pottedOtherColor)
            {
                // Score specific color points
                AddScoreToCurrent(GetSnookerPoints(required));
            }
        }

        if (pottedRequired && !pottedOtherColor)
        {
            requiredColorIndex++;
            if (requiredColorIndex > coinData.colors.Count)
            {
                // All colors cleared: decide winner by score and set it
                int p1Score = gameData.GetSnookerScore(1);
                int p2Score = gameData.GetSnookerScore(2);
                int winnerId = p1Score >= p2Score ? 1 : 2;
                gameData.SetSnookerWinner(winnerId);
                return SnookerShotResult.Win;
            }
            return SnookerShotResult.ContinueTurn;
        }

        RespotColors(pocketed);
        return SnookerShotResult.ChangeTurn;
    }

    private int GetSnookerPoints(GameObject go)
    {
        if (go == null) return 0;
        if (go.CompareTag("Red")) return 1;

        var id = go.GetComponent<CoinId>();
        if (id == null) return 0;

        switch (id.coinType)
        {
            case CoinType.Yellow: return 2;
            case CoinType.Green:  return 3;
            case CoinType.Brown:  return 4;
            case CoinType.Blue:   return 5;
            case CoinType.Pink:   return 6;
            case CoinType.Black:  return 7;
            default:              return 0;
        }
    }

    private void AddScoreToCurrent(int points)
    {
        if (points <= 0) return;
        // Assumes GameDataSO exposes AddSnookerScore(int playerId, int points)
        gameData.AddSnookerScore(gameData.currentTurnId, points);
    }

    private bool ContainsTag(List<GameObject> list, string tag)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var go = list[i];
            if (go != null && go.CompareTag(tag)) return true;
        }
        return false;
    }

    private bool IsColor(GameObject go)
    {
        if (go == null) return false;
        if (coinData.colors != null && coinData.colors.Contains(go)) return true;

        var id = go.GetComponent<CoinId>();
        return id != null && id.coinType != CoinType.Red;
    }

    private bool ContainsAnyColorExcept(List<GameObject> list, GameObject except)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var go = list[i];
            if (go == null) continue;
            if (IsColor(go) && go != except) return true;
        }
        return false;
    }

    public void RespotColors(List<GameObject> pocketed)
    {
        // Placement parameters
        float boardLift = 0.01f; // vertical lift to avoid z-fighting with board
        int steps = 12; // number of angular samples per radius
        int maxRadiusSteps = 2; // how many radius increments to try
        float radiusStepMultiplier = 1.2f; // each radius = coinRadius * multiplier^step

        for (int i = 0; i < pocketed.Count; i++)
        {
            var go = pocketed[i];
            if (go == null) continue;
            if (!IsColor(go)) continue;

            var coinId = go.GetComponent<CoinId>();
            if (coinId == null) continue;

            Transform spawn = boardData.SnookerCoinPositions[((int)coinId.coinType)-1];
            if (spawn == null) continue;

            var rb = go.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
              
            }

            Vector3 basePos = spawn.position;
            float coinRadius = boardData.CoinRadius > 0 ? boardData.CoinRadius : 0.1f;

            // Try base position first
            Vector3 candidate = basePos + Vector3.up * boardLift;
            bool placed = false;
            if (CanPlaceAt(candidate, coinRadius))
            {
                go.transform.position = candidate;
                placed = true;
            }
            else
            {
                // Search along concentric circles until a free spot is found
                for (int r = 1; r <= maxRadiusSteps && !placed; r++)
                {
                    float radius = coinRadius * Mathf.Pow(radiusStepMultiplier, r);
                    for (int s = 0; s < steps; s++)
                    {
                        float angle = s * Mathf.PI * 2f / steps;
                        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
                        candidate = basePos + offset + Vector3.up * boardLift;
                        if (CanPlaceAt(candidate, coinRadius))
                        {
                            go.transform.position = candidate;
                            placed = true;
                            break;
                        }
                    }
                }
            }

            // Fallback to base spawn if no free spot found
            if (!placed)
            {
                go.transform.position = basePos + Vector3.up * boardLift;
            }

            go.transform.rotation = spawn.rotation;
            rb.isKinematic = false;
            go.SetActive(true);
        }
    }

    public bool CanPlaceAt(Vector3 targetPosition, float coinRadius)
    {
        // Check for overlapping colliders at the target position (ignore board and striker)
        Collider[] overlaps = Physics.OverlapSphere(targetPosition, coinRadius * 1.1f, placementLayerMask);
        for (int i = 0; i < overlaps.Length; i++)
        {
            var col = overlaps[i];
            if (col == null) continue;

            // Any other collider blocks placement
            return false;
        }

        return true;
    }


    public void GlowCoins(PlayerCoin coin)
    {
        StartCoroutine(GlowRoutine(coin));
    }

    private IEnumerator GlowRoutine(PlayerCoin coin)
    {
        // Helper to enable/disable glow on a list safely
        void EnableGlowOn(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var go = list[i];
                if (go != null && go.activeInHierarchy)
                {
                    var glow = go.GetComponent<CoinGlow>();
                    if (glow != null) glow.EnableGlow();
                }
            }
        }

        void DisableGlowOn(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var go = list[i];
                if (go != null && go.activeInHierarchy)
                {
                    var glow = go.GetComponent<CoinGlow>();
                    if (glow != null) glow.DisableGlow();
                }
            }
        }

        // Reds: glow all active reds
        if (coin == PlayerCoin.Red)
        {
            var activeReds = coinData.reds;
            if (activeReds != null)
            {
                EnableGlowOn(activeReds);
                yield return new WaitForSeconds(glowDuration);
                DisableGlowOn(activeReds);
            }
            yield break;
        }

        // Any Color: glow all active colors
        if (coin == PlayerCoin.Color)
        {
            var activeColors = coinData.colors;
            if (activeColors != null)
            {
                EnableGlowOn(activeColors);
                yield return new WaitForSeconds(glowDuration);
                DisableGlowOn(activeColors);
            }
            yield break;
        }

        // Specific color: filter from colors list by CoinId.coinType and glow only that color
        List<GameObject> targets = new List<GameObject>();
        if (coinData.colors != null)
        {
            CoinType targetType = CoinType.Yellow;
            switch (coin)
            {
                case PlayerCoin.Yellow: targetType = CoinType.Yellow; break;
                case PlayerCoin.Green:  targetType = CoinType.Green;  break;
                case PlayerCoin.Brown:  targetType = CoinType.Brown;  break;
                case PlayerCoin.Blue:   targetType = CoinType.Blue;   break;
                case PlayerCoin.Pink:   targetType = CoinType.Pink;   break;
                case PlayerCoin.Black:  targetType = CoinType.Black;  break;
                default: yield break;
            }

            for (int i = 0; i < coinData.colors.Count; i++)
            {
                var go = coinData.colors[i];
                if (go == null || !go.activeInHierarchy) continue;
                var id = go.GetComponent<CoinId>();
                if (id != null && id.coinType == targetType)
                {
                    targets.Add(go);
                }
            }
        }

        if (targets.Count > 0)
        {
            // Glow targets and dim non-target colors for clarity (optional)
            // Enable glow on targets
            for (int i = 0; i < targets.Count; i++)
            {
                var glow = targets[i].GetComponent<CoinGlow>();
                if (glow != null) glow.EnableGlow();
            }

            yield return new WaitForSeconds(glowDuration);

            // Disable glow on targets
            for (int i = 0; i < targets.Count; i++)
            {
                var glow = targets[i].GetComponent<CoinGlow>();
                if (glow != null) glow.DisableGlow();
            }
        }
    }
}
