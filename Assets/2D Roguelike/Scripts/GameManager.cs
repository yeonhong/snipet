using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public class GameManager : MonoBehaviour, IPlayerManage
	{
		public static GameManager instance = null;

		public float levelStartDelay = 2f;
		public float turnDelay = 0.1f;
		public int playerFoodPoints = 100;

		[SerializeField] private AudioClip gameOverSound = null;

		private BoardManager boardScript;
		private int level = 1;
		private EnemyManager _enemyManager = null;
		private bool _playersTurn = true;
		private Transform _tPlayer = null;

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
			} else if (instance != this) {
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);

			boardScript = GetComponent<BoardManager>();
			boardScript.OnEnemyCreated.AddListener(OnEnemyCreated);

			if (_enemyManager == null) {
				_enemyManager = new EnemyManager(turnDelay);
				_enemyManager.OnEndEnemyTurn += OnEndEnemyTurn;
			}

			InitGame();
		}

		private void OnDestroy() {
			boardScript.OnEnemyCreated.RemoveListener(OnEnemyCreated);
		}

		private void OnEnemyCreated(GameObject enemy) {
			AddEnemyToList(enemy.GetComponent<Enemy>());
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

			_enemyManager.Init();
			boardScript?.SetupScene(level);
			_tPlayer = GameObject.FindGameObjectWithTag("Player").transform;

			OnGameInit?.Invoke(this, new GameDayArgs(level));
			Invoke("HideLevelImage", levelStartDelay);
		}

		private void HideLevelImage() {
			OnGameStart?.Invoke(this, null);
		}

		private void AddEnemyToList(Enemy enemy) => _enemyManager.AddEnemy(enemy);

		public void GameOver() {
			SoundManager.instance.PlaySingle(gameOverSound);
			SoundManager.instance.StopMusic();

			OnGameOver?.Invoke(this, new GameDayArgs(level));

			enabled = false;
		}

		public int GetPlayerFoodPoints() {
			return playerFoodPoints;
		}

		public void SetPlayerFoodPoints(int amount) {
			playerFoodPoints = amount;
		}

		public bool IsPlayersTurn() {
			return _playersTurn;
		}

		public void EndPlayersTurn() {
			_playersTurn = false;

			StartCoroutine(_enemyManager.MoveEnemies(_tPlayer));
		}

		private void OnEndEnemyTurn(object sender, EventArgs e) {
			_playersTurn = true;
		}
	}
}
