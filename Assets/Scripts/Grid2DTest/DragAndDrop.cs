using UnityEngine;
namespace Grid2D
{
	public class DragAndDrop : MonoBehaviour
	{
		[Header("Restrictions")]
		public bool considerScale = true;
		public bool considerOtherObjects = true;

		[Space(5)]

		public Vector2 SnapSize = new Vector2(1, 1);
		public Vector4 currentPosition = new Vector4(1, 1, 1, 1);
		private Vector2 gridOffset = Vector2.zero;
		private Vector2 gridSize = Vector2.one;
		private Vector3 screenPoint = default;
		private Vector4 lastPos = default;
		private Vector3 lastParentPos = default;
		private Vector4 targetPos = default;

		private void Awake() {

			// Fix the position according to the scale of this object
			var newPos = transform.localPosition;
			newPos.x = (transform.localScale.x / 2f) - 0.5f;
			newPos.y = -((transform.localScale.y / 2f) - 0.5f);

			transform.localPosition = newPos;

			// Update Data
			UpdateGridData();
			UpdatePosition();

			// Save actual position
			lastParentPos = transform.parent.position;
			lastPos = currentPosition;

			// Add position
			AddPosition(lastPos);
		}

		// Get recent values of the Grid
		private void UpdateGridData() {
			gridSize = GridMap.Instance.gridSize;
			gridOffset = GridMap.Instance.GetGridOffset();
		}

		public void UpdateAll() {
			UpdatePosition();
			AddPosition(lastPos);
		}

		#region Drag 관련
		private void OnMouseDown() {
			// Remove the last position
			RemovePosition(lastPos);

			// Update data
			UpdateGridData();

			// Correct the position according to the scale of this object
			var newPos = transform.localPosition;
			newPos.x = (transform.localScale.x / 2f) - 0.5f;
			newPos.y = -((transform.localScale.y / 2f) - 0.5f);

			transform.localPosition = newPos;

			UpdatePosition();
		}

		private void OnMouseDrag() {

			// Get World Point using the Mouse Position
			screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			screenPoint.z = 0;

			// Fix the requested position if the number of the cells is even
			if (gridSize.x % 2 == 0) {
				screenPoint.x -= 0.5f;
			}
			if (gridSize.y % 2 == 0) {
				screenPoint.y -= 0.5f;
			}

			// Change GameObject position
			transform.parent.position = SnapToGrid(screenPoint);
		}

		private void OnMouseUp() {
			UpdateGridData();

			// Save Target Pos
			targetPos = GetCurrentPos();

			// Check if it is occupying the position of another object
			if (considerOtherObjects) {

				// If it is not busy
				if (!IsOccupied()) {
					// The last saved position is removed
					RemovePosition(lastPos);

					// And the new position is added
					UpdatePosition();
					AddPosition(targetPos);

				} else { // If busy, add the saved position again
					AddPosition(lastPos);
				}
			} else {
				UpdatePosition();
				AddPosition(targetPos);
			}
		} 
		#endregion

		private Vector4 GetCurrentPos() {
			var parentPos = transform.parent.position;
			Vector4 ret;
			ret.x = (parentPos.x + (gridSize.x * 0.5f) + 0.5f);
			ret.y = (parentPos.x + (gridSize.x * 0.5f) + 0.5f) + transform.localScale.x - 1;
			ret.z = -(parentPos.y - (gridSize.y * 0.5f) - 0.5f);
			ret.w = -(parentPos.y - (gridSize.y * 0.5f) - 0.5f) + transform.localScale.y - 1;
			return ret;
		}

		private void AddPosition(Vector4 pos) {
			GridMap.Instance.AddPosition(pos);
		}

		private void RemovePosition(Vector4 pos) {
			GridMap.Instance.RemovePosition(pos);
		}

		private bool IsOccupied() {
			if (GridMap.Instance.IsOccupied(targetPos)) {
				transform.parent.position = lastParentPos;
				currentPosition = lastPos;
				return true;
			}

			return false;
		}

		// Update object position variable
		private void UpdatePosition() {
			currentPosition = GetCurrentPos();
			
			// Save actual position
			lastParentPos = transform.parent.position;
			lastPos = currentPosition;
		}

		// Fix the GameObject position if the Grid Transform has changed
		public void FixPosition(Vector3 newPos) {
			newPos.z = 0;
			transform.parent.position = transform.parent.position + newPos;

			UpdateGridData();
			UpdatePosition();
		}

		// Function that allows you to move an object according to the Grid
		private Vector3 SnapToGrid(Vector3 dragPos) {
			// If X is even, fix the target position
			if (gridSize.x % 2 == 0) {
				dragPos.x = (Mathf.Round(dragPos.x / SnapSize.x) * SnapSize.x) + 0.5f;
			}
			else {
				dragPos.x = (Mathf.Round(dragPos.x / SnapSize.x) * SnapSize.x);
			}

			// If Y is even, fix the target position
			if (gridSize.y % 2 == 0) {
				dragPos.y = (Mathf.Round(dragPos.y / SnapSize.y) * SnapSize.y) + 0.5f;
			}
			else {
				dragPos.y = (Mathf.Round(dragPos.y / SnapSize.y) * SnapSize.y);
			}

			#region Restrictions

			// Restrict exit from grid
			var maxXPos = ((gridSize.x - 1) * 0.5f) + gridOffset.x;
			var maxYPos = ((gridSize.y - 1) * 0.5f) + gridOffset.y;

			// Considering GameObject Scale
			if (considerScale) {

				if (dragPos.x > maxXPos - transform.localScale.x + 1) {
					dragPos.x = maxXPos - transform.localScale.x + 1;
				}

				if (dragPos.x < -maxXPos + gridOffset.x + gridOffset.x) {
					dragPos.x = -maxXPos + gridOffset.x + gridOffset.x;
				}

				if (dragPos.y > maxYPos) {
					dragPos.y = maxYPos;
				}

				if (dragPos.y < (-maxYPos + gridOffset.y + gridOffset.y) + transform.localScale.y - 1) {
					dragPos.y = -maxYPos + gridOffset.y + gridOffset.y + transform.localScale.y - 1;
				}
			}

			else {

				if (dragPos.x > maxXPos) {
					dragPos.x = maxXPos;
				}

				if (dragPos.x < -maxXPos + gridOffset.x + gridOffset.x) {
					dragPos.x = -maxXPos + gridOffset.x + gridOffset.x;
				}

				if (dragPos.y > maxYPos) {
					dragPos.y = maxYPos;
				}

				if (dragPos.y < -maxYPos + gridOffset.y + gridOffset.y) {
					dragPos.y = -maxYPos + gridOffset.y + gridOffset.y;
				}
			}

			#endregion

			return dragPos;
		}
	}

}