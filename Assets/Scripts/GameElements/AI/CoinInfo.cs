using System;
using UnityEngine;

[Serializable]
public class CoinInfo
{
    public Vector3 strikerPosition;
    public Vector3 finalPosition;
    public Vector3 finalBoardDirection;


    public GameObject Coin;
    public GameObject Hole;

    public float angle;
    public float distance;

    public bool isBlockedH = false;
    public bool isBlockedC = false;

    public GameObject blockedCoinAlongStriker;
    public GameObject blockedCoinAlongHole;
}
