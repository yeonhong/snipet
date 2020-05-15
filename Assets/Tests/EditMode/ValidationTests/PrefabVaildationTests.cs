using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ValidationTests
{
	[TestFixture]
	[TestFixtureSource(typeof(PrefabProvider))]
	[Category("Validation")]
	public class PrefabValidationTests
	{
		private readonly GameObject _prefab;
		private readonly Transform _transform;
		public PrefabValidationTests(GameObject prefab) {
			_prefab = prefab;
			_transform = prefab.transform;
		}

		[Test]
		public void 프리팹은_두개의_자식을_가진다() {
			Assert.That(_transform.childCount == 2);
		}

		[Test]
		public void Foreground의_이미지는_fillamount가_1이다() {
			var image = _transform.GetChild(1).GetComponent<Image>();
			Assert.That(image.fillAmount == 1f);
		}
	}

	internal class PrefabProvider : IEnumerable<GameObject>
	{
		private readonly string[] _prefabFolders = { "Assets/Tests/Prefabs" };

		public IEnumerator<GameObject> GetEnumerator() {

			foreach (var guid in AssetDatabase.FindAssets("t:prefab", _prefabFolders)) {
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var prefab = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;
				yield return prefab;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable<GameObject>)this).GetEnumerator();
		}
	}
}
