using NUnit.Framework;
using System;

// namespace로 메뉴에서 계층구조로 분류가 가능하다
namespace Editor
{
	/*
	 * 테스트 명명법 정리
	 * 상위 클래스틑 테스트할 대상 클래스이름 + Tests (ex. PlayerTests)
	 * inner클래스의 경우는 대상 클래스의 함수나 프로퍼티를 테스트 (ex. CurrentHpPropertyTests)
	 * 테스트의 경우 [시나리오]_[기대값] (ex. 체력이상으로회복할때_최대체력까지만회복된다)
	 * 
	 * 테스트코드의 순서 // arrage (정렬) // act (동작) // assert (확인)
	 */

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
