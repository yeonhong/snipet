using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BruteForceSearchTest
{
	public class 카펫
	{
		[Test]
		[TestCase(8, 1, ExpectedResult = new int[] { 3, 3 })]
		[TestCase(10, 2, ExpectedResult = new int[] { 4, 3 })]
		[TestCase(24, 24, ExpectedResult = new int[] { 8, 6 })]
		//갈색 격자의 수 brown은 8 이상 5,000 이하인 자연수입니다.
		//노란색 격자의 수 yellow는 1 이상 2,000,000 이하인 자연수입니다.
		//카펫의 가로 길이는 세로 길이와 같거나, 세로 길이보다 깁니다.
		public int[] MySolution(int brown, int yellow) {
			int width = 3, height = 3;

			while (width * height != brown + yellow) {
				width++;

				if (width * height == brown + yellow) {
					if (brown != (2 * width) + (2 * height) - 4) {
						height++;
						width = height;
					}
					else {
						break;
					}
				}
				else if (width * height > brown + yellow) {
					height++;
					width = height;
				}
			}

			Debug.Log($"{width},{height}");
			return new int[] { width, height };
		}

		[TestCase(8, 1, ExpectedResult = new int[] { 3, 3 })]
		[TestCase(10, 2, ExpectedResult = new int[] { 4, 3 })]
		[TestCase(24, 24, ExpectedResult = new int[] { 8, 6 })]
		public int[] SiteSolution(int brown, int red) {
			int nSum = brown + red;
			int[] answer = new int[2];
			for (int hgt = 3; hgt < nSum / 3 + 1; hgt++) {
				for (int wid = hgt; wid < nSum / 3 + 1; wid++) {
					if (hgt * wid == nSum) {
						if ((hgt - 2) * 2 + (wid - 2) * 2 + 4 == brown) {
							answer[0] = wid;
							answer[1] = hgt;
							return answer;
						}
					}
				}
			}
			return answer;
		}
	}

	public class 소수찾기
	{
		private static string[] charData;
		private static char[] words;
		HashSet<int> combined = new HashSet<int>();

		[Test]
		[TestCase("17", ExpectedResult = 3)]
		[TestCase("011", ExpectedResult = 2)]
		[TestCase("0", ExpectedResult = 0)]
		public int MySolution(string numbers) {

			var arr = numbers.ToCharArray();
			words = new char[arr.Length];
			charData = new string[arr.Length];
			genWord(arr.Length, 0, getRemainChar(arr, -1));

			foreach(var n in combined) {
				Debug.Log(n);
			}

			return combined.Where(o => IsPrime(o)).Count();
		}

		private void genWord(int n, int depth, char[] remainWords) {

			for (int i = 0; i < n; i++) {
				words[depth] = remainWords[i];
				for (int f = depth + 1; f < words.Length; f++) {
					words[f] = (char)0;
				}

				combined.Add(int.Parse(new string(words)));
				genWord(n - 1, depth + 1, getRemainChar(remainWords, i));
			}
		}

		private char[] getRemainChar(char[] source, int removeIdx) {
			if (removeIdx == -1) {
				return source;
			}

			char[] result = new char[source.Length - 1];
			int index = 0;
			for (int i = 0; i < source.Length; i++) {
				if (i != removeIdx) {
					result[index] = source[i];
					index++;
				}
			}

			return result;

		}

		private bool IsPrime(int number) {
			if (number < 2) {
				return false;
			}
			for (int f = 2; f < number; f++) {
				if (number % f == 0) {
					return false;
				}
			}
			return true;
		}
	}

	public class 소수찾기2
	{
		List<int> makeNumbers = new List<int>();
		private string chs;
		void MakeNumber(char[] board, int num, bool[] mask) {
			for (int i = 0; i < chs.Length; i++) {
				if (mask[i]) continue;
				board[num] = chs[i];
				mask[i] = true;
				makeNumbers.Add(int.Parse(new string(board)));
				if (num >= chs.Length - 1) {
					board[num] = '\0';
					mask[i] = false;
					return;
				}
				MakeNumber(board, num + 1, mask);
				board[num] = '\0';
				mask[i] = false;
			}
		}
		public int solution(string numbers) {
			int answer = 0;
			chs = numbers;
			MakeNumber(new char[chs.Length], 0, new bool[chs.Length]);
			makeNumbers = makeNumbers.Distinct().ToList();
			if (makeNumbers.Contains(0)) makeNumbers.Remove(0);
			if (makeNumbers.Contains(1)) makeNumbers.Remove(1);
			int max = makeNumbers.Max();
			List<int> prime = new List<int>();//[Math.Pow(10,numbers.Length-1)];
			for (int i = 1; i <= max; i++) {
				prime.Add(i); //prime[i-1] = i;
			}
			for (int i = 2; i <= Math.Sqrt(max); i++) {
				for (int j = i * 2; j <= max; j += i) {
					prime[j - 1] = 0;
				}
			}
			foreach (var item in makeNumbers) {
				if (prime.Contains(item)) answer++;
			}
			return answer;
		}
	}
}
