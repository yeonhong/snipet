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

		private BoardManager boardManager;
		private int level = 1;
		private EnemyManager _enemyManager = null;
		private bool _playersTurn = true;
		private Player player = null;

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
			AllocateInstance();
			AllocateBoardManager();
			AllocateEnemyManager();

			InitGame();
		}

		private void AllocateEnemyManager() {
			if (_enemyManager == null) {
				_enemyManager = new EnemyManager(turnDelay);
				_enemyManager.OnEndEnemyTurn += OnEndEnemyTurn;
			}
		}

		private void AllocateBoardManager() {
			if (boardManager == null) {
				boardManager = GetComponent<BoardManager>();
				boardManager.OnEnemyCreated.AddListener(OnEnemyCreated);
			}
		}

		private void AllocateInstance() {
			if (instance == null) {
				instance = this;
			}
			else if (instance != this) {
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);
		}

		private void OnDestroy() {
			boardManager.OnEnemyCreated.RemoveListener(OnEnemyCreated);
		}

		private void OnEnemyCreated(GameObject enemy) {
			_enemyManager.AddEnemy(enemy.GetComponent<Enemy>());
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void CallbackInitialization() {
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
			if (instance != null) {
				MoveToNextLevel();
			}
		}

		private static void MoveToNextLevel() {
			instance.level++;
			instance.InitGame();
		}

		private void InitGame() {
			_enemyManager.Init();

			boardManager?.SetupScene(level);

			var tPlayer = GameObject.FindGameObjectWithTag("Player").transform;
			player = tPlayer.GetComponent<Player>();
			player.OnFoodEmpty += Player_OnFoodEmpty;
			player.OnEndPlayerTurn += Player_OnEndPlayerTurn;

			OnGameInit?.Invoke(this, new GameDayArgs(level));
			Invoke("HideLevelImage", levelStartDelay);
		}

		private void Player_OnEndPlayerTurn(object sender, EventArgs e) {
			_playersTurn = false;
			StartCoroutine(_enemyManager.MoveEnemies(player.transform));
		}

		private void Player_OnFoodEmpty(object sender, EventArgs e) {
			SoundManager.instance.PlaySingle(gameOverSound);
			SoundManager.instance.StopMusic();

			OnGameOver?.Invoke(this, new GameDayArgs(level));

			enabled = false;
		}

		private void HideLevelImage() {
			OnGameStart?.Invoke(this, null);
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

		private void OnEndEnemyTurn(object sender, EventArgs e) {
			_playersTurn = true;
		}
	}
}
