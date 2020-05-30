using NUnit.Framework;
using Roguelike2D;

namespace Tests
{
	[TestFixture]
	public class EnemyManagerTests
	{
		[Test]
		public void 생성테스트() {
			var enemyManager = new EnemyManager(1f);
			Assert.IsNotNull(enemyManager.Enemies);
		}
	}
}
