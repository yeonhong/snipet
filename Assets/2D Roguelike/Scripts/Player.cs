using Roguelike2D.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roguelike2D
{
	public class Player : MovingObject
	{
		[SerializeField] private float restartLevelDelay = 1f;
		[SerializeField] private int wallDamage = 1;
		[SerializeField] private FoodTextDisplayer _foodTextDisplayer = null;
		public AudioClip[] moveSounds = null;

		private Animator animator = null;
		private PlayerModel _playerModel = null;
		public IPlayerManage _playerManager { private get; set; }
		public ISoundManager _soundManager { private get; set; }
		public InputContoller _inputContoller { private get; set; }

		protected override void Start() {
			animator = GetComponent<Animator>();
			if (_unityService == null) {
				_unityService = new UnityService();
			}
			if (_playerManager == null) {
				_playerManager = GameManager.instance;
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

			_playerModel = new PlayerModel(_playerManager.GetPlayerFoodPoints());
			_foodTextDisplayer?.UpdateFoodAmount(_playerModel.Food);
			base.Start();
		}

		private void OnDisable() {
			_playerManager.SetPlayerFoodPoints(_playerModel.Food);
		}

		#region 이동관련
		private void Update() {
			if (!_playerManager.IsPlayersTurn()) {
				return;
			}

			_inputContoller.MoveController(out int horizontal, out int vertical);

			if (horizontal != 0 || vertical != 0) {
				AttemptMove(horizontal, vertical);
			}
		}

		private void AttemptMove(int xDir, int yDir) {
			var isMoved = base.AttemptMove<Wall>(xDir, yDir);

			if (isMoved) {
				_soundManager.RandomizeSfx(moveSounds);
			}

			_playerManager.EndPlayersTurn();

			_playerModel.LoseFood(1);
			_foodTextDisplayer?.UpdateFoodAmount(_playerModel.Food);
			CheckIfGameOver();
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
			_foodTextDisplayer?.GainFoodAmount(food.Points, _playerModel.Food);
			food.Consume();
		}

		private IEnumerator CoRestart() {
			yield return new WaitForSeconds(restartLevelDelay);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}

		private void CheckIfGameOver() {
			if (_playerModel.IsFoodEmpty) {
				_playerManager.GameOver();
			}
		}

		public void OnDamage(int damage) {
			animator.SetTrigger("playerHit");

			_playerModel.LoseFood(damage);
			_foodTextDisplayer?.LossFoodAmount(damage, _playerModel.Food);

			CheckIfGameOver();
		}
	}
}

