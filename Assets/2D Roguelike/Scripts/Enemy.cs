using UnityEngine;

namespace Roguelike2D
{
	public class Enemy : MovingObject
	{
		private const string ATTACK_ANIM = "enemyAttack";
		[SerializeField] private int _attackPoint = 1;
		[SerializeField] private AudioClip[] _attackSounds = null;

		private Animator _animator = null;
		private bool _skipMove = false;

		protected override void Start() {
			base.Start();
			_animator = GetComponent<Animator>();
		}

		public void Move(Vector3 target) {
			if (IsSkipMove()) {
				return;
			}

			GetMoveDirection(target, out int horizontal, out int vertical);
			AttemptMove<Player>(horizontal, vertical);
		}

		private void GetMoveDirection(Vector3 target, out int horizontal, out int vertical) {
			horizontal = GetDirection(target.x, transform.position.x);
			vertical = GetDirection(target.y, transform.position.y);
			if (horizontal != 0) {
				vertical = 0;
			}
		}

		private bool IsSkipMove() {
			_skipMove = !_skipMove;
			return !_skipMove;
		}

		private int GetDirection(float dest, float curr) {
			return dest > curr ? 1 : -1;
		}

		protected override void OnBumped<T>(T bumpedObject) {
			var player = bumpedObject as Player;
			player.OnDamage(_attackPoint);

			_animator.SetTrigger(ATTACK_ANIM);

			PlayAttackSound();
		}

		private void PlayAttackSound() {
			if (_attackSounds != null) {
				SoundManager.instance.RandomizeSfx(_attackSounds);
			}
		}
	}
}
