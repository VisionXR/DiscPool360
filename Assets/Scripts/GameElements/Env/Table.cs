using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public TableDataSO tableData;
    public GameObject cameraRig;
    public GameObject mainCanvas;
    public GameObject poolCanvas;
    public GameObject snookerCanvas;
    public GameObject FoulCanvas;

    [Header("Game Objects")]
    public List<Transform> camPositions;
    public List<Transform> canvasPositions;
    public GameObject playerTransforms;
    



    public void SetTableRotation(int index)
    {
        if (index < 0 || index >= camPositions.Count)
        {
            Debug.LogError("Index out of range for table positions.");
            return;
        }

        cameraRig.transform.position = camPositions[index-1].position;
        cameraRig.transform.rotation = camPositions[index-1].rotation;


       
      
    }


}
