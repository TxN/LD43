using UnityEngine;
using UnityEditor;

public class CreateLibratyAssetAction {
	[MenuItem("Assets/Create/Tileset Resource Asset")]
	public static void CreateMyAsset() {
        TemplateEvent asset = ScriptableObject.CreateInstance<TemplateEvent>();

		AssetDatabase.CreateAsset(asset, "Assets/Resources/StoryEvents/DefaultEvent_new.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}
}
