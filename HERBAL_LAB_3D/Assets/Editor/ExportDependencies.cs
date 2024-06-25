using UnityEditor;
using UnityEngine;

public class ExportDependencies
{
    [MenuItem("Tools/Export Selected GameObject With Dependencies")]
    public static void ExportSelectedWithDependencies()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("No GameObject selected. Please select a GameObject.");
            return;
        }

        string path = EditorUtility.SaveFilePanel("Export Package", "", Selection.activeGameObject.name + ".unitypackage", "unitypackage");
        if (string.IsNullOrEmpty(path))
            return;

        Object[] selection = Selection.objects;
        AssetDatabase.ExportPackage(AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName("bundle", "name"), path, ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Exported package to: " + path);
    }
}
