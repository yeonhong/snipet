using System.Collections.Generic;
using UnityEngine;

namespace Grid2D
{
	[RequireComponent(typeof(MeshRenderer))]
	public class GridMap : MonoBehaviour
	{
		public static GridMap Instance { get; private set; }

		public Vector2 gridSize = new Vector2(1, 1);
		private Vector2 gridOffset = default;
		private Vector3 lastGridSize = default;
		private Vector3 lastPosition = default;

		#region occupiedPositions
		// 자리를 차지한 정보를 리스트로 관리.
		private List<Vector4> occupiedPositions = new List<Vector4>();

		public void AddPosition(Vector4 pos) {
			if (!occupiedPositions.Contains(pos)) {
				occupiedPositions.Add(pos);
			}
		}

		public void RemovePosition(Vector4 pos) {
			if (occupiedPositions.Contains(pos)) {
				occupiedPositions.Remove(pos);
			}
		}

		public bool IsOccupied(Vector4 targetPos) {
			foreach (Vector4 pos in occupiedPositions) {
				if (((targetPos.x >= pos.x && targetPos.x <= pos.y) || (targetPos.y >= pos.x && targetPos.y <= pos.y) || (pos.y >= targetPos.x && pos.y <= targetPos.y))
					&& ((targetPos.z >= pos.z && targetPos.z <= pos.w) || (targetPos.w >= pos.z && targetPos.w <= pos.w) || (pos.w >= targetPos.z && pos.w <= targetPos.w))) {
					return true;
				}
			}
			return false;
		}
		#endregion

		private void Awake() {
			Instance = this;
			UpdateScale();
		}

		private void Update() {
			// If the transform has changed
			if (transform.hasChanged) {
				RefreshGridData();
			}

			// If grid size has changed
			if ((gridSize.x != lastGridSize.x) || (gridSize.y != lastGridSize.y)) {
				lastGridSize = gridSize;
				UpdateScale();
			}
		}

		// Update transform and texture size
		private void UpdateScale() {
			transform.localScale = new Vector3(gridSize.x, gridSize.y, 1);
			GetComponent<Renderer>().material.mainTextureScale = gridSize;
		}

		private void RefreshGridData() {
			// Update the grid texture size 
			// and fix positions of the draggable gameobjects
			transform.hasChanged = false;
			occupiedPositions.Clear();
			GetComponent<Renderer>().material.mainTextureScale = gridSize;
			FixPositions();
		}

		// Fix draggable gameobjects positions
		private void FixPositions() {
			var objs = FindObjectsOfType<DragAndDrop>();
			var diff = transform.localPosition - lastPosition;
			foreach (DragAndDrop i in objs) {
				i.FixPosition(diff);
				i.UpdateAll();
			}
			lastPosition = transform.localPosition;
		}

		public Vector2 GetGridOffset() {
			gridOffset.x = transform.localPosition.x;
			gridOffset.y = transform.localPosition.y;
			return gridOffset;
		}
	}
}
