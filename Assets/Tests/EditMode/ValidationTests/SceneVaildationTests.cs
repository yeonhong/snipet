using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ValidationTests
{
	[TestFixture]
	[TestFixtureSource(typeof(SceneProvider))]
	[Category("Validation")]
	public class SceneValidationTests
	{
		private readonly string _scenePath;
		private Scene _scene;

		public SceneValidationTests(string scenePath) {
			_scenePath = scenePath;
		}

		[OneTimeSetUp]
		public void LoadScene() {
			_scene = SceneManager.GetSceneAt(0);
			if (SceneManager.sceneCount > 1 || _scene.path != _scenePath) {
				_scene = EditorSceneManager.OpenScene(_scenePath, OpenSceneMode.Single);
			}
		}

		[Test]
		public void 테스트씬에는_카메라가_한개는_있어야한다() {
			var cameras = Object.FindObjectsOfType<Camera>();
			Assert.That(cameras.Length > 0);
		}
	}

	internal class SceneProvider : IEnumerable<string>
	{
		public IEnumerator<string> GetEnumerator() {

			foreach (var scene in EditorBuildSettings.scenes) {
				if (!scene.enabled || scene.path == null ||
					!Path.GetFileName(scene.path).StartsWith("Test")) {
					continue;
				}

				yield return scene.path;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}
}
