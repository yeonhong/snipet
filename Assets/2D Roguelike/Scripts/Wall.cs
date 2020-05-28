using NUnit.Framework;
using UnityEngine;

namespace Roguelike2D
{
	public class Wall : MonoBehaviour
	{
		public AudioClip chopSound1 = null;
		public AudioClip chopSound2 = null;
		public Sprite dmgSprite = null;
		public int hp = 3;

		private SpriteRenderer spriteRenderer = null;

		private void Awake() {
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void DamageWall(int loss) {
			SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
			spriteRenderer.sprite = dmgSprite;
			hp -= loss;
			if (hp <= 0) {
				gameObject.SetActive(false);
			}
		}
	}
}
