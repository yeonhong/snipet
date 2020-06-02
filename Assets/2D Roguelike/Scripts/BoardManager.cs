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
		}

		public int columns = 8;
		public int rows = 8;
		public Count wallCount = new Count(5, 9);
		public Count foodCount = new Count(1, 5);
		public GameObject exit;
		public GameObject[] floorTiles;
		public GameObject[] wallTiles;
		public GameObject[] foodTiles;
		public GameObject[] enemyTiles;
		public GameObject[] outerWallTiles;

		private Transform boardHolder;
		private List<Vector3> gridPositions = new List<Vector3>();

		public CreateEnemyEvent OnEnemyCreated = new CreateEnemyEvent();

		private void InitialiseList() {
			gridPositions.Clear();

			for (int x = 1; x < columns - 1; x++) {
				for (int y = 1; y < rows - 1; y++) {
					gridPositions.Add(new Vector3(x, y, 0f));
				}
			}
		}

		private void BoardSetup() {
			boardHolder = new GameObject("Board").transform;

			for (int x = -1; x < columns + 1; x++) {
				for (int y = -1; y < rows + 1; y++) {
					GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

					if (x == -1 || x == columns || y == -1 || y == rows) {
						toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
					}

					var instance =
						Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

					instance.transform.SetParent(boardHolder);
				}
			}
		}

		private Vector3 RandomPosition() {
			int randomIndex = Random.Range(0, gridPositions.Count);
			Vector3 randomPosition = gridPositions[randomIndex];
			gridPositions.RemoveAt(randomIndex);
			return randomPosition;
		}

		private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum, UnityEvent<GameObject> OnCreated = null) {
			int objectCount = Random.Range(minimum, maximum + 1);

			for (int i = 0; i < objectCount; i++) {
				var randomPosition = RandomPosition();
				var tileChoice = tileArray[Random.Range(0, tileArray.Length)];
				var instance = Instantiate(tileChoice, randomPosition, Quaternion.identity);
				OnCreated?.Invoke(instance);
			}
		}

		public void SetupScene(int level) {
			BoardSetup();
			InitialiseList();
			LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
			LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
			int enemyCount = (int)Mathf.Log(level, 2f);
			LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount, OnEnemyCreated);
			Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
		}
	}
}
