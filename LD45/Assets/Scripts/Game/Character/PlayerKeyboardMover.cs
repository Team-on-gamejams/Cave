using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardMover : MonoBehaviour {
	[SerializeField] Rigidbody2D rigidbody;
	[SerializeField] Player player;

	private void FixedUpdate() {
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		if(v != 0 || h != 0) {
			transform.localPosition += new Vector3(h, v).normalized * player.speed * Time.deltaTime;
		}
	}
}
