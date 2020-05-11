using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Linq;

namespace TDD_in_Unity
{
	public class HeartContainer
	{
		private readonly List<Heart> _list;

		public HeartContainer(List<Heart> list) {
			_list = list;
		}

		public void Replenish(int heartPieces) {

			if(heartPieces < 0) {
				throw new ArgumentOutOfRangeException(nameof(heartPieces));
			}

			foreach (var heart in _list) {
				if (heartPieces > 0) {
					var toReplenish = Mathf.Min(heart.EmptyHeartPieces, heartPieces);
					heartPieces -= toReplenish;
					heart.Replenish(toReplenish);
				}
				else
					break;
			}
		}

		public void Deplete(int heartPieces) {

			if (heartPieces < 0) {
				throw new ArgumentOutOfRangeException(nameof(heartPieces));
			}

			foreach (var heart in _list.AsEnumerable().Reverse()) {
				if (heartPieces > 0) {
					var toDeplate = Mathf.Min(heart.FilledHeartPieces, heartPieces);
					heartPieces -= toDeplate;
					heart.Deplate(toDeplate);
				}
				else
					break;
			}
		}
	}
}