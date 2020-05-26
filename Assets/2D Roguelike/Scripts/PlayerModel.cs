using System;

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
}