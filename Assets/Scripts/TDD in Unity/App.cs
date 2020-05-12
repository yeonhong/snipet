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

		private Player _player = null;

		private HeartContainer _heartContainer = null;

		private void Start() {

			_player = new Player(16, 16);

			_heartContainer = new HeartContainer(
				_images.Select(image => new Heart(image)).ToList());

			_player.Healed += (sender, args) => { _heartContainer.Replenish(args.Amount); };
			_player.Damaged += (sender, args) => { _heartContainer.Deplete(args.Amount); };
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				_player.Heal(_count);
			}

			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				_player.Damage(_count);
			}
		}
	}
}
