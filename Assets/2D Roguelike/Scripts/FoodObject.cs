using System;
using UnityEngine;

namespace Roguelike2D
{
	public class FoodObjectModel
	{
		public int Points { get; private set; }

		public FoodObjectModel(int point) {
			Points = point;
		}
	}

	public class FoodObject : MonoBehaviour
	{
		[SerializeField] private int _points = 1;
		public int Points { get { return _points; } }

		[SerializeField] private AudioClip[] _fxSounds = null;

		private FoodObjectModel model = null;
		private ISoundManager _soundManager = null;

		private void Awake() {
			if(model == null) {
				model = new FoodObjectModel(_points);
			}
		}
		
		public void Consume() {
			if (_soundManager == null) {
				_soundManager = SoundManager.Instance;
			}
			_soundManager.RandomizeSfx(_fxSounds);
			gameObject.SetActive(false);
		}
	}
}