using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLoadScene : MonoBehaviour {
	public void CallSceneLoad(string name) {
		//EventData eventData = new EventData("OnSceneNeedLoad");
		//eventData.Data.Add("id", name);
		//GameManager.Instance.EventManager.CallOnSceneNeedLoad(eventData);
		SceneLoader.Instance.LoadScene(name);
	}

	public void CallSceneLoad(int buildId) {
		//EventData eventData = new EventData("OnSceneNeedLoad");
		//eventData.Data.Add("id", buildId);
		//GameManager.Instance.EventManager.CallOnSceneNeedLoad(eventData);
		SceneLoader.Instance.LoadScene(buildId);
	}
}
