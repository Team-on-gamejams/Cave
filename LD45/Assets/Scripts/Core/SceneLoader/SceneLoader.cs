using System.Collections;
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

		if(data["id"] is int)
			LoadScene((int)(data["id"]));
		else if (data["id"] is string)
			LoadScene((string)(data["id"]));
		else
			Debug.LogError("OnSceneNeedLoad [id] have unsupported type");
	}

	public void LoadScene(int id) {
		loader = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);

		EventData eventData = new EventData("OnSceneLoadStart");
		eventData.Data.Add("id", id);
		eventData.Data.Add("loader", loader);
		GameManager.Instance.EventManager.CallOnSceneLoadStart(eventData);

		loader.completed += (a) => {
			EventData eventData_ = new EventData("OnSceneLoadStart");
			eventData_.Data.Add("id", id);
			eventData_.Data.Add("loader", loader);
			GameManager.Instance.EventManager.CallOnSceneLoadEnd(eventData_);
		};
	}

	public void LoadScene(string name) {
		LoadScene(SceneUtility.GetBuildIndexByScenePath(name));
	}
}
