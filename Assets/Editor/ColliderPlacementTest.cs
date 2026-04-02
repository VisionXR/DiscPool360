using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ColliderPlacementTest : EditorWindow
{
    private Transform centerObject;
    private GameObject cubePrefab;
    private float radius = 5f;
    private int numberOfCubes = 32;
    private List<GameObject> placedCubes = new List<GameObject>();

    [MenuItem("Tests/Collider Placement Test")]
    public static void ShowWindow()
    {
        GetWindow<ColliderPlacementTest>("Collider Placement Test");
    }

    private void OnGUI()
    {
        GUILayout.Label("Collider Placement Settings", EditorStyles.boldLabel);

        centerObject = (Transform)EditorGUILayout.ObjectField("Center Object", centerObject, typeof(Transform), true);
        cubePrefab = (GameObject)EditorGUILayout.ObjectField("Cube Prefab", cubePrefab, typeof(GameObject), false);
        radius = EditorGUILayout.FloatField("Radius", radius);
        numberOfCubes = EditorGUILayout.IntSlider("Number of Cubes", numberOfCubes, 1, 128);

        if (GUILayout.Button("Place Colliders"))
        {
            PlaceColliders();
        }
    }

    private void PlaceColliders()
    {
        if (cubePrefab == null || centerObject == null)
        {
            Debug.LogWarning("Assign both Center Object and Cube Prefab.");
            return;
        }

        // Compact list: remove null or destroyed entries to avoid indexing destroyed objects
        for (int i = placedCubes.Count - 1; i >= 0; i--)
        {
            if (placedCubes[i] == null)
            {
                placedCubes.RemoveAt(i);
            }
        }

        // Remove extra cubes
        while (placedCubes.Count > numberOfCubes)
        {
            var last = placedCubes[placedCubes.Count - 1];
            if (last != null)
            {
                DestroyImmediate(last);
            }
            placedCubes.RemoveAt(placedCubes.Count - 1);
        }

        // Create missing cubes
        for (int i = placedCubes.Count; i < numberOfCubes; i++)
        {
            var cube = (GameObject)PrefabUtility.InstantiatePrefab(cubePrefab, centerObject);
            placedCubes.Add(cube);
        }

        // Position and orient cubes safely
        for (int i = 0; i < numberOfCubes; i++)
        {
            var cube = placedCubes[i];
            if (cube == null) continue; // skip destroyed/missing entries

            float angle = i * Mathf.PI * 2f / numberOfCubes;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            Vector3 position = centerObject.position + offset;

            var t = cube.transform; // safe because cube != null
            t.position = position;
            t.LookAt(centerObject.position);
            t.Rotate(0f, 90f, 0f); // Adjust if needed for your prefab orientation
        }
    }
}