using UnityEngine;
using UnityEngine.Assertions;

namespace Roguelike2D.UI
{
	public class UIManager : MonoBehaviour
	{
		public GameObject restartButton = null;
		public GameObject levelImageObject = null;
		public FoodTextDisplayer foodTextDisplayer = null;

		private Player _player = null;

		private void Awake() {
			_player = FindObjectOfType<Player>();

			Assert.IsNotNull(_player);
		}

		private void OnEnable() {
			_player.UpdatedFood += _player_UpdatedFood;
			_player.GainFood += _player_GainFood;
			_player.LossFood += _player_LossFood;
		}

		private void OnDisable() {
			_player.UpdatedFood -= _player_UpdatedFood;
			_player.GainFood -= _player_GainFood;
			_player.LossFood -= _player_LossFood;
		}

		private void _player_UpdatedFood(object sender, Player.UpdateFoodCountArgs e) {
			foodTextDisplayer.UpdateFoodAmount(e.Current);
		}

		private void _player_LossFood(object sender, Player.LossFoodArgs e) {
			foodTextDisplayer.LossFoodAmount(e.Loss, e.Current);
		}

		private void _player_GainFood(object sender, Player.GainFoodArgs e) {
			foodTextDisplayer.GainFoodAmount(e.Gain, e.Current);
		}
	}
}