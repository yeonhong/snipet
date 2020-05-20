using NUnit.Framework;
using Roguelike2D;
using UnityEngine;

namespace ValidationTests
{
	[TestFixture]
	[TestFixtureSource(typeof(PlayerPrefabProvider))]
	[Category("Validation")]
	public class PlayerPrefabTests
	{
		private readonly GameObject _prefab;
		public PlayerPrefabTests(GameObject prefab) {
			_prefab = prefab;
		}

		[Test]
		public void 기본컴포넌트_체크() {
			Assert.IsNotNull(_prefab.GetComponent<SpriteRenderer>());
			Assert.IsNotNull(_prefab.GetComponent<Animator>());
			Assert.IsNotNull(_prefab.GetComponent<BoxCollider2D>());
			Assert.IsNotNull(_prefab.GetComponent<Rigidbody2D>());
			Assert.IsNotNull(_prefab.GetComponent<Player>());
		}

		[Test]
		public void Tag는_Player이다() {
			Assert.That(_prefab.tag == "Player");
		}

		[Test]
		public void SortingLayer는_Units이다() {
			var sprite = _prefab.GetComponent<SpriteRenderer>();
			Assert.IsTrue(sprite.sortingLayerName.Equals("Units"));
		}
	}

	internal class PlayerPrefabProvider : PrefabProvider
	{
		protected override bool FilterPath(string path) {
			return path.Contains("Player");
		}
	}
}
