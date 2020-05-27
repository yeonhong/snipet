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

		[TestFixture]
		[UnityPlatform(RuntimePlatform.WindowsEditor, 
			RuntimePlatform.Android, RuntimePlatform.OSXPlayer)]
		public class MobilePlatform
		{
			[UnityTest]
			public IEnumerator _0_기본생성() {
				var dummy = Substitute.For<IUnityService>();
				var moveContoller = new InputController_Mobile(dummy);

				yield return null;

				moveContoller.MoveController(out int horizontal, out int vertical);
				Assert.That(horizontal == 0 && vertical == 0);
			}

			[UnityTest]
			public IEnumerator _1_이동입력의_크기가_1보다_작으면_동작하지않는다() {
				var dummy = Substitute.For<IUnityService>();
				dummy.IsMouseButtonDown().Returns(true);
				dummy.GetMousePosition().Returns(Vector3.zero);
				var moveContoller = new InputController_Mobile(dummy);
				moveContoller.MoveController(out int h, out int v);

				yield return null;
				dummy.IsMouseButtonDown().Returns(false);
				dummy.IsMouseButtonUp().Returns(true);
				dummy.GetMousePosition().Returns(new Vector3(1, 0, 0));
				moveContoller.MoveController(out int horizontal, out int vertical);

				Assert.That(horizontal == 0 && vertical == 0, $"{horizontal} {vertical} 은 0이어야함");
			}

			[UnityTest]
			[TestCase(2, 0, ExpectedResult = null)]
			[TestCase(-2, 0, ExpectedResult = null)]
			[TestCase(0, 2, ExpectedResult = null)]
			[TestCase(0, -2, ExpectedResult = null)]
			public IEnumerator _2_기본이동테스트(float endX, float endY) {
				var dummy = Substitute.For<IUnityService>();
				dummy.IsMouseButtonDown().Returns(true);
				dummy.GetMousePosition().Returns(Vector3.zero);
				var moveContoller = new InputController_Mobile(dummy);
				moveContoller.MoveController(out int h, out int verticalv);

				yield return null;
				dummy.IsMouseButtonDown().Returns(false);
				dummy.IsMouseButtonUp().Returns(true);
				dummy.GetMousePosition().Returns(new Vector3(endX, endY, 0f));
				moveContoller.MoveController(out int horizontal, out int vertical);

				if(endX > 0 || endX < 0) {
					Assert.AreEqual(Mathf.Abs(horizontal), 1);
				} else {
					Assert.AreEqual(Mathf.Abs(vertical), 1);
				}
			}

			[UnityTest]
			[TestCase(1, 1, ExpectedResult = null)]
			[TestCase(-1, -1, ExpectedResult = null)]
			public IEnumerator _3_X축_우선_값을반영합니다(float endX, float endY) {
				var dummy = Substitute.For<IUnityService>();
				dummy.IsMouseButtonDown().Returns(true);
				dummy.GetMousePosition().Returns(Vector3.zero);
				var moveContoller = new InputController_Mobile(dummy);
				moveContoller.MoveController(out int h, out int verticalv);

				yield return null;
				dummy.IsMouseButtonDown().Returns(false);
				dummy.IsMouseButtonUp().Returns(true);
				dummy.GetMousePosition().Returns(new Vector3(endX, endY, 0f));
				moveContoller.MoveController(out int horizontal, out int vertical);

				Assert.AreEqual(Mathf.Abs(horizontal), 1);
			}
		}
	}
}
