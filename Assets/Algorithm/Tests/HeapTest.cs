using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace HeapTest
{
	public class 디스크컨트롤러
	{
		[Test]
		[TestCase(0, ExpectedResult = 9)]
		public int solution(int v) {
			var jobs = new int[,] { { 0, 3 }, { 1, 9 }, { 2, 6 } };

			List<Job> joblist = new List<Job>();
			for (int f = 0; f < jobs.GetLength(0); f++) {
				joblist.Add(new Job(jobs[f, 0], jobs[f, 1]));
			}

			int time = 0;
			int workTime = 0;
			while (joblist.Count > 0) {

				var job = joblist.
					Where(o => o.requestTime <= time).
					OrderBy(o => o.duration).FirstOrDefault();

				if (job != null) {
					time += job.duration;
					workTime += (time - job.requestTime);
					joblist.Remove(job);
				}
				else {
					time++;
				}
			}

			return workTime / jobs.GetLength(0);
		}

		private class Job
		{
			public int requestTime;
			public int duration;

			public Job(int rt, int dt) {
				requestTime = rt;
				duration = dt;
			}
		}
	}

	public class 이중우선큐
	{
		[Test]
		[TestCase(1, new string[] { "I 16", "D 1" }, ExpectedResult = new int[] { 0, 0 })]
		[TestCase(2, new string[] { "I 7", "I 5", "I -5", "D -1" }, ExpectedResult = new int[] { 7, 5 })]
		public int[] solution(int n, string[] operations) {
			List<int> data = new List<int>();

			for (int f = 0; f < operations.Length; f++) {
				var op = operations[f];

				var words = op.Split(' ');
				var value = int.Parse(words[1]);
				if (words[0] == "I") {
					data.Add(value);
				}
				else if (words[0] == "D" && data.Count > 0) {
					if (value == -1) {
						// delete min
						data.Remove(data.Min());
					}
					else if (value == 1) {
						// delete max
						data.Remove(data.Max());
					}
				}
			}

			if (data.Count > 0) {
				return new int[] { data.Max(), data.Min() };
			}
			else {
				return new int[] { 0, 0 };
			}
		}
	}

	public class 이중우선큐_솔루션
	{
		[Test]
		[TestCase(1, new string[] { "I 16", "D 1" }, ExpectedResult = new int[] { 0, 0 })]
		[TestCase(2, new string[] { "I 7", "I 5", "I -5", "D -1" }, ExpectedResult = new int[] { 7, 5 })]
		public int[] solution(int n, string[] operations) {
			List<int> lst = new List<int>();
			foreach (string cmd in operations) {
				string[] str = cmd.Split(' ');
				if (str[0].Equals("I")) lst.Add(int.Parse(str[1]));
				else if (lst.Count() > 0 && str[1] == "1") lst.Remove(lst.Max());
				else if (lst.Count() > 0 && str[1] == "-1") lst.Remove(lst.Min());
			}
			return lst.Count() == 0 ? new int[] { 0, 0 } : new int[] { lst.Max(), lst.Min() };
		}
	}
}
