using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ValidationTests
{
	internal class PrefabProvider : IEnumerable<GameObject>
	{
		protected readonly string[] _prefabFolders = { "Assets/2D Roguelike/Prefabs" };

		protected virtual bool FilterPath(string path) {
			return true;
		}

		public IEnumerator<GameObject> GetEnumerator() {

			foreach (var guid in AssetDatabase.FindAssets("t:prefab", _prefabFolders)) {
				var path = AssetDatabase.GUIDToAssetPath(guid);
				if (!FilterPath(path)) continue;
				var prefab = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;
				yield return prefab;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable<GameObject>)this).GetEnumerator();
		}
	}
}