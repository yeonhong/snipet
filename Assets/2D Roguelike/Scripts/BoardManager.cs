using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

// todo : board manager 리펙토링
namespace Roguelike2D
{
	public class CreateEnemyEvent : UnityEvent<GameObject> { }

	public class BoardManager : MonoBehaviour
	{
		[Serializable]
		public class Count
		{
			public int minimum;
			public int maximum;

			public Count(int min, int max) {
				minimum = min;
				maximum = max;
			}

			public Count(int value) {
				minimum = value;
				maximum = value;
			}
		}

		[SerializeField] private int columns = 8;
		[SerializeField] private int rows = 8;
		[SerializeField] private Count wallCount = new Count(5, 9);
		[SerializeField] private Count foodCount = new Count(1, 5);
		[SerializeField] private GameObject exit = null;
		[SerializeField] private GameObject[] floorTiles = null;
		[SerializeField] private GameObject[] wallTiles = null;
		[SerializeField] private GameObject[] foodTiles = null;
		[SerializeField] private GameObject[] enemyTiles = null;
		[SerializeField] private GameObject[] outerWallTiles = null;

		public CreateEnemyEvent OnEnemyCreated = new CreateEnemyEvent();

		private List<Vector3> _gridPositions = new List<Vector3>();

		public void SetupScene(int level) {
			SetUpBoard();
			InitGridPositions();

			Count enemyCount = new Count((int)Mathf.Log(level, 2f));

			LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum, null);
			LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum, null);
			LayoutObjectAtRandom(enemyTiles, enemyCount.minimum, enemyCount.maximum, OnEnemyCreated);
			LayoutExitObject();
		}

		private void LayoutExitObject() {
			Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
		}

		private void SetUpBoard() {
			var boardHolder = new GameObject("Board").transform;

			for (int x = -1; x < columns + 1; x++) {
				for (int y = -1; y < rows + 1; y++) {

					var instance = Instantiate(
						GetBoardObject(x, y),
						new Vector3(x, y, 0f),
						Quaternion.identity
					);

					instance.transform.SetParent(boardHolder);
				}
			}
		}

		private GameObject GetBoardObject(int x, int y) {
			return IsBorderLine(x, y) ? GetRandomOuterWall() : GetRandomFloor();
		}

		private GameObject GetRandomFloor() {
			return floorTiles[Random.Range(0, floorTiles.Length)];
		}

		private GameObject GetRandomOuterWall() {
			return outerWallTiles[Random.Range(0, outerWallTiles.Length)];
		}

		private bool IsBorderLine(int x, int y) {
			return x == -1 || x == columns || y == -1 || y == rows;
		}

		private void InitGridPositions() {
			_gridPositions.Clear();

			for (int xPos = 1; xPos < columns - 1; xPos++) {
				for (int yPos = 1; yPos < rows - 1; yPos++) {
					_gridPositions.Add(new Vector3(xPos, yPos, 0f));
				}
			}
		}

		private void LayoutObjectAtRandom(GameObject[] objects, 
			int min, int max, UnityEvent<GameObject> OnCreated = null) {

			int cntCreate = Random.Range(min, max + 1);

			for (int i = 0; i < cntCreate; i++) {
				var newObject = Instantiate(
					GetRandomObject(objects),
					GetRandomEmptyPosition(),
					Quaternion.identity
				);

				OnCreated?.Invoke(newObject);
			}
		}

		private static GameObject GetRandomObject(GameObject[] objects) {
			return objects[Random.Range(0, objects.Length)];
		}

		private Vector3 GetRandomEmptyPosition() {
			var randomIndex = Random.Range(0, _gridPositions.Count);
			var result = _gridPositions[randomIndex];
			_gridPositions.RemoveAt(randomIndex);
			return result;
		}

		#region Validation Tests
#if UNITY_EDITOR
		public bool ValidateResources() {

			if (exit == null) {
				return false;
			}

			if (floorTiles == null) {
				return false;
			}

			if (wallTiles == null) {
				return false;
			}

			if (foodTiles == null) {
				return false;
			}

			if (enemyTiles == null) {
				return false;
			}

			if (outerWallTiles == null) {
				return false;
			}

			return true;
		}
#endif
		#endregion
	}
}
