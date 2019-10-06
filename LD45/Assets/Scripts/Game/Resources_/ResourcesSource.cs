using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSource : Interactable {
	public ItemSO.ItemType NeededHands;
	public GameObject ResourcePrefab;
	public int ResourceCount;
	public float DropTime;
	public float DropDistance;
	public Vector3 ResourceDropPointСorrection;
	public int NeededClick;

	int CurrentClick;

	protected override void Awake() {
		base.Awake();
		CurrentClick = 0;

		OnMouseClick += HitSource;
	}

	void HitSource() {
		if (!IsSuitableHand())
			return;

		if (++CurrentClick == NeededClick){

			while(ResourceCount-- != 0) {
				GameObject res = Instantiate(ResourcePrefab, transform.position + ResourceDropPointСorrection, Quaternion.identity);
				LeanTween.moveLocal(res, res.transform.position + new Vector3(Random.Range(-DropDistance, DropDistance), Random.Range(-DropDistance, DropDistance), 0), DropTime);
			}

			Destroy(gameObject);
		}
	}

	bool IsSuitableHand() {
		//TODO: remove when hotbar will be ready
		return true;
		return GameManager.Instance.Player.Equipment.hands.Type == NeededHands;
	}
}
