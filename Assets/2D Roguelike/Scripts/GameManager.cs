using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Roguelike2D.Utils;
using Roguelike2D.UI;

namespace Roguelike2D
{
	public interface IPlayerManage
	{
		int GetPlayerFoodPoints();
		bool IsPlayersTurn();
	}

	public class GameManager : MonoBehaviour, IPlayerManage
	{
		public static GameManager instance = null;

		[SerializeField] private float _levelStartDelay = 2f;
		[SerializeField] private float _nextLevelDelay = 1f;
		[SerializeField] private float _turnDelay = 0.1f;
		[SerializeField] private int _initFoodPoints = 100;
		
		[SerializeField] private AudioClip _gameOverSound = null;
		[SerializeField] private int _level = 1;

		private BoardManager _boardManager = null;
		private EnemyManager _enemyManager = null;
		private UIManager _uiManager = null;
		private Player _player = null;
		private int _playerFoodPoints;

		enum Turn
		{
			Player,
			Enemy,
		}

		private Turn _currentTurn = Turn.Player;

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
			AllocateUIManager();

			SetPlayerFoodPoints(_initFoodPoints);

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

		private void AllocateUIManager() {
			if(_uiManager == null) {
				_uiManager = GameObject.FindWithTag(UIManager.TAG_NAME).GetComponent<UIManager>();
				_uiManager.OnRestartButtonClick += OnRestart;
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

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void CallbackInitialization() {
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
			if (instance != null) {
				instance.AllocateUIManager();
				instance.InitGame(instance._level);
			}
		}

		public int GetPlayerFoodPoints() {
			return _playerFoodPoints;
		}

		private void SetPlayerFoodPoints(int amount) {
			_playerFoodPoints = amount;
		}

		public bool IsPlayersTurn() {
			return _currentTurn == Turn.Player;
		}

		private void InitGame(int level) {
			_enemyManager?.Init();
			_boardManager?.SetupScene(level);
			AllocatePlayerComponent();
			ShowGameStart(level);
		}

		private void ShowGameStart(int level) {
			OnGameInit?.Invoke(this, new GameDayArgs(level));
			
			DOTween.Sequence().Wait(_levelStartDelay, () => {
				OnGameStart?.Invoke(this, null);
			});
		}

		private void AllocatePlayerComponent() {
			var tPlayer = GameObject.FindGameObjectWithTag(Player.TAG_NAME).transform;
			_player = tPlayer.GetComponent<Player>();
			_player.OnFoodEmpty += OnFoodEmpty;
			_player.OnEndPlayerTurn += OnEndPlayerTurn;
			_player.OnEnterExit += OnEnterExit;
		}

		private void OnEnterExit(object sender, EventArgs e) {
			_level++;
			LoadLevelScene(_player.Food);
		}

		private void OnRestart(object sender, EventArgs e) {
			_level = 0;
			LoadLevelScene(_initFoodPoints);
		}

		private void LoadLevelScene(int startFoodPoints) {
			SetPlayerFoodPoints(startFoodPoints);
			DOTween.Sequence().Wait(_nextLevelDelay, () => {
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
			});
		}

		private void OnEnemyCreated(GameObject enemy) {
			_enemyManager.Add(enemy.GetComponent<Enemy>());
		}

		private void OnEndPlayerTurn(object sender, EventArgs e) {
			_currentTurn = Turn.Enemy;
			StartCoroutine(_enemyManager.MoveEnemies(_player.transform));
		}

		private void OnFoodEmpty(object sender, EventArgs e) {
			SoundManager.instance.PlaySingle(_gameOverSound);
			SoundManager.instance.StopMusic();

			OnGameOver?.Invoke(this, new GameDayArgs(_level));

			enabled = false;
		}

		private void OnEndEnemyTurn(object sender, EventArgs e) {
			_currentTurn = Turn.Player;
		}

		#region Validation Tests
#if UNITY_EDITOR
		public bool ValidateResources() {
			return _gameOverSound != null;
		}
#endif 
		#endregion
	}
}
