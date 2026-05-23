using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIHitTest : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;
    public StrikerDataSO strikerData;
    public LineRenderer lineRenderer;
    public CoinSorter coinSorter;

    public GameObject testCoin;
    public GameObject striker;
    public int testHole;

    [Header("Key Bindings (New Input System)")]
    public Key ShootStriker = Key.L;
    public Key ResetKey = Key.R;
    public Key SaveKey = Key.S;


    private Vector3 strikerInitPos;
    private Vector3 coinInitPos;


    private void Start()
    {
        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 0.005f;
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null)
            return;

        if (testCoin != null && kb[ShootStriker].wasPressedThisFrame)

        {
            CoinInfo info = GetInfo(testCoin, testHole, strikerData.currentStriker.transform.position);
            if (info != null)
            {
                lineRenderer.positionCount = 3;
                lineRenderer.SetPosition(0, strikerData.currentStriker.transform.position);
                lineRenderer.SetPosition(1, info.finalPosition);
                lineRenderer.SetPosition(2, info.Hole.transform.position);
            }

            StrikerShooting strikerShooting = striker.GetComponent<StrikerShooting>();
            Vector3 dir = (info.finalPosition - striker.transform.position).normalized;
            strikerShooting.SetForceAndDir(2, dir);
        }

        if (testCoin != null && kb[SaveKey].wasPressedThisFrame)
        {
            strikerInitPos = striker.transform.position;
            coinInitPos = testCoin.transform.position;
            striker.SetActive(true);
            testCoin.SetActive(true);
            testCoin.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }

        if (testCoin != null && kb[ResetKey].wasPressedThisFrame)
        {
            striker.transform.position = strikerInitPos;
            striker.transform.rotation = Quaternion.identity;
            testCoin.transform.position = coinInitPos;
            testCoin.transform.rotation = Quaternion.identity;
            lineRenderer.positionCount = 0;
            striker.SetActive(true);
            testCoin.SetActive(true);
            testCoin.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    public CoinInfo GetInfo(GameObject coin,int holeid,Vector3 StrikerPosition)
    {

        CoinInfo coinInfo = new CoinInfo();
        RaycastHit hitInfo;
        GameObject hole = boardData.Holes[holeid];
        coinInfo.Coin = coin;
        coinInfo.Hole = boardData.Holes[holeid];
        coinInfo.strikerPosition = StrikerPosition;
        Vector3 holedir = (hole.transform.position - coin.transform.position).normalized;
        Vector3 finalPos = coin.transform.position - holedir * (boardData.CoinRadius + boardData.StrikerRadius);
        coinInfo.finalPosition = finalPos;
        Vector3 Strikerdir = (finalPos - StrikerPosition).normalized;
        coinInfo.finalBoardDirection = (coin.transform.position - StrikerPosition).normalized;
        coinInfo.angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(holedir, Strikerdir));
        coinInfo.distance = Vector3.Distance(coin.transform.position, hole.transform.position) + Vector3.Distance(coin.transform.position, StrikerPosition);


        if (Physics.SphereCast(coin.transform.position, boardData.CoinRadius, holedir, out hitInfo, 2, LayerMask.GetMask("Coin")))
        {
            GameObject tmpobj = hitInfo.collider.gameObject;

            coinInfo.isBlockedH = true;
            coinInfo.blockedCoinAlongHole = tmpobj;

        }

        if (Physics.SphereCast(StrikerPosition, boardData.StrikerRadius, Strikerdir, out hitInfo, 2, LayerMask.GetMask("Coin")))
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

        return coinInfo;
    }

}
