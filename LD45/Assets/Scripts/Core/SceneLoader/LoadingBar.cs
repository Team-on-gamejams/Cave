using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {
	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] Image loadingBar;

	int sceneId;
	AsyncOperation loader;
	Coroutine loadingBarRoutine;

	void Awake() {
		DisableCanvasGroup();

		EventManager.OnSceneLoadStart += OnSceneLoadStart;
		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;
	}

	void OnDestroy() {
		EventManager.OnSceneLoadStart -= OnSceneLoadStart;
		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;
	}

	void OnSceneLoadStart(EventData data) {
		if(loader != null) {
			Debug.LogError("Cant display LoadingBar for 2 scenes");
			return;
		}

		sceneId = (int)(data.Data?["id"] ?? -1);
		loader = data.Data?["loader"] as AsyncOperation;

		if(sceneId == -1 || loader == null) {
			Debug.LogError("LoadingBar data does not contains all necessary arguments");
			return;
		}

		loadingBarRoutine = StartCoroutine(LoadingBarUpdate());
		EnableCanvasGroup();
	}

	void OnSceneLoadEnd(EventData data) {
		StopCoroutine(loadingBarRoutine);
		DisableCanvasGroup();

		sceneId = -1;
		loader = null;
		loadingBarRoutine = null;
	}

	IEnumerator LoadingBarUpdate() {
		while(!loader.isDone) {
			loadingBar.fillAmount = loader.progress / 0.9f;

			yield return null;
		}
	}

	void DisableCanvasGroup() {
		canvasGroup.alpha = 0.0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	void EnableCanvasGroup() {
		canvasGroup.alpha = 1.0f;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}
}
