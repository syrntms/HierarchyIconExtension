using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[InitializeOnLoad]
public class MissingScriptIconExtension : IHierarchyIconExtensionFeature {

	private static Texture2D iconTexture;
	private static Dictionary<int, Texture2D> instnceIdToTexture = new Dictionary<int, Texture2D>();

	static MissingScriptIconExtension()
	{
		HierarchyIconExtensionList.AddExtension(new MissingScriptIconExtension());
		iconTexture = AssetDatabase.LoadAssetAtPath(
			"Assets/HierarchyIconExtension/Editor/HierarchyIconExtension/Feature/Alice.png",
			typeof(Texture2D)
		) as Texture2D;
	}

	public int GetPriority()
	{
		return 0;
	}

	public Texture2D GetDisplayIcon(int instanceId)
	{
		bool ret = HasMissingScript(EditorUtility.InstanceIDToObject(instanceId) as GameObject);
		return ret ? iconTexture : null;
	}

	public bool HasMissingScript(GameObject go)
	{
		var components = go.GetComponents<Component>();
		bool isDisplay = components.Any(component => component == null);
		if (isDisplay) {
			return true;
		}
		foreach (var child in go.transform.Cast<Transform>()) {
			bool ret = HasMissingScript(child.gameObject);
			if (ret) {
				return true;
			}
		}
		return false;
	}

}
