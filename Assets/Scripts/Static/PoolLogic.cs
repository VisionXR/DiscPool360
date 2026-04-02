using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLogic : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public CoinDataSO coinData;

    public float glowDuration = 2f;

    public bool AreAnyCoinsRemaining(PlayerCoin myCoin)
    {
        if (myCoin == PlayerCoin.Stripe)
        {
            for (int i = 0; i < coinData.stripes.Count; i++)
            {
                var c = coinData.stripes[i];
                if (c != null && c.activeInHierarchy) return true;
            }
        }
        else if (myCoin == PlayerCoin.Solid)
        {
            for (int i = 0; i < coinData.solids.Count; i++)
            {
                var c = coinData.solids[i];
                if (c != null && c.activeInHierarchy) return true;
            }
        }
        else if (myCoin == PlayerCoin.Black)
        {
            var c = coinData.black;
            if (c != null && c.activeInHierarchy) return true;
        }
        else if(myCoin == PlayerCoin.AllPool)
        {
            for (int i = 0; i < coinData.stripes.Count; i++)
            {
                var c = coinData.stripes[i];
                if (c != null && c.activeInHierarchy) return true;
            }
            for (int i = 0; i < coinData.solids.Count; i++)
            {
                var c = coinData.solids[i];
                if (c != null && c.activeInHierarchy) return true;
            }
            var black = coinData.black;
            if (black != null && black.activeInHierarchy) return true;
        }

            return false;
    }

    public bool HasPocketedMyCoin(List<GameObject> pocketedCoins, PlayerCoin myCoin)
    {
        string myTag = myCoin == PlayerCoin.Stripe ? "Stripe" : "Solid";
        for (int i = 0; i < pocketedCoins.Count; i++)
        {
            var go = pocketedCoins[i];
            if (go != null && go.CompareTag(myTag)) return true;
        }
        return false;
    }

    public bool HasPocketedBlack(List<GameObject> pocketedCoins)
    {
        for (int i = 0; i < pocketedCoins.Count; i++)
        {
            var go = pocketedCoins[i];
            if (go == null) continue;
            if (go == coinData.black) return true;
            if (go.CompareTag("Black")) return true;
        }
        return false;
    }

    // Glow logic (similar to Snooker): briefly highlight target coins
    public void GlowCoins(PlayerCoin coin)
    {
        StartCoroutine(GlowRoutine(coin));
    }

    private IEnumerator GlowRoutine(PlayerCoin coin)
    {
        // Helper local functions
        void EnableGlowOn(List<GameObject> list)
        {
            if (list == null) return;
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
            if (list == null) return;
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

        // Stripes
        if (coin == PlayerCoin.Stripe)
        {
            EnableGlowOn(coinData.stripes);
            yield return new WaitForSeconds(glowDuration);
            DisableGlowOn(coinData.stripes);
            yield break;
        }

        // Solids
        if (coin == PlayerCoin.Solid)
        {
            EnableGlowOn(coinData.solids);
            yield return new WaitForSeconds(glowDuration);
            DisableGlowOn(coinData.solids);
            yield break;
        }

        // Black
        if (coin == PlayerCoin.Black)
        {
            var black = coinData.black;
            if (black != null && black.activeInHierarchy)
            {
                var glow = black.GetComponent<CoinGlow>();
                if (glow != null)
                {
                    glow.EnableGlow();
                    yield return new WaitForSeconds(glowDuration);
                    glow.DisableGlow();
                }
            }
            yield break;
        }
    }
}