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
		private readonly Transform _transform;
		public WallPrefabValidationTests(GameObject prefab) {
			_prefab = prefab;
			_transform = prefab.transform;
		}

		[Test]
		public void 기본컴포넌트_체크() {
			Assert.IsNotNull(_prefab.GetComponent<SpriteRenderer>());
			Assert.IsNotNull(_prefab.GetComponent<BoxCollider2D>());
			Assert.IsNotNull(_prefab.GetComponent<Wall>());
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
			var wall = _prefab.GetComponent<Wall>();
			Assert.IsNotNull(wall.chopSound1);
			Assert.IsNotNull(wall.chopSound2);
			Assert.IsNotNull(wall.dmgSprite);
			Assert.IsTrue(wall.hp > 0);
		}
	}

	internal class WallPrefabProvider : PrefabProvider
	{
		protected override bool FilterPath(string path) {
			return (path.Contains("Wall") && !path.Contains("Outer"));
		}
	}
}
