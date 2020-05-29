using Roguelike2D.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roguelike2D
{
	public class Player : MovingObject
	{
		[SerializeField] private float restartLevelDelay = 1f;
		[SerializeField] private int wallDamage = 1;
		public AudioClip[] moveSounds = null;

		private Animator animator = null;
		private PlayerModel _playerModel = null;
		public IPlayerManage _gameManager { private get; set; }
		public ISoundManager _soundManager { private get; set; }
		public InputContoller _inputContoller { private get; set; }

		#region EventHandler
		public event EventHandler<UpdateFoodCountArgs> OnUpdatedFood;
		public event EventHandler<GainFoodArgs> OnGainFood;
		public event EventHandler<LossFoodArgs> OnLossFood;

		public class UpdateFoodCountArgs : EventArgs
		{
			public int Current { get; }

			public UpdateFoodCountArgs(int current) {
				Current = current;
			}
		}

		public class GainFoodArgs : EventArgs
		{
			public int Current { get; }
			public int Gain { get; }

			public GainFoodArgs(int gain, int current) {
				Current = current;
				Gain = gain;
			}
		}

		public class LossFoodArgs : EventArgs
		{
			public int Current { get; }
			public int Loss { get; }

			public LossFoodArgs(int loss, int current) {
				Current = current;
				Loss = loss;
			}
		}

		#endregion

		protected override void Start() {
			base.Start();

			animator = GetComponent<Animator>();

			if (_gameManager == null) {
				_gameManager = GameManager.instance;
			}

			if (_soundManager == null) {
				_soundManager = SoundManager.instance;
			}

			if (_inputContoller == null) {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
				_inputContoller = new InputController_Standalone(_unityService);
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
				_inputContoller = new InputController_Mobile(_unityService);
#endif
			}

			_playerModel = new PlayerModel(_gameManager.GetPlayerFoodPoints());
			OnUpdatedFood?.Invoke(this, new UpdateFoodCountArgs(_playerModel.Food));
		}

		private void OnDisable() {
			_gameManager.SetPlayerFoodPoints(_playerModel.Food);
		}

		#region 이동관련
		private void Update() {
			if (!_gameManager.IsPlayersTurn() || _isMoving) {
				return;
			}

			_inputContoller.MoveController(out int horizontal, out int vertical);

			if (horizontal != 0 || vertical != 0) {
				AttemptMove(horizontal, vertical);
			}
		}

		// todo : 플레이어가 이동하는 중인데 적이 이동한다?
		private void AttemptMove(int xDir, int yDir) {
			var isMoved = base.AttemptMove<Wall>(xDir, yDir);

			_playerModel.LoseFood(1);
			OnUpdatedFood?.Invoke(this, new UpdateFoodCountArgs(_playerModel.Food));
			CheckIfGameOver();

			if (isMoved) {
				_soundManager.RandomizeSfx(moveSounds);
				Invoke("OnTurnEnd", moveTime);
			} else {
				OnTurnEnd();
			}
		}

		private void OnTurnEnd() {
			Debug.LogWarning("player move? " + _isMoving);
			_gameManager.EndPlayersTurn();
		}

		protected override void OnBumped<T>(T component) {
			var bumpedWall = component as Wall;
			if (bumpedWall != null) {
				bumpedWall.DamageWall(wallDamage);
				animator.SetTrigger("playerChop");
			}
		}
		#endregion

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.tag == "Exit") {
				StartCoroutine(CoRestart());
				enabled = false;
			}
			else if (other.tag == "Food" || other.tag == "Soda") {
				EatFood(other.GetComponent<FoodObject>());
			}
		}

		private void EatFood(FoodObject food) {
			_playerModel.GainFood(food.Points);
			OnGainFood?.Invoke(this, new GainFoodArgs(food.Points, _playerModel.Food));

			food.Consume();
		}

		private IEnumerator CoRestart() {
			yield return new WaitForSeconds(restartLevelDelay);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}

		private void CheckIfGameOver() {
			if (_playerModel.IsFoodEmpty) {
				_gameManager.GameOver();
			}
		}

		public void OnDamage(int damage) {
			animator.SetTrigger("playerHit");

			_playerModel.LoseFood(damage);
			OnLossFood?.Invoke(this, new LossFoodArgs(damage, _playerModel.Food));

			CheckIfGameOver();
		}
	}
}

