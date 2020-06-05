using Roguelike2D.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Roguelike2D.Utils;

namespace Roguelike2D
{
	public class Player : MovingObject
	{
		public const string TAG_NAME = "Player";
		private const int MOVE_COST = 1;
		[SerializeField] private int wallDamage = 1;
		public AudioClip[] moveSounds = null;

		public IPlayerManage _gameManager { private get; set; }
		public ISoundManager _soundManager { private get; set; }
		public InputContoller _inputContoller { private get; set; }

		private Animator _animator = null;
		private PlayerModel _playerModel = null;
		public int Food { get { return _playerModel.Food; } }

		#region EventHandler
		public event EventHandler<UpdateFoodCountArgs> OnUpdatedFood;
		public event EventHandler<GainFoodArgs> OnGainFood;
		public event EventHandler<LossFoodArgs> OnLossFood;
		public event EventHandler OnFoodEmpty;
		public event EventHandler OnEndPlayerTurn;
		public event EventHandler OnEnterExit;

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

			AllocateComponent();

			OnUpdatedFood?.Invoke(this, new UpdateFoodCountArgs(_playerModel.Food));
		}

		private void AllocateComponent() {
			_animator = GetComponent<Animator>();

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

			OnMoveEnd += Player_OnMoveEnd;
		}

		#region 이동관련
		private void Update() {
			if (!_gameManager.IsPlayersTurn() || IsMoving) {
				return;
			}

			_inputContoller.MoveController(out int horizontal, out int vertical);

			if (horizontal != 0 || vertical != 0) {
				AttemptMove(horizontal, vertical);
			}
		}

		private void AttemptMove(int xDir, int yDir) {
			var isMove = base.AttemptMove<Wall>(xDir, yDir);

			_playerModel.LoseFood(MOVE_COST);
			OnUpdatedFood?.Invoke(this, new UpdateFoodCountArgs(_playerModel.Food));
			CheckIfGameOver();

			if (isMove) {
				_soundManager.RandomizeSfx(moveSounds);
			} else {
				EndPlayerTurn();
			}
		}

		private void Player_OnMoveEnd(object sender, EventArgs e) {
			EndPlayerTurn();
		}

		private void EndPlayerTurn() {
			OnEndPlayerTurn?.Invoke(this, null);
		}

		protected override void OnBumped<T>(T component) {
			var bumpedWall = component as Wall;
			if (bumpedWall != null) {
				bumpedWall.DamageWall(wallDamage);
				_animator.SetTrigger("playerChop");
			}
		}
		#endregion

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.tag == "Exit") {
				OnEnterExit?.Invoke(this, null);
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

		private void CheckIfGameOver() {
			if (_playerModel.IsFoodEmpty) {
				OnFoodEmpty.Invoke(this, null);
			}
		}

		public void OnDamage(int damage) {
			_animator.SetTrigger("playerHit");

			_playerModel.LoseFood(damage);
			OnLossFood?.Invoke(this, new LossFoodArgs(damage, _playerModel.Food));
			CheckIfGameOver();
		}
	}
}
