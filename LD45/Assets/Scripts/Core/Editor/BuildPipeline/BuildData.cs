using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class BuildData : ScriptableObject {
	public static BuildData GetDefault() {
		string stringName = "com.myproject.myconfigdata";
		string stringPath = "Assets/myconfigdata.asset";
		BuildData data = null;

		if (EditorBuildSettings.TryGetConfigObject<BuildData>(stringName, out data))
			return data;

		if (File.Exists(stringPath))
			data = AssetDatabase.LoadAssetAtPath<BuildData>(stringPath);

		if (data == null) {
			stringPath = EditorUtility.SaveFilePanelInProject("New Config File", "myconfigdata", "asset", "Select Config File Asset", "Assets");
			data = ScriptableObject.CreateInstance<BuildData>();
			AssetDatabase.CreateAsset(data, stringPath);
		}
		EditorBuildSettings.AddConfigObject(stringName, data, false);

		return data;
	}
}
