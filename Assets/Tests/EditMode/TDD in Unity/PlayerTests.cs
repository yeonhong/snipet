using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;
using TDD_in_Unity;

public class PlayerTests
{
	public class CurrentHealth속성
	{
		[Test]
		public void Health의_기본값은_0이다() {
			var player = new Player(0);

			Assert.AreEqual(0, player.CurrentHealth);
		}

		[Test]
		public void Health의_초기값은_0이하의_값을_넣을수_없다() {
			Assert.Throws<ArgumentOutOfRangeException>(() => {
				new Player(-1);
			});
		}

		[Test]
		public void Health의_초기값은_최대값을_넘길수_없다() {
			Assert.Throws<ArgumentOutOfRangeException>(() => {
				new Player(2, 1);
			});
		}
	}

	public class Heal함수
	{
		[Test]
		public void _0을_넣으면_아무일도_안생긴다() {
			var player = new Player(0);

			player.Heal(0);

			Assert.AreEqual(0, player.CurrentHealth);
		}

		[Test]
		public void 체력을_1회복시키기() {
			var player = new Player(0);

			player.Heal(1);

			Assert.AreEqual(1, player.CurrentHealth);
		}

		[Test]
		public void 최대체력이상으로_회복되지_않는다() {
			var player = new Player(0);

			player.Heal(100);

			Assert.AreEqual(player.MaximumHealth, player.CurrentHealth);
		}
	}

	public class Damage함수
	{
		[Test]
		public void 데미지를_0을_받으면_아무일도_안생긴다() {
			var player = new Player(1);

			player.Damage(0);

			Assert.AreEqual(1, player.CurrentHealth);
		}

		[Test]
		public void 데미지를_1을_받으면_1이_깍인다() {
			var player = new Player(1);

			player.Damage(1);

			Assert.AreEqual(0, player.CurrentHealth);
		}

		[Test]
		public void 데미지를_크게받아도_0이하로_내려가지_않는다() {
			var player = new Player(1);

			player.Damage(100);

			Assert.AreEqual(0, player.CurrentHealth);
		}
	}

	public class Heal이벤트
	{
		[Test]
		public void 힐이벤트발생() {
			var amount = -1;
			var player = new Player(1);

			player.Healed += (sender, args) => {
				amount = args.Amount;
			};

			player.Heal(0);

			Assert.AreEqual(0, amount);
		}

		[Test]
		public void 오버힐링은_무시한다() {
			var amount = -1;
			var player = new Player(1,1);

			player.Healed += (sender, args) => {
				amount = args.Amount;
			};

			player.Heal(100);

			Assert.AreEqual(0, amount);
		}
	}

	public class Damage이벤트
	{
		[Test]
		public void 데미지이벤트발생() {
			var amount = -1;
			var player = new Player(1);

			player.Damaged += (sender, args) => {
				amount = args.Amount;
			};

			player.Damage(1);

			Assert.AreEqual(1, amount);
		}

		[Test]
		public void 오버데미지는_무시한다() {
			var amount = -1;
			var player = new Player(0, 1);

			player.Damaged += (sender, args) => {
				amount = args.Amount;
			};

			player.Damage(100);

			Assert.AreEqual(0, amount);
		}
	}
}
