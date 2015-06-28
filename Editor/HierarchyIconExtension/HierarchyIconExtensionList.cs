using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[InitializeOnLoad]
public class HierarchyIconExtensionList
{
	private static List<IHierarchyIconExtensionFeature> extensions = new List<IHierarchyIconExtensionFeature>();
	public static List<IHierarchyIconExtensionFeature> WorkingExtensions = new List<IHierarchyIconExtensionFeature>();

	public static IEnumerable<IHierarchyIconExtensionFeature> GetAllExtensions()
	{
		return extensions;
	}
	public static void AddExtension(IHierarchyIconExtensionFeature feature)
	{
		bool isExist = extensions.Any(ext => feature.GetType() == ext.GetType());
		if (isExist) {
			return;
		}
		extensions.Add(feature);
		extensions = extensions.OrderBy(ext => ext.GetPriority()).ToList();
		UpdateWorkingExtensions();
	}

	public static void RemoveExtension(IHierarchyIconExtensionFeature feature)
	{
		for (int i = 0 ; i < extensions.Count ; ++i) {
			bool isSameType = extensions[i].GetType() == feature.GetType();
			if (!isSameType) {
				continue;
			}
			extensions.RemoveAt(i);
			break;
		}
		UpdateWorkingExtensions();
	}

	public static void UpdateWorkingExtensions()
	{
		WorkingExtensions.Clear();
		foreach (var extension in extensions) {
			var stateString = EditorUserSettings.GetConfigValue(extension.GetType().ToString())
				?? ((int)HierarchyIconExtensionMenu.ExtensionState.Idle).ToString();
			var state = (HierarchyIconExtensionMenu.ExtensionState)int.Parse(stateString);
			switch (state) {
			case HierarchyIconExtensionMenu.ExtensionState.Idle:
				break;
			case HierarchyIconExtensionMenu.ExtensionState.Work:
				WorkingExtensions.Add(extension);
				break;
			default:
				break;
			}
		}
	}
}
