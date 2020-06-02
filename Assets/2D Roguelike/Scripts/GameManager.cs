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

		private BoardManager _boardManager;
		private int _level = 1;
		private EnemyManager _enemyManager = null;
		private bool _playersTurn = true;
		private Player _player = null;

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

			InitGame(_level);
		}

		private void AllocateEnemyManager() {
			if (_enemyManager == null) {
				_enemyManager = new EnemyManager(turnDelay);
				_enemyManager.OnEndEnemyTurn += OnEndEnemyTurn;
			}
		}

		private void AllocateBoardManager() {
			if (_boardManager == null) {
				_boardManager = GetComponent<BoardManager>();
				_boardManager.OnEnemyCreated.AddListener(OnEnemyCreated);
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
			_boardManager.OnEnemyCreated.RemoveListener(OnEnemyCreated);
		}

		private void OnEnemyCreated(GameObject enemy) {
			_enemyManager.Add(enemy.GetComponent<Enemy>());
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
			instance._level++;
			instance.InitGame(instance._level);
		}

		private void InitGame(int level) {
			_enemyManager.Init();
			_boardManager?.SetupScene(level);
			AllocatePlayerComponent();

			OnGameInit?.Invoke(this, new GameDayArgs(level));
			Invoke("WaitStartDelay", levelStartDelay);
		}

		private void AllocatePlayerComponent() {
			var tPlayer = GameObject.FindGameObjectWithTag("Player").transform;
			_player = tPlayer.GetComponent<Player>();
			_player.OnFoodEmpty += Player_OnFoodEmpty;
			_player.OnEndPlayerTurn += Player_OnEndPlayerTurn;
		}

		private void Player_OnEndPlayerTurn(object sender, EventArgs e) {
			_playersTurn = false;
			StartCoroutine(_enemyManager.MoveEnemies(_player.transform));
		}

		private void Player_OnFoodEmpty(object sender, EventArgs e) {
			SoundManager.instance.PlaySingle(gameOverSound);
			SoundManager.instance.StopMusic();

			OnGameOver?.Invoke(this, new GameDayArgs(_level));

			enabled = false;
		}

		private void WaitStartDelay() {
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
