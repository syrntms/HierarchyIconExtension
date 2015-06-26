using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[InitializeOnLoad]
public class MissingScriptIconExtension : HierarchyIconFeatureBase {

	private static Texture2D iconTexture;

	static MissingScriptIconExtension()
	{
		HierarchyIconExtension.Extensions.Add(new MissingScriptIconExtension());
		iconTexture = AssetDatabase.LoadAssetAtPath ("Assets/Editor/Feature/Alice.png", typeof(Texture2D)) as Texture2D;
	}

	public override int GetPriority()
	{
		return 0;
	}

	public override Texture2D GetIconTexture()
	{
		return iconTexture;
	}

	public override bool IsDisplayIcon(GameObject go)
	{
		var components = go.GetComponents<Component>();
		return components.Any(component => component == null);
	}

}
