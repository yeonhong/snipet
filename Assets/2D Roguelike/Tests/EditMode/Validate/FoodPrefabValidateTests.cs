using NUnit.Framework;
using Roguelike2D;
using UnityEngine;

namespace ValidationTests
{
	[TestFixture]
	[TestFixtureSource(typeof(FoodPrefabProvider))]
	[Category("Validation")]
	public class FoodPrefabTests
	{
		private readonly GameObject _prefab;
		public FoodPrefabTests(GameObject prefab) {
			_prefab = prefab;
		}

		[TestCase(typeof(SpriteRenderer))]
		[TestCase(typeof(BoxCollider2D))]
		[TestCase(typeof(FoodObject))]
		public void 기본컴포넌트_체크(System.Type type) {
			Assert.IsNotNull(_prefab.GetComponent(type));
		}

		[Test]
		public void SortLayer는_Items이어야한다() {
			var spriterenderer = _prefab.GetComponent<SpriteRenderer>();
			Assert.IsTrue(spriterenderer.sortingLayerName.Equals("Items"));
		}

		[Test]
		public void Tag는_Soda이어야한다() {
			Assert.That(_prefab.tag == "Food");
		}
	}

	internal class FoodPrefabProvider : PrefabProvider
	{
		protected override bool FilterPath(string path) {
			return path.Contains("Food");
		}
	}
}
