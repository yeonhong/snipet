using NUnit.Framework;
using System;
using System.Collections.Generic;
using TDD_in_Unity.Infrastructure;
using UnityEngine.UI;

namespace TDD_in_Unity
{
	[TestFixture]
	public class HeartContainerTests
	{
		private Image _image_last;
		private Image _image_first;
		private HeartContainer _heartContainer;

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

			[Test]
			public void _3_다중_하트에_적용할수_있는지() {
				((HeartContainer)A.HeartContainer().With(
					A.Heart().With(An.Image().WithFillAmount(0.75f)),
					A.Heart().With(_image_last))).Replenish(2);

				Assert.AreEqual(0.25f, _image_last.fillAmount);
			}
		}

		public class Deplete함수
		{
			private Image target;

			[SetUp]
			public void BeforeTests() {
				target = An.Image().WithFillAmount(1f);
			}

			[Test]
			public void _0_100퍼센트일때_0을깎았을때() {
				((HeartContainer)A.HeartContainer().With(
						A.Heart().With(target))).
						Deplete(0);

				Assert.AreEqual(1f, target.fillAmount);
			}

			[Test]
			public void _1_100퍼센트일때_1을깎으면_75퍼센트가_된다() {
				((HeartContainer)A.HeartContainer().With(
					A.Heart().With(target))).
					Deplete(1);

				Assert.AreEqual(0.75f, target.fillAmount);
			}

			[Test]
			public void _1_두개의_하트에서_깍을때의_테스트() {
				((HeartContainer)A.HeartContainer().With(
					A.Heart().With(target),
					A.Heart().With(An.Image().WithFillAmount(0.25f)
					))).
					Deplete(2);

				Assert.AreEqual(0.75f, target.fillAmount);
			}
		}
	}
}
