using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Roguelike2D
{
	public interface IPlayerManage
	{
		int GetPlayerFoodPoints();
		void SetPlayerFoodPoints(int amount);
		bool IsPlayersTurn();
		void EndPlayersTurn();
		void GameOver();
	}

	// todo : 재시작 기능을 추가한다
	// todo : gamemanager 리펙토링
	// todo : ui manager가 필요하다 - 중간 connect를 위해.

	public class GameManager : MonoBehaviour, IPlayerManage
	{
		public static GameManager instance = null;

		public float levelStartDelay = 2f;
		public float turnDelay = 0.1f;
		public int playerFoodPoints = 100;

		[HideInInspector] public bool playersTurn = true;
		[SerializeField] private AudioClip gameOverSound = null;

		private BoardManager boardScript;
		private int level = 1;
		private List<Enemy> enemies;
		private bool enemiesMoving;
		private bool doingSetup = true;

		#region EventHandler
		public event EventHandler<GameDayArgs> OnGameInit;
		public event EventHandler OnGameStart;
		public event EventHandler<GameDayArgs> OnGameOver;

		public class GameDayArgs : EventArgs
		{
			public int Day { get; }
			public GameDayArgs(int day) {
				Day = day;
			}
		} 
		#endregion

		private void Awake() {
			if (instance == null) {
				instance = this;
			}
			else if (instance != this) {
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);
			enemies = new List<Enemy>();
			boardScript = GetComponent<BoardManager>();
			InitGame();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void CallbackInitialization() {
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
			if (instance != null) {
				instance.level++;
				instance.InitGame();
			}
		}

		private void InitGame() {
			doingSetup = true;
			
			enemies.Clear();
			boardScript?.SetupScene(level);

			OnGameInit?.Invoke(this, new GameDayArgs(level));
			Invoke("HideLevelImage", levelStartDelay);
		}

		private void HideLevelImage() {
			OnGameStart?.Invoke(this, null);
			doingSetup = false;
		}

		private void Update() {
			if (playersTurn || enemiesMoving || doingSetup) {
				return;
			}

			StartCoroutine(MoveEnemies());
		}

		public void AddEnemyToList(Enemy script) {
			enemies.Add(script);
		}

		public void GameOver() {
			SoundManager.instance.PlaySingle(gameOverSound);
			SoundManager.instance.StopMusic();

			OnGameOver?.Invoke(this, new GameDayArgs(level));

			enabled = false;
		}

		private IEnumerator MoveEnemies() {
			enemiesMoving = true;

			yield return new WaitForSeconds(turnDelay);

			if (enemies.Count == 0) {
				yield return new WaitForSeconds(turnDelay);
			}

			for (int i = 0; i < enemies.Count; i++) {
				enemies[i].MoveEnemy();
				yield return new WaitForSeconds(enemies[i].moveTime);
			}

			playersTurn = true;
			enemiesMoving = false;
		}

		public int GetPlayerFoodPoints() {
			return playerFoodPoints;
		}

		public void SetPlayerFoodPoints(int amount) {
			playerFoodPoints = amount;
		}

		public bool IsPlayersTurn() {
			return playersTurn;
		}

		public void EndPlayersTurn() {
			playersTurn = false;
		}
	}
}

