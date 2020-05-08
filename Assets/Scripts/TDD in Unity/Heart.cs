using System;
using UnityEngine.UI;

namespace TDD_in_Unity
{
	public class Heart
	{
		private const float FillPerHeartPiece = 0.25f;
		private Image _image;

		public Heart(Image image) {
			_image = image;
		}

		public void Deplate(int numberOfHeartPieces) {
			if (numberOfHeartPieces < 0) {
				throw new ArgumentOutOfRangeException(nameof(numberOfHeartPieces));
			}

			_image.fillAmount -= numberOfHeartPieces * FillPerHeartPiece;
		}

		public void Replenish(int numberOfHeartPieces) {
			if (numberOfHeartPieces < 0) {
				throw new ArgumentOutOfRangeException(nameof(numberOfHeartPieces));
			}

			_image.fillAmount += numberOfHeartPieces * FillPerHeartPiece;
		}
	}
}