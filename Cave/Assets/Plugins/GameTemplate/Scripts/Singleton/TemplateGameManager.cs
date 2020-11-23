﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using yaSingleton;
using Cinemachine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Template Game Manager", menuName = "Singletons/TemplateGameManager")]
public class TemplateGameManager : Singleton<TemplateGameManager> {
	//Properties
	public Camera Camera { get {
			if(mainCamera == null) 
				mainCamera = Camera.main;
			return mainCamera;
	}}
	public CinemachineVirtualCamera VirtualCamera {
		get {
			if (virtualCamera == null)
				virtualCamera = Camera.GetComponent<CinemachineVirtualCamera>();
			return virtualCamera;
		}
	}

	//Global data
	[ReadOnly] public string buildNameString;
	[ReadOnly] public string productName;

	//UI
	[ReadOnly] public UIInput uiinput;
	[ReadOnly] public EventSystem eventSystem;
	[ReadOnly] public InputSystemUIInputModule inputSystem;

	//Debug UI
	[ReadOnly] public UIPopupGroup debugPopups;

	//Other singletons
	public EventManager Events { get; private set; }
	public AudioManager audioManager;
	public SceneLoader sceneLoader;

	Camera mainCamera;
	CinemachineVirtualCamera virtualCamera;

	protected override void Initialize() {
		Debug.Log("GameManager.Initialize()");
		base.Initialize();

		Events = new EventManager();

		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;

		StartCoroutine(DelayedSetup());

		IEnumerator DelayedSetup() {
			yield return null;
			yield return null;
			Events.CallOnOnApplicationStart();
			Events.CallOnSceneLoadEnd(null);
		}
	}

	protected override void Deinitialize() {
		Debug.Log("GameManager.Deinitialize()");
		base.Deinitialize();

		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
		Events.CallOnOnApplicationExit();
	}

	void OnSceneLoadEnd(EventData data) {
		mainCamera = Camera.main;
	}
}

#if UNITY_EDITOR

[UnityEditor.InitializeOnLoad]
class SingletoneImporter {
	static UnityEngine.Object[] assets = null;
	static UnityEngine.Object selectedBefore = null;
	static int i = 0;
	static bool isSubscribeBefore = false;

	static SingletoneImporter() {
		if (!isSubscribeBefore && UnityEditor.EditorApplication.timeSinceStartup < 30f) {
			isSubscribeBefore = true;
			UnityEditor.EditorApplication.update += Update;
		}
	}

	static void Update() {
		ImportSingletons();
	}

	static void ImportSingletons() {
		if(assets == null) {
			Debug.Log("Init singletons");
			
			string[] guids = UnityEditor.AssetDatabase.FindAssets("t:scriptableobject", new[] { "Assets/ScriptableObjects/Singletons" });
			assets = new UnityEngine.Object[guids.Length];

			for(int i = 0; i < assets.Length; ++i) {
				assets[i] = UnityEditor.AssetDatabase.LoadMainAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]));
			}

			selectedBefore = UnityEditor.Selection.activeObject;
		}

		if(i == assets.Length) {
			UnityEditor.EditorApplication.update -= Update;
			UnityEditor.Selection.activeObject = selectedBefore;
		}
		else {
			UnityEditor.Selection.activeObject = assets[i++];
		}
	}
}

#endif
