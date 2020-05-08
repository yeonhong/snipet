using UnityEngine;
using UnityEngine.UI;

namespace TDD_in_Unity
{
	public class App : MonoBehaviour
	{
		[SerializeField] private Image _image = null;
		private Heart _heart = null;

		private void Start() {
			_heart = new Heart(_image);
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				_heart.Replenish(1);
			}

			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				_heart.Deplate(1);
			}
		}
	}
}
