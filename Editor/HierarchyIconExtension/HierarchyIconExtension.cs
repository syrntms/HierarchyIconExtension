using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[InitializeOnLoad]
public class HierarchyIconExtension
{

	private static Dictionary<int, HierarchyGameObjectIconData> instanceIdToIconData = new Dictionary<int, HierarchyGameObjectIconData>();

	static HierarchyIconExtension()
	{
		EditorApplication.update += onUpdateHierarchy;
		EditorApplication.hierarchyWindowItemOnGUI += onDrawHierarchyGameObject;
	}

	private static void onUpdateHierarchy()
	{
		GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>() as GameObject[];
		instanceIdToIconData.Clear();

		foreach (var extension in HierarchyIconExtensionList.WorkingExtensions) {
			foreach (var go in gameObjects) {
				var iconTexture = extension.GetDisplayIcon(go);
				if (iconTexture == null) {
					continue;
				}

				HierarchyGameObjectIconData data;
				bool isExist = instanceIdToIconData.TryGetValue(go.GetInstanceID(), out data);
				if (!isExist) {
					data = new HierarchyGameObjectIconData();
				}

				data.IconList.Add(iconTexture);
				instanceIdToIconData[go.GetInstanceID()] = data;
			}
		}
	}

	private static void onDrawHierarchyGameObject(int instanceID, Rect selectionRect)
	{
		HierarchyGameObjectIconData data;
		bool isExist = instanceIdToIconData.TryGetValue(instanceID, out data);
		if (!isExist) {
			return;
		}

		Rect r = new Rect(selectionRect); 
		r.x = r.xMax;

		foreach (var icon in data.IconList) {
			r.x -= 18;
			r.width = 20;
			GUI.Label (r, icon); 
		}
	}
}

public class HierarchyGameObjectIconData
{
	public List<Texture2D> IconList = new List<Texture2D>();
}
