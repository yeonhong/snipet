using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SortTest
{
	public class H_Index
	{
		[Test]
		[TestCase(new int[] { 3, 0, 6, 1, 5 }, ExpectedResult = 3)]
		[TestCase(new int[] { 10, 8, 5, 4, 3 }, ExpectedResult = 4)]
		[TestCase(new int[] { 25, 8, 5, 3, 3 }, ExpectedResult = 3)]
		public int solution(int[] citations) {
			var data = new List<int>(citations);
			var ordered = data.OrderByDescending(o => o);

			int hIndex = 0;
			foreach(var val in ordered) {
				if (val <= hIndex)
					return hIndex;
				hIndex++;
			}

			return hIndex;
		}
	}
}
