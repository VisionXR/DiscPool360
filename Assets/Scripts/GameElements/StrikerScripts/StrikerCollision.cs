using com.VisionXR.ModelClasses;
using System.Text.RegularExpressions;
using UnityEngine;

public class StrikerCollision : MonoBehaviour
{
    public StrikerDataSO strikerData;
    

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Hole")
        {
            GetComponent<Rigidbody>().isKinematic = true;

            // Expect names like "Hole1", "Hole2" where the fifth character (index 4) is the id
            int holeId = 1;
            string name = collision.gameObject.name ?? string.Empty;
            if (name.Length > 4 && char.IsDigit(name[4]))
            {
                holeId = (int)char.GetNumericValue(name[4]);
            }
            else
            {
                // Fallback to regex if naming differs
                var m = Regex.Match(name, "\\d+");
                if (m.Success)
                {
                    int.TryParse(m.Value, out holeId);
                }
                else
                {
                    Debug.LogWarning($"Could not parse hole id from object name '{name}'");
                }
            }

            // You can add functionality here for when the striker collides with a hole
            strikerData.StrikerPocketed(holeId);
        }

        else
        {
            
            strikerData.StrikerCollided(collision.gameObject);
        }

    }

}
