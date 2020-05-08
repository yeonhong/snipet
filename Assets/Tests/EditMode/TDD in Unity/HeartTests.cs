using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;
using TDD_in_Unity;

public class Heart_Tests
{
	private Image _image;
	private Heart _heart;

	[SetUp]
	public void BeforeEveryTest() {
		_image = new GameObject().AddComponent<Image>();
		_heart = new Heart(_image);
	}

	public class Replenish함수 : Heart_Tests
	{
		[Test]
		public void _0_하트를_0으로_채우는_테스트() {
			// arrange 
			_image.fillAmount = 0f;

			// act
			_heart.Replenish(0);

			// assert
			Assert.AreEqual(0, _image.fillAmount);
		}

		[Test]
		public void _1_하트를_1단계_채우기() {
			_image.fillAmount = 0f;

			_heart.Replenish(1);

			Assert.AreEqual(0.25f, _image.fillAmount);
		}

		[Test]
		public void _1_하트를_1단계상태에서_1을채워서_2로만들기() {
			_image.fillAmount = 0.25f;

			_heart.Replenish(1);

			Assert.AreEqual(0.5f, _image.fillAmount);
		}

		[Test]
		public void _2_마이너스값이오면_ArgumentOutOfRangeException를_밷기() {
			Assert.Throws<ArgumentOutOfRangeException>(() => _heart.Replenish(-1));
		}
	}

	public class Depleate함수 : Heart_Tests
	{
		[Test]
		public void _0_100프로상태에서_0단계깍기() {
			_image.fillAmount = 1f;

			_heart.Deplate(0);

			Assert.AreEqual(1f, _image.fillAmount);
		}

		[Test]
		public void _1_100프로상태에서_1단계깍기_75프로상태로() {
			_image.fillAmount = 1f;

			_heart.Deplate(1);

			Assert.AreEqual(0.75f, _image.fillAmount);
		}

		[Test]
		public void _2_마이너스값이오면_ArgumentOutOfRangeException를_밷기() {
			Assert.Throws<ArgumentOutOfRangeException>(() => _heart.Deplate(-1));
		}
	}
}
