using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike2D
{
	public class EnemyManager
	{
		public IReadOnlyList<Enemy> Enemies { get { return _enemies; } }
		public bool IsEnemyMoving { get; private set; }
		public event EventHandler OnEndEnemyTurn;

		private readonly float _turnDelay;
		private List<Enemy> _enemies;

		public EnemyManager(float turnDelay) {
			_enemies = new List<Enemy>();
			_turnDelay = turnDelay;

			Initialize();
		}

		public void Initialize() {
			IsEnemyMoving = false;
			_enemies.Clear();
		}

		public void Add(Enemy enemy) => _enemies.Add(enemy);

		public IEnumerator MoveEnemies(Transform target) {
			yield return new WaitForSeconds(_turnDelay);

			for (int i = 0; i < Enemies.Count; i++) {
				Enemy enemy = Enemies[i];
				enemy.Move(target.position);

				while (enemy.IsMoving) {
					yield return null;
				}
			}

			yield return new WaitForSeconds(_turnDelay);

			OnEndEnemyTurn?.Invoke(this, null);
		}
	}
}