using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SpriteOrderSorter : MonoBehaviour {
	public enum SortingOriginEnum : byte { Pivot, Bottom, Top, Left, Right }
	public enum SortingAxisEnum : byte { X, Y }

	[SerializeField] float Precision = 10;
	[SerializeField] int Offset = 0;
	[SerializeField] SortingOriginEnum SortingOrigin = SortingOriginEnum.Bottom;
	[SerializeField] SortingAxisEnum SortingAxis = SortingAxisEnum.Y;

	protected SpriteRenderer renderer;

	protected virtual void Awake() {
		renderer = GetComponent<SpriteRenderer>();
	}

	protected void UpdateSortOrder() {
		Vector2 pos = renderer.bounds.center;

		switch (SortingOrigin) {
			case SortingOriginEnum.Bottom:
				pos += Vector2.down * renderer.bounds.size.y / 2 /** GetLowestPixel(renderer.sprite.texture)*/;
				break;
			case SortingOriginEnum.Top:
				pos += Vector2.up * renderer.bounds.size.y / 2;
				break;
			case SortingOriginEnum.Left:
				pos += Vector2.left * renderer.bounds.size.x / 2;
				break;
			case SortingOriginEnum.Right:
				pos += Vector2.right * renderer.bounds.size.x / 2;
				break;
			default:
				pos = transform.position;
				break;
		}

		float posFromAxis = SortingAxis == SortingAxisEnum.X ? pos.x : -pos.y;
		renderer.sortingOrder = (int)(posFromAxis * Precision + Offset);
	}

	//float GetLowestPixel(Texture2D tex) {
	//	for (int x = 0; x < tex.width; ++x)
	//		for (int y = tex.height; y >= 0; --y)
	//			if (tex.GetPixel(x, y).a != 0)
	//				return ((float)y) / tex.height;
	//	return 0.0f;
	//}
}