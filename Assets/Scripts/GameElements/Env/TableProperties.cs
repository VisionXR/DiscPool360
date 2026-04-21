using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class TableProperties : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public TableDataSO tableData;

    [Header("Game Objects")]
    public GameObject cam;
    public GameObject allAssets;
    public Transform BoardTransform;
    public List<Transform> playerTransforms;
    public List<Transform> canvasTransforms;
    public List<Transform> camTransforms;


    private void OnEnable()
    {
        tableData.SetBoardPosition(BoardTransform);
        tableData.SetPlayerPositions(playerTransforms);
        tableData.SetMainCanvasPositions(canvasTransforms);
        tableData.SetCamPositions(camTransforms);
     
        tableData.SetAllAssets(allAssets);
    }
}
