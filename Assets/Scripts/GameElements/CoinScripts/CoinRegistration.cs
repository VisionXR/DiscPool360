using com.VisionXR.ModelClasses;
using UnityEngine;

public class CoinRegistration : MonoBehaviour
{
    public CoinDataSO coinData;
    private void Start()
    {
        coinData.RegisterCoin(gameObject);

    }
    private void OnDestroy()
    {
        coinData.UnregisterCoin(gameObject);
    }
}
