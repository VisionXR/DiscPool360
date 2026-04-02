using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class BoardProperties : MonoBehaviour
{
   
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;


    [Header("Board  list Properties")]
    public List<GameObject> Holes;
    //public List<GameObject> HolesTriggers;
    //public List<GameObject> HolesGlows;
    public List<Transform> StrikerFoulPositions;
    public List<Transform> SnookerCoinPositions;


    [Header("Board Properties")]
    public Transform AllCoinsTransform;
    public Transform StrikerTransform;

    public GameObject Board;
    public GameObject SnookerObject;
    public GameObject EdgeHighLight;

    

    private void OnEnable()
    {
            // Set all properties from this script to the ScriptableObject
            boardData.SetBoard(Board);
            boardData.SetHoles(Holes);
            boardData.SetEdgeHighlight(EdgeHighLight);
            //boardData.SetHoleGlows(HolesGlows);
            boardData.SetStrikerFoulPositions(StrikerFoulPositions);
            //boardData.SetHoleTriggers(HolesTriggers);
            boardData.SetAllCoinsTransform(AllCoinsTransform);
            boardData.SetStrikerTransform(StrikerTransform);

            boardData.SetSnookerObject(SnookerObject);
            boardData.SetSnookerCoinPositions(SnookerCoinPositions);

    }
     
 }






