using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Roguelike2D.UI
{
	public class UIManager : MonoBehaviour
	{
		public GameObject restartButton = null;
		public GameObject levelImage = null;
		public Text levelText;
		public FoodTextDisplayer foodTextDisplayer = null;

		private Player _player = null;
		private GameManager _gameManager = null;

		private void Awake() {
			_player = FindObjectOfType<Player>();
			_gameManager = GameManager.instance;

			Assert.IsNotNull(_player);
			Assert.IsNotNull(_gameManager);
		}

		private void OnEnable() {
			_player.OnUpdatedFood += _player_OnUpdatedFood;
			_player.OnGainFood += _player_OnGainFood;
			_player.OnLossFood += _player_OnLossFood;

			_gameManager.OnGameInit += _gameManager_OnGameInit;
			_gameManager.OnGameStart += _gameManager_OnGameStart;
			_gameManager.OnGameOver += _gameManager_OnGameOver;
		}
		
		private void OnDisable() {
			_player.OnUpdatedFood -= _player_OnUpdatedFood;
			_player.OnGainFood -= _player_OnGainFood;
			_player.OnLossFood -= _player_OnLossFood;

			_gameManager.OnGameInit -= _gameManager_OnGameInit;
			_gameManager.OnGameStart -= _gameManager_OnGameStart;
			_gameManager.OnGameOver -= _gameManager_OnGameOver;
		}

		#region FoodTestDisplayer 관련
		private void _player_OnUpdatedFood(object sender, Player.UpdateFoodCountArgs e) {
			foodTextDisplayer.UpdateFoodAmount(e.Current);
		}

		private void _player_OnLossFood(object sender, Player.LossFoodArgs e) {
			foodTextDisplayer.LossFoodAmount(e.Loss, e.Current);
		}

		private void _player_OnGainFood(object sender, Player.GainFoodArgs e) {
			foodTextDisplayer.GainFoodAmount(e.Gain, e.Current);
		}
		#endregion

		private void _gameManager_OnGameOver(object sender, GameManager.GameDayArgs e) {
			levelText.text = $"After {e.Day} days, you starved.";
			levelImage.SetActive(true);
		}

		private void _gameManager_OnGameStart(object sender, System.EventArgs e) {
			levelImage.SetActive(false);
		}

		private void _gameManager_OnGameInit(object sender, GameManager.GameDayArgs e) {
			levelText.text = $"Day {e.Day}";
			levelImage.SetActive(true);
		}
	}
}