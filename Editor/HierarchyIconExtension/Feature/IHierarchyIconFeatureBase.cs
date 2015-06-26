using UnityEngine;
using UnityEditor;
using System;

public interface IHierarchyIconExtensionFeature {

	int GetPriority();
	Texture2D GetDisplayIcon(GameObject go);
}
