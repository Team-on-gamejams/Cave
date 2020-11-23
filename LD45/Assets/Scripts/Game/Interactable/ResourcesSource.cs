using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSource : Interactable {
	public SpriteRenderer SpriteRenderer;

	public ItemSO.ItemType NeededHands;

	public GameObject ResourcePrefab;
	public byte ResourceCount;
	public float DropTime;
	public float DropDistance;
	public Vector3 ResourceDropPointСorrection;

	public byte NeededHits;
	public Sprite[] DamagedSprites;

	int CurrentHit;

	protected override void Awake() {
		base.Awake();
		CurrentHit = 0;

		OnMouseClick += Interract;
	}

	protected override void Start() {
		base.Start();
		Chunk chunk = WorldGenerator.instance.GetChunkFromWorldPos(transform.position);
		transform.parent = chunk.transform;
		chunk.ResourcesSources.Add(this);
	}

	private void OnDestroy() {
		WorldGenerator.instance.GetChunkFromWorldPos(transform.position).ResourcesSources.Remove(this);
	}

	public override bool CanInteract() {
		return ((isInteractLMB ? GameManager.Instance.Player.Equipment.handLeft : GameManager.Instance.Player.Equipment.handRight)?.Type ?? ItemSO.ItemType.None) == NeededHands;
	}

	//TODO: hit particles
	//TODO: visially show that source damaged
	void Interract() {
		GameManager.Instance.Player.Equipment.OnUseHandAnimEndEvent += HitSource;
		GameManager.Instance.Player.Equipment.PlayUseHandItemAnim(isInteractLMB);
	}

	void HitSource(){
		GameManager.Instance.Player.Equipment.GOLinkedAnim = null;

		if (++CurrentHit == NeededHits) {
			while (ResourceCount-- != 0) {
				GameObject res = Instantiate(ResourcePrefab, transform.position + ResourceDropPointСorrection, Quaternion.identity);
                LeanTween.move(res, res.transform.position + new Vector3(Random.Range(-DropDistance, DropDistance), Random.Range(-DropDistance, DropDistance), 0), DropTime)
					.setOnComplete(res.GetComponent<Interactable>().RecalcInteractPos);
			}
			Destroy(gameObject);
		}
		else {
			if(CurrentHit < DamagedSprites.Length && DamagedSprites[CurrentHit] != null)
				SpriteRenderer.sprite = DamagedSprites[CurrentHit];
		}
	}
}
