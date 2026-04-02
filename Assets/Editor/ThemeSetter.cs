using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
// UIGradient is in global namespace (no using required)

public class ThemeSetter : EditorWindow
{
    [SerializeField] private Color idleColor = Color.white;
    [SerializeField] private Color hoverColor = Color.gray;
    [SerializeField] private Color color1 = Color.white;
    [SerializeField] private Color color2 = Color.black;

    [SerializeField] private List<GameObject> buttonPrefabs = new List<GameObject>();

    private SerializedObject so;

    [MenuItem("Tools/Theme Setter")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<ThemeSetter>("Theme Setter");
        wnd.minSize = new Vector2(420, 300);
    }

    private void ApplyThemeToButtonPrefabs(List<GameObject> list, Color bgColor, Color hoverColor, Color g1, Color g2)
    {
        if (list == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            var prefab = list[i];
            if (prefab == null) continue;
            string path = AssetDatabase.GetAssetPath(prefab);

            // Helper to apply to a root GameObject (either prefab contents or scene object)
            System.Action<GameObject, bool> applyToRoot = (root, isScene) =>
            {
                // Find background child by common names
                Transform bg = FindChildByNames(root.transform, new[] { "BackgroundImage", "BackGroundImage", "Background", "BG" });
                Transform hover = FindChildByNames(root.transform, new[] { "HoverImage", "Hover", "HoverImg" });

                int changes = 0;

                if (bg != null)
                {
                    var img = bg.GetComponent<Image>();
                    if (img != null)
                    {
                        Undo.RecordObject(img, "ThemeSetter Change Background Image Color");
                        img.color = bgColor;
                        EditorUtility.SetDirty(img);
                        changes++;
                    }

                    var grad = bg.GetComponent<UIGradient>();
                    if (grad != null)
                    {
                        Undo.RecordObject(grad, "ThemeSetter Change Gradient");
                        grad.m_color1 = g1;
                        grad.m_color2 = g2;
                        EditorUtility.SetDirty(grad);
                        changes++;
                    }
                }

                if (hover != null)
                {
                    var img = hover.GetComponent<Image>();
                    if (img != null)
                    {
                        Undo.RecordObject(img, "ThemeSetter Change Hover Image Color");
                        img.color = hoverColor;
                        EditorUtility.SetDirty(img);
                        changes++;
                    }
                }

                if (changes > 0)
                {
                    if (isScene)
                    {
                        Debug.Log($"ThemeSetter: Applied theme to scene object '{root.name}', changed {changes} component(s).");
                        EditorSceneManager.MarkSceneDirty(root.scene);
                    }
                }
                else
                {
                    Debug.LogWarning($"ThemeSetter: No matching Background/Hover/UIGradient found in '{root.name}'.");
                }
            };

            if (string.IsNullOrEmpty(path))
            {
                // scene object
                applyToRoot(prefab, true);
                continue;
            }

            GameObject rootPrefab = PrefabUtility.LoadPrefabContents(path);
            if (rootPrefab == null)
            {
                Debug.LogWarning($"ThemeSetter: Failed to load prefab at path '{path}'.");
                continue;
            }
            try
            {
                applyToRoot(rootPrefab, false);
                PrefabUtility.SaveAsPrefabAsset(rootPrefab, path);
                Debug.Log($"ThemeSetter: Applied theme to prefab asset '{path}'.");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(rootPrefab);
            }
        }
    }

    // Find descendant transform by any of the provided names (case-insensitive)
    private Transform FindChildByNames(Transform root, string[] names)
    {
        if (root == null) return null;
        var q = new System.Collections.Generic.Queue<Transform>();
        q.Enqueue(root);
        while (q.Count > 0)
        {
            var t = q.Dequeue();
            foreach (var n in names)
            {
                if (string.Equals(t.name, n, System.StringComparison.OrdinalIgnoreCase)) return t;
            }
            for (int i = 0; i < t.childCount; i++) q.Enqueue(t.GetChild(i));
        }
        return null;
    }

    private void OnEnable()
    {
        so = new SerializedObject(this);
    }

    private void OnDisable()
    {
        so = null;
    }

    private void OnGUI()
    {
        if (so == null) so = new SerializedObject(this);
        so.Update();

        EditorGUILayout.LabelField("Theme Colors", EditorStyles.boldLabel);
        idleColor = EditorGUILayout.ColorField("Idle Color", idleColor);
        hoverColor = EditorGUILayout.ColorField("Hover Color", hoverColor);
        color1 = EditorGUILayout.ColorField("Color 1", color1);
        color2 = EditorGUILayout.ColorField("Color 2", color2);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Button Prefabs (Background/Hover/UIGradient)", EditorStyles.boldLabel);
        DrawPrefabList(so.FindProperty("buttonPrefabs"));

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Drag prefabs (UI Image prefabs) into the lists. Press 'Change Theme' to apply colors to prefab assets.", MessageType.Info);

        if (GUILayout.Button("Change Theme", GUILayout.Height(32)))
        {
            ApplyThemeToPrefabs();
        }

        so.ApplyModifiedProperties();
    }

    private void DrawPrefabList(SerializedProperty listProp)
    {
        if (listProp == null) return;
        EditorGUILayout.PropertyField(listProp, true);
    }

    private void ApplyThemeToPrefabs()
    {
        // Buttons: set BackgroundImage color = idleColor, HoverImage color = hoverColor, and gradient colors
        ApplyThemeToButtonPrefabs(buttonPrefabs, idleColor, hoverColor, color1, color2);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ThemeSetter: Applied theme to prefabs.");
    }

 

   
}
