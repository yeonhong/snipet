using NSubstitute;
using NUnit.Framework;
using Roguelike2D;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	[TestFixture]
	public class PlayerTests
	{
		Player player = null;

		[SetUp]
		public void BeforeTests() {
			player = new GameObject().AddComponent<Player>();
			player.gameObject.AddComponent<BoxCollider2D>();
			player.gameObject.AddComponent<Rigidbody2D>();

			var playerManage = Substitute.For<IPlayerManage>();
			playerManage.IsPlayersTurn().Returns(true);
			playerManage.GetPlayerFoodPoints().Returns(100);

			player._playerManage = playerManage;
		}

		[TearDown]
		public void AfterTests() {
			Object.DestroyImmediate(player.gameObject);
		}

		[UnityTest]
		public IEnumerator Player는_Horizontal이_1일때_x가_1로_이동한다() {
			var service = Substitute.For<IUnityService>();
			service.GetAxisRaw("Horizontal").Returns(1);
			player._unityService = service;

			yield return new WaitForSeconds(1f);

			Assert.That(player.transform.position.x == 1f);
		}
	}
}
