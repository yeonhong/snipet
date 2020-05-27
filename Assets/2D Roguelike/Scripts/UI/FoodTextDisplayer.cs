using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike2D.UI
{
	public class FoodTextDisplayer : MonoBehaviour
	{
		public Text UIText { get; private set; }

		private void Awake() {
			UIText = GetComponent<Text>();
		}

		public void UpdateFoodAmount(int amount) {
			UIText.text = $"Food: {amount}";
		}

		public void GainFoodAmount(int gainAmount, int curAmount) {
			UIText.text = $"+ {gainAmount} Food: {curAmount}";
		}

		public void LossFoodAmount(int lossAmount, int curAmount) {
			UIText.text = $"- {lossAmount} Food: {curAmount}";
		}
	}
}
