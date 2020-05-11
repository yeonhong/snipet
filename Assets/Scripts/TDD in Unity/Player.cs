
using System;
using UnityEngine;

namespace TDD_in_Unity
{
	public class Player
	{
		public int CurrentHealth { get; private set; }
		public int MaximumHealth { get; private set; }

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
			CurrentHealth = Mathf.Min(CurrentHealth + amount, MaximumHealth);
		}

		public void Damage(int amount) {
			CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
		}
	}
}