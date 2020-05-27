using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Roguelike2D.UI;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.UI
{
	// todo : food text displayer 테스트 작성.
    public class FoodTextDisplayerTests
    {
		GameObject _foodTextObject;
		FoodTextDisplayer _foodTextDisplayer;

		[SetUp]
		public void BeforeTests() {
			_foodTextObject = new GameObject();
			_foodTextObject.AddComponent<Text>();
			_foodTextDisplayer = _foodTextObject.AddComponent<FoodTextDisplayer>();
		}

        [UnityTest]
        public IEnumerator _0_생성테스트()
        {
			yield return null;
			Assert.IsNotNull(_foodTextDisplayer.UIText);
        }

		[Test]
		public void _1_Food갯수를_갱신한다() {
			int amount = 10;

			_foodTextDisplayer.UpdateFoodAmount(amount);

			Assert.AreEqual($"Food: {amount}", _foodTextDisplayer.UIText.text);
		}

		[Test]
		public void _2_Food를_획득했을때의_표시() {
			int curAmount = 10;
			int gainAmount = 10;

			_foodTextDisplayer.GainFoodAmount(gainAmount, curAmount);

			Assert.AreEqual($"+ {gainAmount} Food: {curAmount}", _foodTextDisplayer.UIText.text);
		}

		[Test]
		public void _2_Food를_잃었을때의_표시() {
			int curAmount = 10;
			int lossAmount = 10;

			_foodTextDisplayer.LossFoodAmount(lossAmount, curAmount);

			Assert.AreEqual($"- {lossAmount} Food: {curAmount}", _foodTextDisplayer.UIText.text);
		}
	}
}
