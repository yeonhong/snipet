using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace StackQueueTest
{
	public class 프린터
	{
		[Test]
		[TestCase(new int[] { 2, 1, 3, 2 }, 2, ExpectedResult = 1)]
		[TestCase(new int[] { 1, 1, 9, 1, 1, 1 }, 0, ExpectedResult = 5)]

		public int solution(int[] priorities, int location) {
			int answer = 0;

			Queue<PrintPage> printQ = new Queue<PrintPage>();
			for(int f=0; f<priorities.Length; f++) {
				printQ.Enqueue(new PrintPage(f, priorities[f]));
			}

			int printing = 0;
			while (printQ.Count > 0) {

				Debug.Log(printQ.Max(x => x.priority));

				var headQ = printQ.Dequeue();
				if (printQ.Where(o => headQ.priority < o.priority).Count() > 0) {
					printQ.Enqueue(headQ);
				} else {
					printing++;
					if (headQ.index == location)
						return printing;
				}
			}

			return answer;
		}

		class PrintPage
		{
			public int index;
			public int priority;

			public PrintPage(int index, int priority) {
				this.index = index;
				this.priority = priority;
			}
		}
	}

	public class 프린터_Solution
	{
		[Test]
		[TestCase(new int[] { 2, 1, 3, 2 }, 2, ExpectedResult = 1)]
		[TestCase(new int[] { 1, 1, 9, 1, 1, 1 }, 0, ExpectedResult = 5)]

		public int solution(int[] priorities, int location) {
			int answer = 0;
			Queue<KeyValuePair<int, int>> que = new Queue<KeyValuePair<int, int>>();
			for (int i = 0; i < priorities.Length; i++) {
				que.Enqueue(new KeyValuePair<int, int>(i, priorities[i]));
			}
			while (true) {
				int nMax = que.Max(x => x.Value);
				var kv = que.Dequeue();
				if (kv.Value == nMax) {
					if (kv.Key == location) return answer + 1;
					else {
						answer++;
						continue;
					}
				}
				que.Enqueue(kv);
			}
		}
	}
}
