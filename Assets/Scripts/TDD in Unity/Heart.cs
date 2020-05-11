using System;
using UnityEngine.UI;

namespace TDD_in_Unity
{
	public class Heart
	{
		private const int HeartPiecesOfHeart = 4;
		private const float FillPerHeartPiece = 1f / HeartPiecesOfHeart;

		private Image _image;

		public Heart(Image image) {
			_image = image;
		}

		public int FilledHeartPieces => CalculateHeartPieces();
		public int EmptyHeartPieces => HeartPiecesOfHeart - CalculateHeartPieces();

		private int CalculateHeartPieces() {
			return (int)(_image.fillAmount * HeartPiecesOfHeart);
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