using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSource : MonoBehaviour {
	public ItemSO.ItemType NeededHands;
	public GameObject ResourcePrefab;
	public int ResourceCount;
	public float DropTime;
	public float DropDistance;
	public int NeededClick;
	int CurrentClick;

	SpriteOutline outline;

	private void Awake() {
		CurrentClick = 0;
	}

	private void OnMouseEnter() {
		outline = gameObject.AddComponent<SpriteOutline>();
		outline._outlineSize = 5.0f;
	}

	private void OnMouseExit() {
		Destroy(outline);
	}

	void OnMouseDown() {
		if (!CanClick())
			return;

		if (++CurrentClick == NeededClick){

			while(ResourceCount-- != 0) {
				GameObject res = Instantiate(ResourcePrefab, transform.position, Quaternion.identity);
				LeanTween.moveLocal(res, res.transform.position + new Vector3(Random.Range(-DropDistance, DropDistance), Random.Range(-DropDistance, DropDistance), 0), DropTime);
			}

			Destroy(gameObject);
		}
	}

	bool CanClick() {
		//TODO: remove when hotbar will be ready
		return true;
		return GameManager.Instance.Player.Equipment.hands.Type == NeededHands;
	}
}
