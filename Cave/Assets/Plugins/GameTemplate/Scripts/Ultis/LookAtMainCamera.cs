﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour {
	void Update() {
		transform.rotation = (TemplateGameManager.Instance.Camera.transform.rotation);
	}
}
