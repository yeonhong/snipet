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

		[TestCase(typeof(SpriteRenderer))]
		[TestCase(typeof(Animator))]
		[TestCase(typeof(BoxCollider2D))]
		[TestCase(typeof(Rigidbody2D))]
		[TestCase(typeof(Player))]
		public void 기본컴포넌트_체크(System.Type type) {
			Assert.IsNotNull(_prefab.GetComponent(type));
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

		[Test]
		public void Play에_설정한_리소스_체크() {
			var player = _prefab.GetComponent<Player>();
			Assert.IsTrue(player.ValidateResources());
		}
	}

	internal class PlayerPrefabProvider : PrefabProvider
	{
		protected override bool FilterPath(string path) {
			return path.Contains("Player");
		}
	}
}
