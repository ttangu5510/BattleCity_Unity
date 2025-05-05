using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

public class FindComponentByNameEditor : EditorWindow
{
    private string componentName = "BoxCollider";
    private Vector2 scrollPos;
    private List<GameObject> foundObjects = new List<GameObject>();

    [MenuItem("Tools/Find Component By Name")]
    public static void ShowWindow()
    {
        GetWindow<FindComponentByNameEditor>("Find Component");
    }

    private void OnGUI()
    {
        GUILayout.Label("Search for Component by Class Name", EditorStyles.boldLabel);

        componentName = EditorGUILayout.TextField("Component Name", componentName);

        if (GUILayout.Button("Search"))
        {
            SearchObjectsWithComponent();
        }

        GUILayout.Space(10);

        if (foundObjects.Count > 0)
        {
            GUILayout.Label($"Found {foundObjects.Count} GameObject(s):", EditorStyles.boldLabel);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (var obj in foundObjects)
            {
                EditorGUILayout.ObjectField(obj.transform.GetHierarchyPath(), obj, typeof(GameObject), true);
            }

            EditorGUILayout.EndScrollView();
        }
    }

    private void SearchObjectsWithComponent()
    {
        foundObjects.Clear();

        // 사용자 정의 클래스도 포함해서 모든 컴포넌트 타입을 검색
        Type type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .FirstOrDefault(t =>
                typeof(Component).IsAssignableFrom(t) &&
                t.Name.Equals(componentName, StringComparison.OrdinalIgnoreCase));

        if (type == null)
        {
            Debug.LogError($"Component type '{componentName}' not found.");
            return;
        }

        GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var root in allObjects)
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                if (t.GetComponent(type) != null)
                    foundObjects.Add(t.gameObject);
            }
        }

        Debug.Log($"Found {foundObjects.Count} GameObjects with component '{componentName}'.");
    }
}

// 확장 메서드로 Transform의 전체 경로 추출
public static class TransformExtensions
{
    public static string GetHierarchyPath(this Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}