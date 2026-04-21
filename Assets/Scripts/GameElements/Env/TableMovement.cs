using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class TableMovement : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public TableDataSO tableData;

    [Header("Follower Objects")]
    public GameObject TableHighLight;
    public Transform tableTransform;
    public List<GameObject> followerObjects;

    // local variables
    public bool canIMove = false;
    private Vector3 lastPosition;

    public void OnHovered()
    {
       
        TableHighLight.SetActive(true);
    }

    public void OnUnhovered()
    {
       
        TableHighLight.SetActive(false);
    }


}
