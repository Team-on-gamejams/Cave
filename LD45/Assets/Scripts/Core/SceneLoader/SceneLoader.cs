﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {
	AsyncOperation loader;

	void Awake() {
		EventManager.OnSceneNeedLoad += OnSceneNeedLoad;
	}

	new void OnDestroy() {
		EventManager.OnSceneNeedLoad -= OnSceneNeedLoad;

		base.OnDestroy();
	}

	void OnSceneNeedLoad(EventData data) {
		if (!data.Data.ContainsKey("id")) {
			Debug.LogError("OnSceneNeedLoad no [id] field");
			return;
		}

		int sceneId = -1;
		bool needUI = false, uiNeedDelay = false;

		if (data["id"] is int) {
			sceneId = (int)(data["id"]);
		}
		else if (data["id"] is string) {
			sceneId = SceneUtility.GetBuildIndexByScenePath((string)(data["id"]));
		}
		else {
			Debug.LogError("OnSceneNeedLoad [id] have unsupported type");
			return;
		}

		LoadScene(sceneId, needUI, uiNeedDelay);
	}

	public void LoadScene(string name, bool needUI, bool uiNeedDelay) {
		LoadScene(SceneUtility.GetBuildIndexByScenePath(name), needUI, uiNeedDelay);
	}

	public void LoadScene(int id, bool needUI, bool uiNeedDelay) {
		loader = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);

		EventData eventData = new EventData("OnSceneLoadStart");
		eventData.Data.Add("id", id);
		eventData.Data.Add("loader", loader);
		eventData.Data.Add("needUI", needUI);
		if(needUI)
			eventData.Data.Add("uiNeedDelay", uiNeedDelay);
		GameManager.Instance.EventManager.CallOnSceneLoadStart(eventData);

		loader.completed += (a) => {
			EventData eventData_ = new EventData("OnSceneLoadStart");
			eventData_.Data.Add("id", id);
			eventData_.Data.Add("loader", loader);
			GameManager.Instance.EventManager.CallOnSceneLoadEnd(eventData_);
		};
	}
}
