using System;
using System.Collections.Generic;
using NUnit.Framework;
using TDD_in_Unity;
using TDD_in_Unity.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainerTests
{
	Image _image_last;
	Image _image_first;
	HeartContainer _heartContainer;

	[SetUp]
	public void BeforeTests() {
		_image_last = An.Image();
		_image_last.fillAmount = 0f;
		_image_first = An.Image();
		_image_first.fillAmount = 0f;

		_heartContainer = new HeartContainer(new List<Heart> {
			A.Heart().With(_image_first),
			A.Heart().With(_image_last),
		});
	}

	public class Replenish함수 : HeartContainerTests
	{
		[Test]
		public void _0_0에서_0으로_설정하기() {
			_heartContainer.Replenish(0);
			Assert.AreEqual(0, _image_last.fillAmount);
		}

		[Test]
		public void _1_0에서_25퍼센트로_설정하기() {
			_image_first.fillAmount = 1f;
			_image_last.fillAmount = 0f;

			_heartContainer.Replenish(1);
			Assert.AreEqual(0.25f, _image_last.fillAmount);
		}

		[Test]
		public void _1_완전_0에서_25퍼센트로_설정하기() {
			_image_first.fillAmount = 0f;
			_image_last.fillAmount = 0f;

			_heartContainer.Replenish(1);
			Assert.AreEqual(0.25f, _image_first.fillAmount);
			Assert.AreEqual(0f, _image_last.fillAmount);
		}

		[Test]
		public void _2_음수를_넣었을때_ArgumentOutOfRangeException_발생() {
			Assert.Throws<ArgumentOutOfRangeException>(() => _heartContainer.Replenish(-1));
		}
	}
}
