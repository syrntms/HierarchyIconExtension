using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public interface IHierarchyIconExtensionFeature
{
	int GetPriority();
	Texture2D GetDisplayIcon(int instanceID);
}
