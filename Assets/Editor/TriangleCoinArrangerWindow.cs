using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TriangleCoinArrangerWindow : EditorWindow
{
    private GameObject coinPrefab;
    private Transform parentRoot;

    private int totalCoins = 15;
    private float coinDiameter = 0.1f;
    private float gap = 0.02f;
    private Vector3 startPosition = Vector3.zero;
    private bool useLocalPosition = true;
    private bool renameCoins = true;
    private bool forceWorldUp = true;

    [MenuItem("Tools/VisionXR/Triangle Coin Arranger")]
    public static void ShowWindow()
    {
        GetWindow<TriangleCoinArrangerWindow>("Triangle Coin Arranger");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Equilateral Triangle Coin Arranger", EditorStyles.boldLabel);

        coinPrefab = (GameObject)EditorGUILayout.ObjectField("Coin Prefab", coinPrefab, typeof(GameObject), false);
        parentRoot = (Transform)EditorGUILayout.ObjectField("Parent Root", parentRoot, typeof(Transform), true);

        GUILayout.Space(5);

        totalCoins = EditorGUILayout.IntField("Total Coins", totalCoins);
        coinDiameter = EditorGUILayout.FloatField("Coin Diameter", coinDiameter);
        gap = EditorGUILayout.FloatField("Gap Between Coins", gap);
        startPosition = EditorGUILayout.Vector3Field("Start Position", startPosition);
        useLocalPosition = EditorGUILayout.Toggle("Use Local Position", useLocalPosition);
        renameCoins = EditorGUILayout.Toggle("Rename Coins", renameCoins);
        forceWorldUp = EditorGUILayout.Toggle("Force Coin Up = World Up", forceWorldUp);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate / Refresh Coins"))
        {
            GenerateOrRefreshCoins();
        }

        if (GUILayout.Button("Arrange Existing Child Coins"))
        {
            ArrangeExistingCoins();
        }

        if (GUILayout.Button("Clear Children"))
        {
            ClearChildren();
        }

        GUILayout.Space(10);
        EditorGUILayout.HelpBox("For 15 coins this creates rows of 1, 2, 3, 4, 5 in an equilateral triangle layout.", MessageType.Info);
    }

    private void GenerateOrRefreshCoins()
    {
        if (parentRoot == null)
        {
            EditorUtility.DisplayDialog("Missing Parent", "Please assign Parent Root.", "OK");
            return;
        }

        if (coinPrefab == null)
        {
            EditorUtility.DisplayDialog("Missing Prefab", "Please assign Coin Prefab.", "OK");
            return;
        }

        ClearChildrenInternal();

        List<Transform> coins = new List<Transform>();

        for (int i = 0; i < totalCoins; i++)
        {
            GameObject coinInstance = (GameObject)PrefabUtility.InstantiatePrefab(coinPrefab);
            if (coinInstance == null)
            {
                coinInstance = Instantiate(coinPrefab);
            }

            Undo.RegisterCreatedObjectUndo(coinInstance, "Create Coin");
            coinInstance.transform.SetParent(parentRoot, false);

            if (renameCoins)
            {
                coinInstance.name = "Coin_" + (i + 1);
            }

            if (forceWorldUp)
            {
                coinInstance.transform.rotation = Quaternion.identity;
            }

            coins.Add(coinInstance.transform);
        }

        ArrangeCoins(coins);
    }

    private void ArrangeExistingCoins()
    {
        if (parentRoot == null)
        {
            EditorUtility.DisplayDialog("Missing Parent", "Please assign Parent Root.", "OK");
            return;
        }

        List<Transform> coins = new List<Transform>();

        for (int i = 0; i < parentRoot.childCount; i++)
        {
            coins.Add(parentRoot.GetChild(i));
        }

        if (coins.Count == 0)
        {
            EditorUtility.DisplayDialog("No Children Found", "Parent Root has no child coins to arrange.", "OK");
            return;
        }

        if (forceWorldUp)
        {
            Undo.RecordObjects(coins.ToArray(), "Align Coin Rotation");
            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].rotation = Quaternion.identity;
            }
        }

        ArrangeCoins(coins);
    }

    private void ArrangeCoins(List<Transform> coins)
    {
        if (coins == null || coins.Count == 0)
        {
            return;
        }

        int rows = GetRequiredRows(coins.Count);

        Object[] undoObjects = new Object[coins.Count];
        for (int i = 0; i < coins.Count; i++)
        {
            undoObjects[i] = coins[i];
        }
        Undo.RecordObjects(undoObjects, "Arrange Triangle Coins");

        float horizontalSpacing = coinDiameter + gap;
        float verticalSpacing = horizontalSpacing * Mathf.Sqrt(3f) * 0.5f;

        int coinIndex = 0;

        for (int row = 0; row < rows; row++)
        {
            int coinsInRow = row + 1;
            float rowWidth = (coinsInRow - 1) * horizontalSpacing;
            float rowStartX = -rowWidth * 0.5f;

            for (int col = 0; col < coinsInRow; col++)
            {
                if (coinIndex >= coins.Count)
                {
                    break;
                }

                Vector3 targetPosition = new Vector3(
                    rowStartX + col * horizontalSpacing,
                    0f,
                    -row * verticalSpacing
                ) + startPosition;

                if (useLocalPosition)
                {
                    coins[coinIndex].localPosition = targetPosition;
                }
                else
                {
                    coins[coinIndex].position = targetPosition;
                }

                if (forceWorldUp)
                {
                    coins[coinIndex].rotation = Quaternion.identity;
                }

                coinIndex++;
            }
        }

        EditorUtility.SetDirty(parentRoot);
        Debug.Log("Coins arranged in equilateral triangle.");
    }

    private int GetRequiredRows(int count)
    {
        int rows = 0;
        int total = 0;

        while (total < count)
        {
            rows++;
            total += rows;
        }

        return rows;
    }

    private void ClearChildren()
    {
        if (parentRoot == null)
        {
            EditorUtility.DisplayDialog("Missing Parent", "Please assign Parent Root.", "OK");
            return;
        }

        ClearChildrenInternal();
    }

    private void ClearChildrenInternal()
    {
        if (parentRoot == null)
        {
            return;
        }

        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < parentRoot.childCount; i++)
        {
            children.Add(parentRoot.GetChild(i).gameObject);
        }

        for (int i = children.Count - 1; i >= 0; i--)
        {
            Undo.DestroyObjectImmediate(children[i]);
        }
    }
}