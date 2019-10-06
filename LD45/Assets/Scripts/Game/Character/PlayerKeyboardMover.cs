using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardMover : MonoBehaviour {
	public System.Action OnMouseMoveEnd;

	[SerializeField] Rigidbody2D rigidbody;
	[SerializeField] Player player;

	Vector3 moveToDirection;
	Vector3 moveToPoint;

	void Awake() {
		moveToPoint = Vector3.zero;
	}

	void FixedUpdate() {
		//TODO: початок руху з прискоренням вплоть до макс. швидкості. Зупинка моментальна
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		if(v != 0 || h != 0) {
			if (moveToPoint != Vector3.zero) {
				moveToPoint = Vector3.zero;
				OnMouseMoveEnd = null;
			}
			rigidbody.MovePosition(transform.localPosition + new Vector3(h, v).normalized * player.speed * Time.deltaTime);
		}

		if (Input.GetMouseButton(0)) {
			moveToPoint = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
			moveToPoint.z = 0;
			moveToDirection = moveToPoint - transform.position;
		}


		if (moveToPoint != Vector3.zero) {
			if ((transform.position - moveToPoint).sqrMagnitude > GameManager.Instance.Player.InteractDistSqr) {
				rigidbody.MovePosition(transform.localPosition + moveToDirection.normalized * player.speed * Time.deltaTime);
			}
			else {
				moveToPoint = Vector3.zero;
				if(OnMouseMoveEnd != null) {
					OnMouseMoveEnd.Invoke();
					OnMouseMoveEnd = null;
				}
			}
		}
	}
}
