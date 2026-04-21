using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public CoinDataSO coinDataSO;

    
    public void OnCollisionEnter(Collision collision)
    {
  
        if (collision.collider.gameObject.tag == "Hole")
        {           
            coinDataSO.CoinPocketed(gameObject);
            coinDataSO.CoinPocketedIntoHole(collision.collider.gameObject);          
            gameObject.SetActive(false);
           
        }
        else if (collision.collider.gameObject.tag == "Floor")
        {
            coinDataSO.CoinFellOnGround(gameObject);
        }
    }


  
}
