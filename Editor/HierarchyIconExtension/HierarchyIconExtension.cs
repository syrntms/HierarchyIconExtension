using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[InitializeOnLoad]
public class HierarchyIconExtension {

	public static List<HierarchyIconFeatureBase> Extensions = new List<HierarchyIconFeatureBase>();
	private static Dictionary<int, HierarchyGameObjectIconData> instanceIdToIconData = new Dictionary<int, HierarchyGameObjectIconData>();

	static HierarchyIconExtension()
	{
		EditorApplication.update += onUpdateHierarchy;
		EditorApplication.hierarchyWindowItemOnGUI += onDrawHierarchyGameObject;
	}

	private static void onUpdateHierarchy()
	{
		GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>() as GameObject[];
		IEnumerable<HierarchyIconFeatureBase> sortedList = Extensions.OrderBy(ext => ext.GetPriority());
		instanceIdToIconData.Clear();

		foreach (var extension in sortedList) {
			foreach (var go in gameObjects) {
				bool isDisplay = extension.IsDisplayIcon(go);
				if (!isDisplay) {
					continue;
				}
				HierarchyGameObjectIconData data;
				bool isExist = instanceIdToIconData.TryGetValue(go.GetInstanceID(), out data);
				if (!isExist) {
					data = new HierarchyGameObjectIconData();
				}

				data.IconList.Add(extension.GetIconTexture());
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
		r.x = r.width;

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
