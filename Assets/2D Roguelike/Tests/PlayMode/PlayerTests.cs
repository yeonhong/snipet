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
			player._playerManager = playerManage;

			var soundManager = Substitute.For<ISoundManager>();
			player._soundManager = soundManager;

			player.transform.position = Vector3.zero;
		}

		[TearDown]
		public void AfterTests() {
			Object.DestroyImmediate(player.gameObject);
		}

		[UnityTest]
		[UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.WindowsPlayer)]
		public IEnumerator Player는_Horizontal이_1일때_x가_양수쪽으로_이동한다() {
			var service = Substitute.For<IUnityService>();
			service.GetAxisRaw("Horizontal").Returns(1);
			service.GetDeltaTime().Returns(1f);
			player._unityService = service;

			yield return new WaitForSeconds(.1f);

			var posX = player.transform.position.x;
			Assert.That(posX > 0f, $"{posX}");
		}

		[UnityTest]
		[UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.WindowsPlayer)]
		public IEnumerator Player는_Vertical이_1일때_y가_양수쪽으로_이동한다() {
			var service = Substitute.For<IUnityService>();
			service.GetAxisRaw("Vertical").Returns(1);
			service.GetDeltaTime().Returns(1f);
			player._unityService = service;

			yield return new WaitForSeconds(.1f);

			var posY = player.transform.position.y;
			Assert.That(posY > 0f, $"{posY}");
		}
	}
}
