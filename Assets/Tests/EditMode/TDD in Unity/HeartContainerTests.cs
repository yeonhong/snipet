﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using TDD_in_Unity;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainerTests
{
	Image _image_last;
	Image _image_first;
	HeartContainer _heartContainer;

	[SetUp]
	public void BeforeTests() {
		_image_last = new GameObject().AddComponent<Image>();
		_image_last.fillAmount = 0f;
		_image_first = new GameObject().AddComponent<Image>();
		_image_first.fillAmount = 0f;

		_heartContainer = new HeartContainer(new List<Heart> { new Heart(_image_first), new Heart(_image_last) });
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
	}
}

public class HeartContainer
{
	private readonly List<Heart> _list;

	public HeartContainer(List<Heart> list) {
		_list = list;
	}

	public void Replenish(int numberOfHeartPieces) {
		foreach(var heart in _list) {
			if (numberOfHeartPieces > 0) {
				var chargeCount = (Heart.HeartPiecesOfHeart - heart.CurrentNumberOfHeartPieces);
				chargeCount = Mathf.Min(chargeCount, numberOfHeartPieces);
				heart.Replenish(chargeCount);
				numberOfHeartPieces -= chargeCount;
			}
		}
	}
}
