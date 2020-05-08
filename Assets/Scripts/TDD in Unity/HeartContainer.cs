using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace TDD_in_Unity
{
	public class HeartContainer
	{
		private readonly List<Heart> _list;

		public HeartContainer(List<Heart> list) {
			_list = list;
		}

		public void Replenish(int numberOfHeartPieces) {

			if(numberOfHeartPieces < 0) {
				throw new ArgumentOutOfRangeException(nameof(numberOfHeartPieces));
			}

			foreach (var heart in _list) {
				if (numberOfHeartPieces > 0) {
					var chargeCount = (Heart.HeartPiecesOfHeart - heart.CurrentNumberOfHeartPieces);
					chargeCount = Mathf.Min(chargeCount, numberOfHeartPieces);
					heart.Replenish(chargeCount);
					numberOfHeartPieces -= chargeCount;
				}
			}
		}
	}
}