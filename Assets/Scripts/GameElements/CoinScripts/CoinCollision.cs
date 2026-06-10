using com.VisionXR.ModelClasses;
using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public CoinDataSO coinDataSO;

    [Header(" Local Objects")]
    public Rigidbody coinRigidBody;
    public void OnCollisionEnter(Collision collision)
    {
  
        if (collision.collider.gameObject.tag == "Hole")
        {
            coinRigidBody.linearVelocity = Vector3.zero;
            coinRigidBody.isKinematic = true;
            coinDataSO.CoinPocketed(gameObject);
            coinDataSO.CoinPocketedIntoHole(collision.collider.gameObject);          
            gameObject.SetActive(false);
           
        }
        else if (collision.collider.gameObject.tag == "Floor")
        {
            coinRigidBody.linearVelocity = Vector3.zero;
            coinRigidBody.isKinematic = true;
            coinDataSO.CoinFellOnGround(gameObject);
        }
    }


  
}
