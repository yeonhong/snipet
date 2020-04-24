using System.Collections.Generic;
using UnityEngine;

namespace Grid2D
{
	[RequireComponent(typeof(MeshRenderer))]
	public class Grid : MonoBehaviour
	{
		public Vector2 gridSize = new Vector2(1, 1);
		private Vector2 gridOffset;
		private Vector3 lastGridSize;
		private Vector3 lastPosition;

		public List<Vector4> occupiedPositions;

		private void Awake() {
			UpdateScale();
		}

		private void Update() {

			// If the transform has changed
			if (transform.hasChanged) {

				// Update the grid texture size and fix positions of the draggable gameobjects
				transform.hasChanged = false;
				occupiedPositions.Clear();
				GetComponent<Renderer>().material.mainTextureScale = gridSize;
				FixPositions();

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
