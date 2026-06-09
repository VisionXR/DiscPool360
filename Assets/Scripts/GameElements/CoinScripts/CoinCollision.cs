using com.VisionXR.ModelClasses;
using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public CoinDataSO coinDataSO;

    
    public void OnCollisionEnter(Collision collision)
    {
  
        if (collision.collider.gameObject.tag == "Hole")
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            coinDataSO.CoinPocketed(gameObject);
            coinDataSO.CoinPocketedIntoHole(collision.collider.gameObject);          
            gameObject.SetActive(false);
           
        }
        else if (collision.collider.gameObject.tag == "Floor")
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            coinDataSO.CoinFellOnGround(gameObject);
        }
    }


  
}
