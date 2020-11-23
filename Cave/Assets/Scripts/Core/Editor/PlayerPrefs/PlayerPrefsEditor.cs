using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPrefsEditor : MonoBehaviour {
	[MenuItem("PlayerPrefs/Clear All")]
	static void ClearPlayerPrefs() {
		PlayerPrefs.DeleteAll();
	}
}
