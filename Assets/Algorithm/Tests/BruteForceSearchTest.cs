using NUnit.Framework;
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
}
