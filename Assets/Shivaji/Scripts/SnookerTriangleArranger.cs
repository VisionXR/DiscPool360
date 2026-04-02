using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnookerTriangleArranger : MonoBehaviour
{
    public BoardDataSO boardData;
    public List<GameObject> colorCoins;
    public void Start()
    {
        Debug.Log("Setting snooker coin positions");
         List<Transform> positions = boardData.SnookerCoinPositions;
         for(int i=0;  i< positions.Count; i++)
         {
                colorCoins[i].transform.position = positions[i].position;
                colorCoins[i].transform.rotation = positions[i].rotation;
        }

    }
}
