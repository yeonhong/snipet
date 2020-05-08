using System.Collections.Generic;
using UnityEngine;

namespace TDD_in_Unity
{
	public class HeartContainer
	{
		private readonly List<Heart> _list;

		public HeartContainer(List<Heart> list) {
			_list = list;
		}

		public void Replenish(int numberOfHeartPieces) {
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