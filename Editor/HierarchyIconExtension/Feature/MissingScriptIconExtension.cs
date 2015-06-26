using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[InitializeOnLoad]
public class MissingScriptIconExtension : IHierarchyIconExtensionFeature {

	private static Texture2D iconTexture;

	static MissingScriptIconExtension()
	{
		HierarchyIconExtension.AddExtension(new MissingScriptIconExtension());
		iconTexture = AssetDatabase.LoadAssetAtPath(
			"Assets/HierarchyIconExtension/Editor/HierarchyIconExtension/Feature/Alice.png",
			typeof(Texture2D)
		) as Texture2D;
	}

	public int GetPriority()
	{
		return 0;
	}

	public Texture2D GetDisplayIcon(GameObject go)
	{
		var components = go.GetComponents<Component>();
		bool isDisplay = components.Any(component => component == null);
		return isDisplay ? iconTexture : null;
	}

}
