using NUnit.Framework;
using System;

// namespace로 메뉴에서 계층구조로 분류가 가능하다
namespace Editor
{
	[TestFixture]
	public class NUnitExampleTests
	{
		[SetUp]
		public void BeforeTests() {
			// 각 테스트 전에 실행.
		}

		[TearDown]
		public void AfterTests() {
			// 각 테스트 후에 실행.
		}

		[Test]
		[Category("NUnit Example")]
		public void 기본성공() {
			Assert.IsTrue(true);
		}

		[Test]
		[Ignore("무시합니다.")]
		public void 테스트_무시() {
			Assert.IsTrue(true);
		}

		[Test]
		public void Throws테스트() {
			Assert.Throws<ArgumentOutOfRangeException>(() => {
				throw new ArgumentOutOfRangeException();
			});
		}

		[TestFixture]
		public class InnerTests
		{
			[Test]
			public void InnerTest클래스로_관련성을_보여줄수있다() {
				// NunitExampleTests+InnerTests
				Assert.IsTrue(true);
			}
		}
	}
}
