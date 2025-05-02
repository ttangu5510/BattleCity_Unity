using UnityEngine;
using UnityEditor;
using System.IO;

public class FindMissingScriptsInProject
{
    [MenuItem("Tools/Find Missing Scripts in Project (Prefabs, Assets)")]
    public static void FindInProject()
    {
        int count = 0;
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (go == null) continue;

            Component[] components = go.GetComponentsInChildren<Component>(true);
            foreach (Component comp in components)
            {
                if (comp == null)
                {
                    Debug.LogWarning($"Missing script found in prefab: {path}", go);
                    count++;
                }
            }
        }

        Debug.Log($"프로젝트 검사 완료: 누락된 스크립트 {count}개 발견");
    }
}
