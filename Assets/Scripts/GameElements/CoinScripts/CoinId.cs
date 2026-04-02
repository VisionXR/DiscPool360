using System;
using UnityEngine;

public class CoinId : MonoBehaviour
{
    public CoinType coinType;
}

[Serializable]
public enum CoinType
{
   Red,
   Yellow,
   Green,
   Brown,
    Blue,
    Pink,
    Black

}
