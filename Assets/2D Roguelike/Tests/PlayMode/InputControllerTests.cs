using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Roguelike2D;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InputControllerTests
    {
		[TestFixture]
		[UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.WindowsPlayer)]
		public class StandalonePlatform
		{
			[UnityTest]
			public IEnumerator 기본생성() {
				var dummy = Substitute.For<IUnityService>();
				var moveContoller = new InputController_Standalone(dummy);

				yield return null;

				moveContoller.MoveController(out int horizontal, out int vertical);
				Assert.That(horizontal == 0 && vertical == 0);
			}

			[UnityTest]
			[TestCase(1, 0, ExpectedResult = null)]
			[TestCase(-1, 0, ExpectedResult = null)]
			[TestCase(0, 1, ExpectedResult = null)]
			[TestCase(0, -1, ExpectedResult = null)]
			public IEnumerator 기본이동테스트(int xDir, int yDir) {
				var dummy = Substitute.For<IUnityService>();
				dummy.GetAxisRaw("Horizontal").Returns(xDir);
				dummy.GetAxisRaw("Vertical").Returns(yDir);

				var moveContoller = new InputController_Standalone(dummy);

				yield return null;

				moveContoller.MoveController(out int horizontal, out int vertical);
				Assert.That(horizontal == xDir && vertical == yDir,
					$"{horizontal} == {xDir} && {vertical} == {yDir}");
			}

			[UnityTest]
			[TestCase(1, 1, ExpectedResult = null)]
			[TestCase(-1, -1, ExpectedResult = null)]
			public IEnumerator X축_우선_값을반영합니다(int xDir, int yDir) {
				var dummy = Substitute.For<IUnityService>();
				dummy.GetAxisRaw("Horizontal").Returns(xDir);
				dummy.GetAxisRaw("Vertical").Returns(yDir);

				var moveContoller = new InputController_Standalone(dummy);

				yield return null;

				moveContoller.MoveController(out int horizontal, out int vertical);
				Assert.That(horizontal == xDir && vertical == 0,
					$"{horizontal} == {xDir} && {vertical} == {0}");
			}
		}

		// todo : 모바일 플랫폼용 이동 테스트 추가하기
		[TestFixture]
		//[UnityPlatform(RuntimePlatform.Android, RuntimePlatform.OSXPlayer)]
		public class MobilePlatform
		{
			[UnityTest]
			public IEnumerator 기본생성() {
				var dummy = Substitute.For<IUnityService>();
				var moveContoller = new InputController_Mobile(dummy);

				yield return null;

				moveContoller.MoveController(out int horizontal, out int vertical);
				Assert.That(horizontal == 0 && vertical == 0);
			}
		}
	}
}
