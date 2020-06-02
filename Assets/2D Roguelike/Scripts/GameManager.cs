using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Roguelike2D
{
	public interface IPlayerManage
	{
		int GetPlayerFoodPoints();
		void SetPlayerFoodPoints(int amount);
		bool IsPlayersTurn();
	}

	// todo : 재시작 기능을 추가한다

	public class GameManager : MonoBehaviour, IPlayerManage
	{
		public static GameManager instance = null;

		[SerializeField] private float _levelStartDelay = 2f;
		[SerializeField] private float _turnDelay = 0.1f;
		[SerializeField] private int _playerFoodPoints = 100;
		[SerializeField] private AudioClip _gameOverSound = null;
		[SerializeField] private int _level = 1;

		private const string PLAYER_TAG = "Player";
		private bool _playersTurn = true;
		private BoardManager _boardManager;
		private EnemyManager _enemyManager = null;
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
				_enemyManager = new EnemyManager(_turnDelay);
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
			_enemyManager?.Init();
			_boardManager?.SetupScene(level);
			AllocatePlayerComponent();
			ShowGameStart(level);
		}

		private void ShowGameStart(int level) {
			OnGameInit?.Invoke(this, new GameDayArgs(level));

			var waitStartDelay = DOTween.Sequence();
			waitStartDelay.AppendInterval(_levelStartDelay);

			waitStartDelay.OnComplete(() => {
				OnGameStart?.Invoke(this, null);
			});
		}

		private void AllocatePlayerComponent() {
			var tPlayer = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
			_player = tPlayer.GetComponent<Player>();
			_player.OnFoodEmpty += Player_OnFoodEmpty;
			_player.OnEndPlayerTurn += Player_OnEndPlayerTurn;
		}

		private void Player_OnEndPlayerTurn(object sender, EventArgs e) {
			_playersTurn = false;
			StartCoroutine(_enemyManager.MoveEnemies(_player.transform));
		}

		private void Player_OnFoodEmpty(object sender, EventArgs e) {
			SoundManager.instance.PlaySingle(_gameOverSound);
			SoundManager.instance.StopMusic();

			OnGameOver?.Invoke(this, new GameDayArgs(_level));

			enabled = false;
		}

		public int GetPlayerFoodPoints() {
			return _playerFoodPoints;
		}

		public void SetPlayerFoodPoints(int amount) {
			_playerFoodPoints = amount;
		}

		public bool IsPlayersTurn() {
			return _playersTurn;
		}

		private void OnEndEnemyTurn(object sender, EventArgs e) {
			_playersTurn = true;
		}
	}
}
