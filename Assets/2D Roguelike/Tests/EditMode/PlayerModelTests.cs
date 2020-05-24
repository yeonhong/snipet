using NUnit.Framework;
using Roguelike2D;
using System;

namespace UnitTests
{
	[TestFixture]
	public class PlayerModelTests
	{
		[Test]
		public void PlayerModel의생성() {
			var model = new PlayerModel(1);
			Assert.That(model.Food == 1);
		}

		[TestFixture]
		public class LoseFood_메소드
		{
			private const int InitFood = 1;
			private PlayerModel _model;
			[SetUp]
			public void BeforeTests() {
				_model = new PlayerModel(InitFood);
			}

			[Test]
			public void _0으로설정하면_변하지않는다() {
				var preValue = _model.Food;

				_model.LoseFood(0);

				Assert.That(_model.Food == preValue);
			}

			[Test]
			public void _설정한_값만큼_줄어든다() {
				var preValue = _model.Food;
				const int lossAmount = 1;

				_model.LoseFood(lossAmount);

				Assert.That(_model.Food == preValue - lossAmount);
			}

			[Test]
			public void _0이하로_줄어들지_않는다() {

				_model.LoseFood(_model.Food + 1);

				Assert.That(_model.Food >= 0);
			}

			[Test]
			public void _음수를_넣으면_Exception발생() {
				Assert.Catch<ArgumentOutOfRangeException>(() => { _model.LoseFood(-1); });
			}
		}

		[TestFixture]
		public class GainFood_메소드
		{
			PlayerModel _model;

			[SetUp]
			public void BeforeTests() {
				_model = new PlayerModel(1);
			}

			[Test]
			public void _0으로설정하면_아무것도변하지않음() {
				var preValue = _model.Food;

				_model.GainFood(0);

				Assert.That(_model.Food == preValue);
			}

			[Test]
			public void _값을넣은만큼_올라간다() {
				var preValue = _model.Food;
				var amount = 1;

				_model.GainFood(amount);

				Assert.That(_model.Food == preValue + amount);
			}

			[Test]
			public void _음수를넣으면_Exception발생() {
				Assert.Catch<ArgumentOutOfRangeException>(() => { _model.GainFood(-1); });
			}
		}

		[TestFixture]
		public class IsEmpty_프로퍼티
		{
			[Test]
			public void Food가_0이면_True() {
				var model = new PlayerModel(0);

				Assert.That(model.IsFoodEmpty == true);
			}

			[Test]
			public void Food가_0보다_크면_False() {
				var model = new PlayerModel(1);

				Assert.That(model.IsFoodEmpty == false);
			}
		}
	}
}
