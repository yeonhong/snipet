using UnityEngine;

// todo : enemy 리펙토링 하기
namespace Roguelike2D
{
	public class Enemy : MovingObject
	{
		public int playerDamage;
		public AudioClip attackSound1;
		public AudioClip attackSound2;

		private Animator animator;
		private Transform target;
		private bool skipMove;


		protected override void Start() {
			GameManager.instance.AddEnemyToList(this);
			animator = GetComponent<Animator>();
			target = GameObject.FindGameObjectWithTag("Player").transform;
			base.Start();
		}

		protected override bool AttemptMove<T>(int xDir, int yDir) {
			if (skipMove) {
				skipMove = false;
				return false;

			}

			var isMoved = base.AttemptMove<T>(xDir, yDir);
			skipMove = true;
			return isMoved;
		}

		public void MoveEnemy() {
			int xDir = 0;
			int yDir = 0;

			if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
				yDir = target.position.y > transform.position.y ? 1 : -1;
			}
			else {
				xDir = target.position.x > transform.position.x ? 1 : -1;
			}

			AttemptMove<Player>(xDir, yDir);
		}

		protected override void OnBumped<T>(T component) {
			var hitPlayer = component as Player;
			hitPlayer.OnDamage(playerDamage);
			animator.SetTrigger("enemyAttack");
			SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
		}
	}
}
