using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

	public class 가장큰수
	{
		[Test]
		[TestCase(new int[] { 1000, 0 }, ExpectedResult = "10000")]
		[TestCase(new int[] { 6, 10, 2 }, ExpectedResult = "6210")]
		[TestCase(new int[] { 3, 30, 34, 5, 9 }, ExpectedResult = "9534330")]
		[TestCase(new int[] { 2, 2 }, ExpectedResult = "22")]
		[TestCase(new int[] { 0, 0 }, ExpectedResult = "0")]
		public string solution_my(int[] numbers) {
			List<string> elements = new List<string>();
			foreach(var e in numbers) {
				elements.Add(e.ToString());
			}

			elements.Sort(new Comparer());
			
			StringBuilder sb = new StringBuilder();
			foreach (var e in elements) {
				sb.Append(e);
			}

			var ret = sb.ToString().TrimStart('0');
			return ret == string.Empty ? "0" : ret;
		}

		class Comparer : IComparer<string>
		{
			public int Compare(string x, string y) {
				var left = int.Parse(string.Format($"{x}{y}"));
				var right = int.Parse(string.Format($"{y}{x}"));
				return right.CompareTo(left);
			}
		}

		[Test]
		[TestCase(new int[] { 1000, 0 }, ExpectedResult = "10000")]
		[TestCase(new int[] { 6, 10, 2 }, ExpectedResult = "6210")]
		[TestCase(new int[] { 3, 30, 34, 5, 9 }, ExpectedResult = "9534330")]
		[TestCase(new int[] { 2, 2 }, ExpectedResult = "22")]
		[TestCase(new int[] { 0, 0 }, ExpectedResult = "0")]
		public string solution_site(int[] numbers) {
			Array.Sort(numbers, (x, y) =>
			{
				string XY = x.ToString() + y.ToString();
				string YX = y.ToString() + x.ToString();
				return YX.CompareTo(XY);
			});
			if (numbers.Where(x => x == 0).Count() == numbers.Length) return "0";
			else return string.Join("", numbers);
		}
	}
}
