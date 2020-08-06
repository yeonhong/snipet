using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HashTableTest
{

	public class 베스트엘범_my
	{
		[Test]
		[TestCase(
			new string[] { "classic", "pop", "classic", "classic", "pop" },
			new int[] { 500, 600, 150, 800, 2500 },
			ExpectedResult = new int[] { 4, 1, 3, 0 })]
		[TestCase(
			new string[] { "classic", "pop", "classic", "classic", "pop", "dance", "dance" },
			new int[] { 5000, 600, 800, 800, 2500, 3, 2 },
			ExpectedResult = new int[] { 0, 2, 4, 1, 5, 6 })]
		public int[] 베스트앨범(string[] genres, int[] plays) {
			List<int> answer = new List<int>();
			const int byGenre = 2;
			Dictionary<string, int> genreDic = new Dictionary<string, int>();
			for (int f = 0; f < genres.Length; f++) {
				var genre = genres[f];
				var play = plays[f];

				if (genreDic.ContainsKey(genre)) {
					genreDic[genre] += play;
				}
				else {
					genreDic.Add(genre, play);
				}
			}

			var genresOrder = from item in genreDic orderby item.Value descending select item.Key;
			foreach (var g in genresOrder) {
				int topPlay = 0;
				int idxTop = -1;

				for (int i = 0; i < byGenre; i++) {
					for (int f = 0; f < genres.Length; f++) {
						if (genres[f] == g) {
							if (plays[f] > topPlay) {
								idxTop = f;
								topPlay = plays[f];
							}
						}
					}

					if (idxTop != -1) {
						answer.Add(idxTop);
						plays[idxTop] = 0;
						topPlay = 0;
						idxTop = -1;
					}
				}
			}

			return answer.ToArray();
		}
	}

	public class 베스트엘범_solution // 베스트엘범
	{
		[Test]
		[TestCase(
		new string[] { "classic", "pop", "classic", "classic", "pop" },
		new int[] { 500, 600, 150, 800, 2500 },
		ExpectedResult = new int[] { 4, 1, 3, 0 })]
		[TestCase(
		new string[] { "classic", "pop", "classic", "classic", "pop", "dance", "dance" },
		new int[] { 5000, 600, 800, 800, 2500, 3, 2 },
		ExpectedResult = new int[] { 0, 2, 4, 1, 5, 6 })]
		public int[] solution(string[] genres, int[] plays) {
			List<int> lstAnswer = new List<int>();

			List<MusicInfo> lst = new List<MusicInfo>();

			for (int i = 0; i < genres.Count(); i++) {
				lst.Add(new MusicInfo(i, genres[i], plays[i]));
			}

			var query = (from m in lst
						 group m by m.Genre into g
						 select new {
							 genre = g.First().Genre,
							 playSum = g.Sum(x => x.Plays)
						 }).OrderByDescending(x => x.playSum);


			foreach (var hitGenre in query) {
				// 장르별 두건 추출
				var query2 = (from m in lst
							  where m.Genre == hitGenre.genre
							  orderby m.Plays descending
							  select m).Take(2);
				foreach (var pick in query2) {
					lstAnswer.Add(pick.Index);
				}
			}

			return lstAnswer.ToArray();
		}

		public class MusicInfo
		{
			public int Index { set; get; }
			public string Genre { set; get; }
			public int Plays { set; get; }

			public MusicInfo(int p_Index, string p_Genre, int p_Plays) {
				this.Index = p_Index;
				this.Genre = p_Genre;
				this.Plays = p_Plays;
			}
		}
	}
}
