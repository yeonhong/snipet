
using System;
using UnityEngine;

namespace TDD_in_Unity
{
	public class Player
	{
		public int CurrentHealth { get; private set; }
		public int MaximumHealth { get; private set; }

		public event EventHandler<HealedEventArgs> Healed;
		public event EventHandler<DamagedEventArgs> Damaged;

		public Player(int currentHealth, int maximumHealth = 12) {
			if (currentHealth < 0) {
				throw new ArgumentOutOfRangeException(nameof(currentHealth), "음수값은 안됨");
			}

			if (currentHealth > maximumHealth) {
				throw new ArgumentOutOfRangeException(nameof(currentHealth), "최대값보다 현재값이 크다");
			}

			CurrentHealth = currentHealth;
			MaximumHealth = maximumHealth;
		}

		public void Heal(int amount) {
			var prev = CurrentHealth;
			CurrentHealth = Mathf.Min(CurrentHealth + amount, MaximumHealth);
			Healed?.Invoke(this, new HealedEventArgs(CurrentHealth - prev));
		}

		public void Damage(int amount) {
			var prev = CurrentHealth;
			CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
			Damaged?.Invoke(this, new DamagedEventArgs(prev - CurrentHealth));
		}

		public class HealedEventArgs : EventArgs
		{
			public HealedEventArgs(int amount) {
				Amount = amount;
			}

			public int Amount { get; private set; }
		}

		public class DamagedEventArgs : EventArgs
		{
			public DamagedEventArgs(int amount) {
				Amount = amount;
			}

			public int Amount { get; private set; }
		}
	}
}