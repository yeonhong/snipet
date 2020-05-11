using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TDD_in_Unity
{
	public class App : MonoBehaviour
	{
		[SerializeField] private List<Image> _images = null;
		[SerializeField] private int _count = 1;

		private HeartContainer _heartContainer = null;

		private void Start() {
			_heartContainer = new HeartContainer(
				_images.Select(image => new Heart(image)).ToList());
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				_heartContainer.Replenish(_count);
			}

			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				_heartContainer.Deplete(_count);
			}
		}
	}
}
