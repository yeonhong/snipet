using NUnit.Framework;
using Roguelike2D;
using UnityEngine;

namespace ValidationTests
{
	[TestFixture]
	[TestFixtureSource(typeof(GameManagerPrefabProvider))]
	[Category("Validation")]
	public class GameManagerPrefabTests
	{
		private readonly GameObject _prefab;
		public GameManagerPrefabTests(GameObject prefab) {
			_prefab = prefab;
		}

		[TestCase(typeof(GameManager))]
		[TestCase(typeof(BoardManager))]
		public void 기본컴포넌트_체크(System.Type type) {
			Assert.IsNotNull(_prefab.GetComponent(type));
		}

		[Test]
		public void GameManager에_연결된_어셋체크() {
			var gameManager = _prefab.GetComponent<GameManager>();
			Assert.IsTrue(gameManager.ValidateResources());
		}

		[Test]
		public void BoardManager에_연결된_어셋체크() {
			var boardManager = _prefab.GetComponent<BoardManager>();
			Assert.IsTrue(boardManager.ValidateResources());
		}
	}

	internal class GameManagerPrefabProvider : PrefabProvider
	{
		protected override bool FilterPath(string path) {
			return path.Contains("GameManager");
		}
	}
}
