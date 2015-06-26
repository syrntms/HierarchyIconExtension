using UnityEngine;
using UnityEditor;
using System;

public class HierarchyIconFeatureBase {

	public virtual int GetPriority()
	{
		return 0;
	}

	public virtual Texture2D GetIconTexture()
	{
		return null;
	}

	public virtual bool IsDisplayIcon(GameObject go)
	{
		return false;
	}

}
