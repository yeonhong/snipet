using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Roguelike2D;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	[TestFixture]
    public class MovingObjectTests
    {
		class MovingObject : Roguelike2D.MovingObject
		{
			protected override void OnCantMove<T>(T component) {
			}

			public void Move(int xDir, int yDir) {
				base.AttemptMove<MovingObject>(xDir, yDir);
			}
		}

		[UnityTest]
        public IEnumerator 오브젝트의생성()
        {
			var go = new GameObject().AddComponent<MovingObject>();
            yield return null;
        }

		[TestFixture]
		public class Move메소드
		{
			private MovingObject mObject;

			[SetUp]
			public void BeforeTests() {
				GameObject obj = new GameObject();
				obj.AddComponent<BoxCollider2D>();
				obj.AddComponent<Rigidbody2D>();
				mObject = obj.AddComponent<MovingObject>();

				var dummyService = Substitute.For<IUnityService>();
				dummyService.GetDeltaTime().Returns(1f);
				mObject._unityService = dummyService;
			}

			[TearDown]
			public void AfterTests() {
				Object.DestroyImmediate(mObject.gameObject);
			}

			[UnityTest]
			//[TestCase(0, 0, ExpectedResult = null)]
			[TestCase(1, 0, ExpectedResult = null)]
			[TestCase(0, 1, ExpectedResult = null)]
			[TestCase(1, 1, ExpectedResult = null)]
			[TestCase(-1, 0, ExpectedResult = null)]
			[TestCase(0, -1, ExpectedResult = null)]
			[TestCase(-1, -1, ExpectedResult = null)]
			public IEnumerator Move메소드호출(int xDir, int yDir) {
				mObject.transform.position = Vector3.zero;

				Vector2 start = mObject.transform.position;
				Vector2 end = start + new Vector2(xDir, yDir);

				mObject.Move(xDir, yDir);
				yield return new WaitForSeconds(.1f);

				Vector2 finish = mObject.transform.position;
				Assert.That(MovingObject.AlmostClosed(end, finish), $"{end} != {finish}");
			}
		}
    }
}
