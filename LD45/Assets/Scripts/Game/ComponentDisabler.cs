using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDisabler : MonoBehaviour {
	[SerializeField] MonoBehaviour Component;
	[SerializeField] bool Destroy;


	void Awake() {
		if(Destroy)
			Destroy(Component);
		Destroy(this);
	}
}
