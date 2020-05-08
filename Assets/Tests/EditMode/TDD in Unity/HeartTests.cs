using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class Heart_Tests
{
	public class Replenish함수
	{
		Image _image;
		Heart _heart;

		[SetUp]
		public void BeforeEveryTestSetUp() {
			_image = new GameObject().AddComponent<Image>();
			_heart = new Heart(_image);
		}

		[Test]
		public void _0_하트를_0으로_채우는_테스트() {
			_image.fillAmount = 0f;
			_heart.Replenish(0);

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
	}
}

public class Heart
{
	private const float FillPerHeartPiece = 0.25f;
	private Image _image;

	public Heart(Image image) {
		_image = image;
	}

	internal void Replenish(int numberOfHeartPieces) {
		_image.fillAmount += numberOfHeartPieces * FillPerHeartPiece;
	}
}
