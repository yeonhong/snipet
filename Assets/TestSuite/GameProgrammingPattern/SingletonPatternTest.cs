using NUnit.Framework;
using ProgrammingPattern;
using UnityEngine;

namespace Tests.ProgrammingPattern
{
	public class SingleMono : Singleton<SingleMono>
	{
		public int SingleValue = 0;
	}

	public class SingletonPatternTest
	{
		[SetUp]
		public void Setup() {
			SingleMono.Instance.SingleValue = 100;
		}

		[Test]
		public void 싱글턴값_변경적용() {
			SingleMono.Instance.SingleValue = 1;
			Assert.IsTrue(SingleMono.Instance.SingleValue == 1);
		}

		[Test]
		public void 싱글턴오브젝트찾기() {
			var singletonObject = GameObject.Find("Tests.ProgrammingPattern.SingleMono");
			Assert.IsNotNull(singletonObject);
		}

		[Test]
		public void 한개만생성되는지체크() {
			var objects = GameObject.FindObjectsOfType<SingleMono>();
			Assert.IsTrue(objects.Length == 1);
		}
	}
}
