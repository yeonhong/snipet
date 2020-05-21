using NUnit.Framework;
using Roguelike2D;
using UnityEngine;

namespace ValidationTests
{
	[TestFixture]
	[TestFixtureSource(typeof(WallPrefabProvider))]
	[Category("Validation")]
	public class WallPrefabValidationTests
	{
		private readonly GameObject _prefab;
		public WallPrefabValidationTests(GameObject prefab) {
			_prefab = prefab;
			
		}

		[TestCase(typeof(SpriteRenderer))]
		[TestCase(typeof(BoxCollider2D))]
		[TestCase(typeof(Wall))]
		public void 기본컴포넌트_체크(System.Type type) {
			Assert.IsNotNull(_prefab.GetComponent(type));
		}

		[Test]
		public void BlockingLayer설정체크() {
			var layer = LayerMask.NameToLayer("BlockingLayer");
			Assert.IsTrue(_prefab.layer == layer);
		}

		[Test]
		public void 이미지의_소팅레이어는_Items이다() {
			var spriterenderer = _prefab.GetComponent<SpriteRenderer>();
			Assert.IsTrue(spriterenderer.sortingLayerName.Equals("Items"));
		}

		[Test]
		public void Wall컴포넌트에_MissingLink가_있는지_체크() {
			Wall wall = GetWallComponent();
			Assert.That(wall.chopSound1 != null && wall.chopSound2 != null && wall.dmgSprite != null);
		}

		[Test]
		public void Wall의_체력이_0_이상인가() {
			var wall = GetWallComponent();
			Assert.IsTrue(wall.hp > 0);
		}

		private Wall GetWallComponent() {
			return _prefab.GetComponent<Wall>();
		}
	}

	internal class WallPrefabProvider : PrefabProvider
	{
		protected override bool FilterPath(string path) {
			return (path.Contains("Wall") && !path.Contains("Outer"));
		}
	}
}
