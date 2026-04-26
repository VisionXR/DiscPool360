using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class CoinSorter : MonoBehaviour
{
    public static CoinSorter instance;

    [Header(" Scriptable Objects")]
    public CoinDataSO coinData;
    public BoardDataSO boardData;
    public AIDataSO aiData;
    public UIDataSO uiData;

    [Header(" Local Objects")]
    public List<CoinInfo> coinInfoList = new List<CoinInfo>();

    private List<GameObject> stripes;
    private List<GameObject> solids;
    private GameObject black;

    private List<GameObject> reds;
    private List<GameObject> colors;

    private int id;


    private void Awake()
    {
        instance = this;
    }
    public void SortAllPoolCoins(int id,PlayerCoin myCoin,List<GameObject> holes, Vector3 strikerPosition)
    {
          
        coinInfoList.Clear();
        this.id = id;
        StartCoroutine(SortPool(myCoin,holes,strikerPosition));
      
    }

    public void SortAllSnookerCoins(int id, PlayerCoin myCoin, List<GameObject> holes, Vector3 strikerPosition)
    {

        coinInfoList.Clear();
        this.id = id;

        StartCoroutine(SortSnooker(myCoin, holes, strikerPosition));

    }

    private IEnumerator SortPool(PlayerCoin myCoin, List<GameObject> holes, Vector3 strikerPosition)
    {

       
            solids = coinData.solids;
            stripes = coinData.stripes;
            black = coinData.black;


            if (myCoin == PlayerCoin.Stripe)
            {
                foreach (GameObject coin in stripes)
                {
                    if (coin.activeInHierarchy)
                    {
                        foreach (GameObject hole in holes)
                        {

                            GetCoinInfo(coin, hole, strikerPosition);
                           
                        }
                    }
                yield return new WaitForSeconds(0.01f);

            }

            }
            else if (myCoin == PlayerCoin.Solid)
            {
                foreach (GameObject coin in solids)
                {
                    if (coin.activeInHierarchy)
                    {
                        foreach (GameObject hole in holes)
                        {

                            GetCoinInfo(coin, hole, strikerPosition);
                            
                        }
                    }
                yield return new WaitForSeconds(0.01f);

            }

            }
            else if (myCoin == PlayerCoin.Black)
            {
                if (black != null)
                {
                    foreach (GameObject hole in holes)
                    {
                        GetCoinInfo(black, hole, strikerPosition);
                       
                    }
                yield return new WaitForSeconds(0.01f);
            }

            }
            else if (myCoin == PlayerCoin.AllPool)
            {
                foreach (GameObject coin in stripes)
                {
                    if (coin.activeInHierarchy)
                    {
                        foreach (GameObject hole in holes)
                        {

                            GetCoinInfo(coin, hole, strikerPosition);
                            
                        }
                    }
                yield return new WaitForSeconds(0.01f);

            }
                foreach (GameObject coin in solids)
                {
                    if (coin.activeInHierarchy)
                    {
                        foreach (GameObject hole in holes)
                        {

                            GetCoinInfo(coin, hole, strikerPosition);
                           
                        }
                    }
                yield return new WaitForSeconds(0.01f);
            }

            }


        if (coinInfoList.Count > 1)
        {
            SortCoinsByBlockedParameter();
            yield return new WaitForSeconds(0.02f);
            SortCoinsByAngleParameter();
            yield return new WaitForSeconds(0.02f);
        }

           aiData.CoinInformationReceived(id, coinInfoList);
    }

    private IEnumerator SortSnooker(PlayerCoin myCoin, List<GameObject> holes, Vector3 strikerPosition)
    {


        reds = coinData.reds;
        colors = coinData.colors;



        if (myCoin == PlayerCoin.Red)
        {
            foreach (GameObject coin in reds)
            {
                if (coin.activeInHierarchy)
                {
                    foreach (GameObject hole in holes)
                    {

                        GetCoinInfo(coin, hole, strikerPosition);
                        
                    }
                }
                yield return new WaitForSeconds(0.01f);
            }

        }
        else if (myCoin == PlayerCoin.Color)
        {
            foreach (GameObject coin in colors)
            {
                if (coin.activeInHierarchy)
                {
                    foreach (GameObject hole in holes)
                    {

                        GetCoinInfo(coin, hole, strikerPosition);
                        
                    }
                }
                yield return new WaitForSeconds(0.01f);

            }

        }

        else if (myCoin == PlayerCoin.Yellow)
        {
            foreach (GameObject coin in colors)
            {
                if (coin.activeInHierarchy && coin.GetComponent<CoinId>().coinType == CoinType.Yellow)
                {
                    foreach (GameObject hole in holes)
                    {
                        GetCoinInfo(coin, hole, strikerPosition);
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
        }
        else if (myCoin == PlayerCoin.Green)
        {
            foreach (GameObject coin in colors)
            {
                if (coin.activeInHierarchy && coin.GetComponent<CoinId>().coinType == CoinType.Green)
                {
                    foreach (GameObject hole in holes)
                    {
                        GetCoinInfo(coin, hole, strikerPosition);
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
        }
        else if (myCoin == PlayerCoin.Brown)
        {
            foreach (GameObject coin in colors)
            {
                if (coin.activeInHierarchy && coin.GetComponent<CoinId>().coinType == CoinType.Brown)
                {
                    foreach (GameObject hole in holes)
                    {
                        GetCoinInfo(coin, hole, strikerPosition);
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
        }
        else if (myCoin == PlayerCoin.Blue)
        {
            foreach (GameObject coin in colors)
            {
                if (coin.activeInHierarchy && coin.GetComponent<CoinId>().coinType == CoinType.Blue)
                {
                    foreach (GameObject hole in holes)
                    {
                        GetCoinInfo(coin, hole, strikerPosition);
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
        }
        else if (myCoin == PlayerCoin.Pink)
        {
            foreach (GameObject coin in colors)
            {
                if (coin.activeInHierarchy && coin.GetComponent<CoinId>().coinType == CoinType.Pink)
                {
                    foreach (GameObject hole in holes)
                    {
                        GetCoinInfo(coin, hole, strikerPosition);
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
        }
        else if (myCoin == PlayerCoin.Black)
        {
            foreach (GameObject coin in colors)
            {
                if (coin.activeInHierarchy && coin.GetComponent<CoinId>().coinType == CoinType.Black)
                {
                    foreach (GameObject hole in holes)
                    {
                        GetCoinInfo(coin, hole, strikerPosition);
                        yield return new WaitForSeconds(0.01f);
                    }
                }
            }
        }

        
            SortCoinsByBlockedParameter();
            yield return new WaitForSeconds(0.02f);
            SortCoinsByAngleParameter();
            yield return new WaitForSeconds(0.02f);
        

        aiData.CoinInformationReceived(id, coinInfoList);

    }

    private void GetCoinInfo(GameObject coin, GameObject hole, Vector3 StrikerPosition)
    {

        if (coin == null)
        {
            return;
        }

        CoinInfo coinInfo = new CoinInfo();
        RaycastHit hitInfo;
        coinInfo.Coin = coin;
        coinInfo.Hole = hole;
        coinInfo.strikerPosition = StrikerPosition;
        Vector3 holedir = (hole.transform.position - coin.transform.position).normalized;
        Vector3 finalPos = coin.transform.position - holedir * (boardData.CoinRadius + boardData.StrikerRadius);
        coinInfo.finalPosition = finalPos;
        Vector3 Strikerdir = (finalPos - StrikerPosition).normalized;
        coinInfo.finalBoardDirection = (coin.transform.position - StrikerPosition).normalized;
        coinInfo.angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(holedir, Strikerdir));
        coinInfo.distance = Vector3.Distance(coin.transform.position, hole.transform.position) + Vector3.Distance(coin.transform.position, StrikerPosition);


        if (Physics.SphereCast(coin.transform.position, boardData.CoinRadius, holedir, out hitInfo,2,LayerMask.GetMask("Coin")))
        {
                GameObject tmpobj = hitInfo.collider.gameObject;
            
                coinInfo.isBlockedH = true;
                coinInfo.blockedCoinAlongHole = tmpobj;

        }

        if (Physics.SphereCast(StrikerPosition, boardData.StrikerRadius, Strikerdir, out hitInfo,2,LayerMask.GetMask("Coin")))
        {
               GameObject tmpobj = hitInfo.collider.gameObject;
            
                if (tmpobj.name != coinInfo.Coin.name)
                {
                    coinInfo.isBlockedC = true;
                    coinInfo.blockedCoinAlongStriker = tmpobj;

                  // Double Touch logic

                    Vector3 newhitPos = coin.transform.position - holedir * (boardData.CoinRadius + boardData.CoinRadius);
                    Vector3 newDir = (newhitPos - tmpobj.transform.position).normalized;
                    Vector3 newFinalPos = tmpobj.transform.position - newDir * (boardData.CoinRadius + boardData.StrikerRadius);

                   coinInfo.finalPosition = newFinalPos;
                   Vector3 newStrikerDir = (newFinalPos - StrikerPosition).normalized;
                   coinInfo.finalBoardDirection = (tmpobj.transform.position - StrikerPosition).normalized;
                   coinInfo.distance += Vector3.Distance(tmpobj.transform.position, coin.transform.position) + 0.1f;



                }
                else
                {
                    coinInfo.isBlockedC = false;
                    coinInfo.blockedCoinAlongStriker = tmpobj;
                }

        }
        this.coinInfoList.Add(coinInfo);

    }
    private void SortCoinsByBlockedParameter()
    {
        for (int j = 0; j <= coinInfoList.Count - 1; j++)
        {
            for (int i = 0; i <= coinInfoList.Count - 2; i++)
            {

                CoinInfo b1 = coinInfoList[i];
                CoinInfo b2 = coinInfoList[i + 1];
                if (b1.isBlockedC && b2.isBlockedC)
                {
                    if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }

                }
                else if (b1.isBlockedC && !b2.isBlockedC)
                {
                    if (b1.isBlockedH && b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                    else if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                    else if (!b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && !b2.isBlockedC)
                {
                    if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                }

            }
        }
       
    }

    private void SortCoinsByAngleParameter()
    {
        for (int j = 0; j <= coinInfoList.Count - 1; j++)
        {
            for (int i = 0; i <= coinInfoList.Count - 2; i++)
            {
                CoinInfo b1 = coinInfoList[i];
                CoinInfo b2 = coinInfoList[i + 1];
                if (!b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }

            }
        }
       
    }
    private void CopyData(int i)
    {
        CoinInfo b = new CoinInfo();
        b = coinInfoList[i];
        coinInfoList[i] = coinInfoList[i + 1];
        coinInfoList[i + 1] = b;
    }
}
