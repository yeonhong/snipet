using UnityEngine;

namespace Roguelike2D
{
	public class Loader : MonoBehaviour
	{
		public GameObject gameManager;
		public GameObject soundManager;

		private void Awake() {
			if (GameManager.instance == null) {
				Instantiate(gameManager);
			}

			if (SoundManager.Instance == null) {
				Instantiate(soundManager);
			}
		}
	}
}