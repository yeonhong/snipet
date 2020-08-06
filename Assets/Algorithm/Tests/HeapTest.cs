using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HeapTest
{
	public class 디스크컨트롤러
	{
		[Test]
		[TestCase(0,  ExpectedResult = 9)]
		public int solution(int v) {
			var jobs = new int[,] { { 0, 3 }, { 1, 9 }, { 2, 6 } };

			List<Job> joblist = new List<Job>();
			for(int f=0; f<jobs.GetLength(0); f++) {
				joblist.Add(new Job(jobs[f,0], jobs[f,1]));
			}

			int time = 0;
			int workTime = 0;
			while(joblist.Count > 0) {

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

		class Job
		{
			public int requestTime;
			public int duration;

			public Job(int rt, int dt) {
				requestTime = rt;
				duration = dt;
			}
		}
	}
}
