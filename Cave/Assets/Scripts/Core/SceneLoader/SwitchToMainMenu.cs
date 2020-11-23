using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToMainMenu : MonoBehaviour {
	void Start() {
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}
