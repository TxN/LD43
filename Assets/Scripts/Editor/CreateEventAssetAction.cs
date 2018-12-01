using UnityEngine;
using UnityEditor;

public class CreateLibratyAssetAction {
	[MenuItem("Assets/Create/Tileset Resource Asset")]
	public static void CreateMyAsset() {
        StoryEvent asset = ScriptableObject.CreateInstance<StoryEvent>();

		AssetDatabase.CreateAsset(asset, "Assets/Resources/StoryEvents/StoryEvent_new.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}
}
