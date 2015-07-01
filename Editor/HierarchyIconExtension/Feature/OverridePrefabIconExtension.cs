using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Linq;

[InitializeOnLoad]
public class OverridePrefabIconExtension : IHierarchyIconExtensionFeature {

	private static Texture2D iconTexture;

	static OverridePrefabIconExtension()
	{
		HierarchyIconExtensionList.AddExtension(new OverridePrefabIconExtension());
		iconTexture = AssetDatabase.LoadAssetAtPath(
			"Assets/HierarchyIconExtension/Editor/HierarchyIconExtension/Feature/Bob.png",
			typeof(Texture2D)
		) as Texture2D;
	}

	public int GetPriority()
	{
		return 1;
	}

	public Texture2D GetDisplayIcon(GameObject go)
	{
		var root = PrefabUtility.FindPrefabRoot(go);
		if (root != go) {
			return null;
		}

		bool isDisplay = hasOverrideProperty(go)
			|| hasOverrideTransformProperty(go);

		return isDisplay ? iconTexture : null;
	}

	private bool hasOverrideProperty(GameObject go)
	{
		var components = go.GetComponents<Component>();
		if (components.Count() <= 0) {
			return false;
		}

		//foreach is for avoiding crash to serialize ugui classes version_uity_4.6.3
		foreach (var component in components) {
			if (component is Transform || component == null) {
				continue;
			}
			var serializedObject = new SerializedObject(component);
			var iterator = serializedObject.GetIterator();
			while (iterator.Next(true)) {
				if (iterator.prefabOverride) {
					return true;
				}
			}
		}
		return false;
	}

	private bool hasOverrideTransformProperty(GameObject go)
	{
		var components = go.GetComponents<Transform>();
		if (components.Count() <= 0) {
			return false;
		}

		var serializedObject = new SerializedObject(components);
		var iterator = serializedObject.GetIterator();
		//these property is automatic overrided when instance was put on hierarchy
		string[] ignorePropertyNames = {
			"m_LocalRotation",
			"m_LocalPosition",
			"m_RootOrder",
			"m_AnchorMin",
			"m_AnchorMax",
			"m_AnchoredPosition",
			"m_SizeDelta",
			"m_Pivot",
		};
		bool isSkip = false;

		while (true) {
			bool isContinue = iterator.Next(!isSkip);
			if (!isContinue) {
				break;
			}

			isSkip = ignorePropertyNames.Contains(iterator.name);
			if (isSkip) {
				continue;
			}

			if (iterator.prefabOverride) {
				return true;
			}
		}
		return false;
	}
}
