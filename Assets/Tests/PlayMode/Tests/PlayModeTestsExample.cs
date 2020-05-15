using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
	[TestFixture]
	public class PlayModeTestsExample
	{
		[OneTimeSetUp]
		public void LoadTestScene() {
			SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
		}

		[UnityTest]
		public IEnumerator 카메라컴포넌트는_한개이상이어야한다() {
			var cameras = Object.FindObjectsOfType<Camera>();
			Assert.IsTrue(cameras.Length > 0);
			yield return null;
		}

		[UnityTest]
		public IEnumerator MainCamera오브젝트가_있다() {
			var cam = GameObject.Find("Main Camera");
			Assert.IsNotNull(cam);
			yield return null;
		}

		[UnityTest]
		[UnityPlatform(RuntimePlatform.WindowsEditor)]
		//[UnityPlatform(exclude = new[] { RuntimePlatform.WindowsEditor })]
		public IEnumerator GameObject_WithRigidBody_WillBeAffectedByPhysics() {
			var go = new GameObject();
			go.AddComponent<Rigidbody>();
			var originalPosition = go.transform.position.y;

			yield return new WaitForFixedUpdate();

			Assert.AreNotEqual(originalPosition, go.transform.position.y);
		}

		[UnityTest]
		public IEnumerator MonoBehaviourTest_Works() {
			yield return new MonoBehaviourTest<MyMonoBehaviourTest>();
		}

		public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
		{
			private int frameCount;
			public bool IsTestFinished {
				get { return frameCount > 10; }
			}

			void Update() {
				frameCount++;
			}
		}
	}
}
