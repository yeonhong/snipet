using NUnit.Framework;
using UnityEngine;

namespace Roguelike2D
{
	public class Wall : MonoBehaviour
	{
		[SerializeField] private AudioClip chopSound1 = null;
		[SerializeField] private AudioClip chopSound2 = null;
		[SerializeField] private Sprite dmgSprite = null;
		[SerializeField] private int _HP = 3;

		private SpriteRenderer _spriteRenderer = null;

		private void Awake() {
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void DamageWall(int loss) {
			SoundManager.Instance.RandomizeSfx(chopSound1, chopSound2);

			_spriteRenderer.sprite = dmgSprite;

			LosingHP(loss);
		}

		private void LosingHP(int amount) {
			_HP -= amount;
			if (_HP <= 0) {
				gameObject.SetActive(false);
			}
		}

#if UNITY_EDITOR
		public bool ValidateResources() {
			return chopSound1 != null && chopSound2 != null && dmgSprite != null && _HP > 0;
		}
#endif
	}
}
