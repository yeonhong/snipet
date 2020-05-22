using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Roguelike2D
{
	public class PlayerModel
	{
		public PlayerModel(int initFood) {
			Food = initFood;
		}

		public int Food { get; private set; }
		public bool IsFoodEmpty => Food <= 0;

		public void LoseFood(int amount) {
			if (amount < 0) {
				throw new ArgumentOutOfRangeException($"{nameof(amount)}의 값은 음수가 되면 안된다.");
			}

			Food = Math.Max(Food - amount, 0);
		}

		public void GainFood(int amount) {
			if (amount < 0) {
				throw new ArgumentOutOfRangeException($"{nameof(amount)}의 값은 음수가 되면 안된다.");
			}

			Food += amount;
		}
	}

	public class Player : MovingObject
	{
		[SerializeField] private float restartLevelDelay = 1f;
		[SerializeField] private int pointsPerFood = 10;
		[SerializeField] private int pointsPerSoda = 20;
		[SerializeField] private int wallDamage = 1;

		[SerializeField] private Text foodText = null;

		public AudioClip moveSound1 = null;
		public AudioClip moveSound2 = null;
		public AudioClip eatSound1 = null;
		public AudioClip eatSound2 = null;
		public AudioClip drinkSound1 = null;
		public AudioClip drinkSound2 = null;
		public AudioClip gameOverSound = null;

		private Animator animator = null;
		private PlayerModel _playerModel = null;
		public IUnityService _unityService { private get; set; }
		public IPlayerManage _playerManager { private get; set; }
		public ISoundManager _soundManager { private get; set; }

		protected override void Start() {
			animator = GetComponent<Animator>();
			if (_unityService == null) {
				_unityService = new UnityService();
			}
			if (_playerManager == null) {
				_playerManager = GameManager.instance;
			}
			if(_soundManager == null) {
				_soundManager = SoundManager.instance;
			}

			_playerModel = new PlayerModel(_playerManager.GetPlayerFoodPoints());
			UpdateFoodText();
			base.Start();
		}

		private void OnDisable() {
			_playerManager.SetPlayerFoodPoints(_playerModel.Food);
		}

		private void Update() {
			if (!_playerManager.IsPlayersTurn()) {
				return;
			}

			MoveController(out int horizontal, out int vertical);

			if (horizontal != 0 || vertical != 0) {
				AttemptMove<Wall>(horizontal, vertical);
			}
		}

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.
#endif

		private void MoveController(out int horizontal, out int vertical) {
			// todo : 플랫폼에 따라 조작계가 변할 수 있도록 리펙토링.
#if UNITY_STANDALONE || UNITY_WEBPLAYER

			horizontal = (int)(_unityService.GetAxisRaw("Horizontal"));
			vertical = (int)(_unityService.GetAxisRaw("Vertical"));
			if (horizontal != 0) {
				vertical = 0;
			}

			//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
			//Check if Input has registered more than zero touches
			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.touches[0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}
				
				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					
					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;
					
					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;
					
					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;
					
					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif //End of mobile platform dependendent compilation section started above with #elif
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
			Wall hitWall = component as Wall;
			hitWall.DamageWall(wallDamage);
			animator.SetTrigger("playerChop");
		}

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.tag == "Exit") {
				Invoke("Restart", restartLevelDelay);
				enabled = false;
			}
			else if (other.tag == "Food") {
				_playerModel.GainFood(pointsPerFood);
				UpdateFoodText(pointsPerFood);
				_soundManager.RandomizeSfx(eatSound1, eatSound2);
				other.gameObject.SetActive(false);
			}
			else if (other.tag == "Soda") {
				_playerModel.GainFood(pointsPerSoda);
				UpdateFoodText(pointsPerSoda);
				_soundManager.RandomizeSfx(drinkSound1, drinkSound2);
				other.gameObject.SetActive(false);
			}
		}

		private void Restart() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}

		public void LoseFood(int loss) {
			animator.SetTrigger("playerHit");
			_playerModel.LoseFood(loss);
			UpdateFoodText(-loss);
			CheckIfGameOver();
		}

		private void CheckIfGameOver() {
			if (_playerModel.IsFoodEmpty) {
				_soundManager.PlaySingle(gameOverSound);
				_soundManager.StopMusic();
				_playerManager.GameOver();
			}
		}

		private void UpdateFoodText(int gainAmount = 0) {
			if (foodText == null) return;

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

