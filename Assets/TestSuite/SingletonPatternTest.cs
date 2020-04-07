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
		}

		[Test]
		public void MakeSingleton() {
			SingleMono.Instance.SingleValue = 1;
			Assert.IsTrue(SingleMono.Instance.SingleValue == 1);
		}

		[Test]
		public void FindSingleton() {
			SingleMono.Instance.SingleValue = 1;
			var singletonObject = GameObject.Find("Tests.ProgrammingPattern.SingleMono");
			Assert.IsNotNull(singletonObject);
		}
	}
}
