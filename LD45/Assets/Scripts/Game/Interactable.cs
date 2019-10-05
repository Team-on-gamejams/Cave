using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	public float InteractDist;
	float InteractDistSqr;

	protected virtual void Awake() {
		InteractDistSqr = InteractDist * InteractDist;
	}

	public bool CanInteract() {
		return GameManager.Instance.Player.CanInteract(transform.position, InteractDistSqr);
	}
}
