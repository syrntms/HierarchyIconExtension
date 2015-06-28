using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[InitializeOnLoad]
public class HierarchyIconExtensionMenu
{
	public enum ExtensionState {
		Idle = 0,
		Work = 1,
	};

	#region preference menu
	private static bool isLoaded = false;
	private static Dictionary<string, ExtensionState> extensionToState = new Dictionary<string, ExtensionState>();
	private static Dictionary<string, ExtensionState> changedStateList = new Dictionary<string, ExtensionState>();
	
	[PreferenceItem("HierarhcyIcon")]
	static void Menu()
	{
		if (isLoaded == false) {
			initialLoadExtensionState();
			isLoaded = true;
		}
		
		EditorGUI.BeginChangeCheck();
		changedStateList.Clear();

		foreach (var pair in extensionToState) {
			var retValue = EditorGUILayout.Toggle(
				pair.Key.ToString(),
				pair.Value == ExtensionState.Work
			);

			var stateInt = (retValue) ? 1 : 0;
			changedStateList.Add(pair.Key, (ExtensionState)stateInt);
		}

		foreach (var changedState in changedStateList) {
			extensionToState[changedState.Key] = changedState.Value;
		}

		if (EditorGUI.EndChangeCheck()) {    
			saveExtensionState();
			HierarchyIconExtensionList.UpdateWorkingExtensions();
		}
	}

	private static void initialLoadExtensionState()
	{
		extensionToState.Clear();
		foreach (var extension in HierarchyIconExtensionList.GetAllExtensions()) {
			var extensionName = extension.GetType().ToString();
			var stateString = EditorUserSettings.GetConfigValue(extensionName) ?? ((int)ExtensionState.Idle).ToString();
			var state = (ExtensionState)int.Parse(stateString);
			extensionToState.Add(extensionName, state);
		}
	}

	private static void saveExtensionState()
	{
		foreach (var pair in extensionToState) {
			EditorUserSettings.SetConfigValue(
				pair.Key.ToString(),
				((int)pair.Value).ToString()
			);
		}
	}
	#endregion
}
