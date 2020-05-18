using NUnit.Framework;
using System;
using System.Collections;
using RangeAttribute = NUnit.Framework.RangeAttribute;

// namespace로 메뉴에서 계층구조로 분류가 가능하다
namespace NUnitExample
{
	/*
	 * 테스트 명명법 정리
	 * 상위 클래스는 테스트할 대상 클래스이름 + Tests (ex. PlayerTests)
	 * inner클래스의 경우는 대상 클래스의 함수나 프로퍼티를 테스트 (ex. CurrentHpPropertyTests)
	 * 테스트의 경우 [시나리오]_[기대값] (ex. 체력이상으로회복할때_최대체력까지만회복된다)
	 * 
	 * 테스트코드의 순서 // arrage (정렬) // act (동작) // assert (확인)
	 * 
	 * Attribute는 아래 링크에서.
	 * https://github.com/nunit/docs/wiki/Attributes
	 */

	[TestFixture]
	[TestFixtureSource(typeof(TestSourceProvider))]
	public class NUnitExampleTests
	{
		public NUnitExampleTests(string arg, int val) {
			// 테스트 소스에 설정한 만큼 반복 테스트함.
			// Debug.Log($"{arg} {val}");
		}

		[OneTimeSetUp]
		public void BeforeTestsOnce() {
			// 모든 테스트 시작 전에 한번만 실행.
		}

		[OneTimeTearDown]
		public void AfterTestsOnce() {
			// 모든 테스트 끝난 후에 한번만 실행.
		}

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
		[Category("NUnit Example")]
		public void 테스트_무시() {
			Assert.IsTrue(true);
		}

		[Test]
		[Category("NUnit Example")]
		public void Throws테스트() {
			Assert.Throws<ArgumentOutOfRangeException>(() => {
				throw new ArgumentOutOfRangeException();
			});
		}

		[Test]
		public void Values_Range매개변수설정([Values(1, 2, 3)] int x, [Range(0.2f, 0.6f, 0.1f)] float f) {
			// Values에 넣은 값 갯수 만큼 반복 테스트.
			// Range에 넣은 값 범위 만큼 반복 테스트
			// Debug.Log($"{x} {f}");
			Assert.IsTrue(true);
		}

		[TestCase(12, 3, 4)]
		[TestCase(12, 2, 6)]
		[TestCase(12, 4, 3)]
		public void DivideTest(int n, int d, int q) {
			// 테스트 케이스만큼 반복.
			Assert.AreEqual(q, n / d);
		}

		[TestFixture]
		public class InnerTests
		{
			[Test]
			[Category("NUnit Example")]
			public void InnerTest클래스로_관련성을_보여줄수있다() {
				// NunitExampleTests+InnerTests
				Assert.IsTrue(true);
			}
		}
	}

	// https://github.com/nunit/docs/wiki/TestFixtureSource-Attribute 참고.
	internal class TestSourceProvider : IEnumerable
	{
		public IEnumerator GetEnumerator() {
			yield return new object[] { "Arg1", 1 };
			yield return new object[] { "Arg2", 2 };
		}
	}
}
