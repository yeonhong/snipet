using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Roguelike2D
{
	public class Player : MovingObject
	{
		[SerializeField] private float restartLevelDelay = 1f;
		[SerializeField] private int wallDamage = 1;

		// todo : 옵저버방식으로 리펙토링.
		[SerializeField] private Text foodText = null;

		public AudioClip moveSound1 = null;
		public AudioClip moveSound2 = null;

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
			UpdateFoodText();
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
				AttemptMove<Wall>(horizontal, vertical);
			}
		}

		// 이동시도 (attempt : 시도하다)
		protected override void AttemptMove<T>(int xDir, int yDir) {
			_playerModel.LoseFood(1);
			UpdateFoodText();

			base.AttemptMove<T>(xDir, yDir);

			if (Move(xDir, yDir, out RaycastHit2D hit)) {
				_soundManager.RandomizeSfx(moveSound1, moveSound2);
			}

			CheckIfGameOver();

			_playerManager.EndPlayersTurn();
		}

		protected override void OnCantMove<T>(T component) {
			var hitWall = component as Wall;
			if (hitWall != null) {
				hitWall.DamageWall(wallDamage);
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
			UpdateFoodText(food.Points);
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
			UpdateFoodText(-damage);
			CheckIfGameOver();
		}

		private void UpdateFoodText(int gainAmount = 0) {
			if (foodText == null) {
				return;
			}

			if (gainAmount == 0) {
				foodText.text = "Food: " + _playerModel.Food;
			}
			else if (gainAmount > 0) {
				foodText.text = "+" + gainAmount + " Food: " + _playerModel.Food;
			}
			else {
				foodText.text = "-" + gainAmount + " Food: " + _playerModel.Food;
			}
		}
	}
}

