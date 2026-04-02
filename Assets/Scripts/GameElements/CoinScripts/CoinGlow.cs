using UnityEngine;

public class CoinGlow : MonoBehaviour
{
    public GameObject glowObject;

    public void EnableGlow()
    {
        glowObject.SetActive(true);
    }

    public void DisableGlow()
    {
        glowObject.SetActive(false);
    }
}
