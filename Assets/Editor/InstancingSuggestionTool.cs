#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectMaterialInstancingChecker
{
    [MenuItem("Tools/Check All Project Materials for GPU Instancing")]
    public static void CheckAllMaterials()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");

        Debug.Log("=== Project-Wide GPU Instancing Check ===");

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null) continue;

            string status = mat.enableInstancing ? "✅ Instancing ON" : "❌ Instancing OFF";
            Debug.Log($"{mat.name} ({path}) — {status}");
        }
    }
}
#endif
