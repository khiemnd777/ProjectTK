// using UnityEngine;
// using UnityEditor;
// using System.IO;

// public static class ScriptableObjectUtility
// {
//     /// <summary>
//     ///	This makes it easy to create, name and place unique new ScriptableObject asset files.
//     /// </summary>
//     public static void CreateAsset<T>(string name = null, string path = "", System.Action<T> postCreated = null) where T : ScriptableObject
//     {
//         T asset = ScriptableObject.CreateInstance<T>();
//         if (path == "")
//         {
//             path = AssetDatabase.GetAssetPath(Selection.activeObject);
//             if (path == "")
//                 path = "Assets";
//             else if (Path.GetExtension(path) != "")
//                 path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
//         }

//         var displayName = name ?? "New " + typeof(T).ToString();
//         displayName = "/" + displayName + ".asset";
//         var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + displayName);

//         AssetDatabase.CreateAsset(asset, assetPathAndName);
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//         // EditorUtility.FocusProjectWindow();
//         Selection.activeObject = asset;
//         if(postCreated != null){
//             postCreated.Invoke(asset);
//         }
//     }
// }