using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Basic
{
	public class 테스트
	{
		[Test]
		[TestCase(
		
		new string[] { "classic", "pop", "classic", "classic", "pop" },
		new int[] { 500, 600, 150, 800, 2500 },

		ExpectedResult = new int[] { })]

		public int[] solution(string[] genres, int[] plays) {
			List<int> lstAnswer = new List<int>();
			return lstAnswer.ToArray();
		}
	}
}
