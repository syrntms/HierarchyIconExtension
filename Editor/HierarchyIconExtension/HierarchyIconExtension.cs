using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[InitializeOnLoad]
public class HierarchyIconExtension
{
	static HierarchyIconExtension()
	{
		EditorApplication.hierarchyWindowItemOnGUI += onDrawHierarchyGameObject;
	}

	private static void onDrawHierarchyGameObject(int instanceID, Rect selectionRect)
	{
		Rect r = new Rect(selectionRect);
		r.x = r.xMax;
		foreach (var extension in HierarchyIconExtensionList.WorkingExtensions) {
			var iconTexture = extension.GetDisplayIcon(instanceID);
			if (iconTexture == null) {
				continue;
			}

			r.x -= 18;
			r.width = 20;
			GUI.Label (r, iconTexture); 
		}
	}
}
