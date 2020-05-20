using NUnit.Framework;
using UnityEngine;

namespace ValidationTests
{
	[TestFixture]
	[TestFixtureSource(typeof(SodaPrefabProvider))]
	[Category("Validation")]
	public class SodaPrefabTests
	{
		private readonly GameObject _prefab;
		public SodaPrefabTests(GameObject prefab) {
			_prefab = prefab;
		}

		[Test]
		public void 기본컴포넌트_체크() {
			Assert.IsNotNull(_prefab.GetComponent<SpriteRenderer>());
			Assert.IsNotNull(_prefab.GetComponent<BoxCollider2D>());
		}

		[Test]
		public void SortLayer는_Items이어야한다() {
			var spriterenderer = _prefab.GetComponent<SpriteRenderer>();
			Assert.IsTrue(spriterenderer.sortingLayerName.Equals("Items"));
		}

		[Test]
		public void Tag는_Soda이어야한다() {
			Assert.That(_prefab.tag == "Soda");
		}
	}

	internal class SodaPrefabProvider : PrefabProvider
	{
		protected override bool FilterPath(string path) {
			return path.Contains("Soda");
		}
	}
}
