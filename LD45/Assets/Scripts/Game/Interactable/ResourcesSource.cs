using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSource : Interactable {
	public ItemSO.ItemType NeededHands;
	public GameObject ResourcePrefab;
	public byte ResourceCount;
	public float DropTime;
	public float DropDistance;
	public Vector3 ResourceDropPointСorrection;
	public byte NeededHits;

	int CurrentHit;

	protected override void Awake() {
		base.Awake();
		CurrentHit = 0;

		OnMouseClick += TryInterract;
	}

	public bool IsSuitableHand() {
		return (GameManager.Instance.Player.Equipment.hands?.Type ?? ItemSO.ItemType.None) == NeededHands;
	}

	//TODO: hit particles
	//TODO: visially show that source damaged
	void TryInterract() {
		if (!IsSuitableHand())
			return;

		GameManager.Instance.Player.Equipment.OnUseHandAnimEndEvent += HitSource;
		GameManager.Instance.Player.Equipment.PlayUseHandItemAnim();
	}

	void HitSource(){
		GameManager.Instance.Player.Equipment.GOLinkedAnim = null;

		if (++CurrentHit == NeededHits) {
			while (ResourceCount-- != 0) {
				GameObject res = Instantiate(ResourcePrefab, transform.position + ResourceDropPointСorrection, Quaternion.identity, GameManager.Instance.CollectorItems.transform);
                LeanTween.moveLocal(res, res.transform.position + new Vector3(Random.Range(-DropDistance, DropDistance), Random.Range(-DropDistance, DropDistance), 0), DropTime)
					.setOnComplete(res.GetComponent<Interactable>().RecalcInteractPos);
			}
			Destroy(gameObject);
		}
	}
}
