using UnityEngine;
using UnityEditor;

public class CreateLibratyAssetAction {
	[MenuItem("Assets/Create/Tileset Resource Asset")]
	public static void CreateMyAsset() {
        ScriptableObject asset = null;// ScriptableObject.CreateInstance<ResourceLibrary>();

		AssetDatabase.CreateAsset(asset, "Assets/Resources/ResourceLibrary_new.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}
}
