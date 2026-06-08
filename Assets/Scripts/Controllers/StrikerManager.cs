using com.VisionXR.ModelClasses;
using UnityEngine;

public class StrikerManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public StrikerDataSO strikerData;
    public BoardDataSO boardData;
    public PlayerDataSO playerData;
    public UIDataSO uiData;

    [Header("Game Objects")]
    public GameObject AllAssets;
    private GameObject currentStriker;

    public string resourceFolderPath = "Strikers/";
    public LayerMask placementLayerMask; // Layer mask to specify which layers to check for placement
    public float raycastDistance = 2.0f;
    public float boardLift = 0.01f;
    public float placeRadius = 0.25f;

    private void OnEnable()
    {
        strikerData.CreateStrikerEvent += CreateStriker;
        strikerData.DestroyStrikerEvent += DestroyStriker;

        strikerData.PlaceStrikerOnEdgeEvent += PlaceStrikerOnEdge;
        strikerData.ResetStrikerEvent += ResetStriker;

    }

    private void OnDisable()
    {
        strikerData.CreateStrikerEvent -= CreateStriker;
        strikerData.DestroyStrikerEvent -= DestroyStriker;

        strikerData.PlaceStrikerOnEdgeEvent -= PlaceStrikerOnEdge;
        strikerData.ResetStrikerEvent -= ResetStriker;

    }

    public void PlaceStrikerOnEdge(int id)
    {
        if (currentStriker == null)
        {
            currentStriker = strikerData.currentStriker;
        }


        Rigidbody strikerRigidbody = currentStriker.GetComponent<Rigidbody>();
        if (strikerRigidbody != null)
        {
            strikerRigidbody.isKinematic = true;
            strikerRigidbody.transform.position = boardData.StrikerFoulPositions[id - 1].position;
            strikerRigidbody.transform.rotation = boardData.StrikerFoulPositions[id - 1].rotation;

        }
    }

    private void CreateStriker(Transform strikerTransform)
    {
        if (currentStriker != null)
        {
            Destroy(currentStriker);
        }

        string path = resourceFolderPath + "Striker";
        GameObject strikerPrefab = Resources.Load<GameObject>(path);

        currentStriker = Instantiate(strikerPrefab, strikerTransform.transform.position, strikerTransform.transform.rotation);
        currentStriker.transform.localScale = Vector3.one*0.75f;
        currentStriker.transform.SetParent(AllAssets.transform);
    }

    private void DestroyStriker()
    {
        if (currentStriker != null)
        {
            Destroy(currentStriker);
            currentStriker = null;
        }
    }

    private void ResetStriker()
    {
        if (currentStriker != null)
        {
            currentStriker.transform.position = boardData.StrikerTransform.transform.position;
            currentStriker.transform.rotation = boardData.StrikerTransform.transform.rotation;
        }
    }



}
