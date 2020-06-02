using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike2D
{
	public class EnemyManager
	{
		public List<Enemy> Enemies { get; private set; }

		private readonly float _turnDelay;

		public bool IsEnemyMoving { get; private set; }

		public event EventHandler OnEndEnemyTurn;

		public EnemyManager(float turnDelay) {
			Enemies = new List<Enemy>();
			_turnDelay = turnDelay;

			Init();
		}

		public void Init() {
			IsEnemyMoving = false;
			Enemies.Clear();
		}

		public void AddEnemy(Enemy enemy) => Enemies.Add(enemy);

		public IEnumerator MoveEnemies(Transform target) {
			yield return new WaitForSeconds(_turnDelay);

			for (int i = 0; i < Enemies.Count; i++) {
				Enemies[i].MoveEnemy(target.position);
				yield return new WaitForSeconds(Enemies[i].moveTime);
			}

			yield return new WaitForSeconds(_turnDelay);

			OnEndEnemyTurn?.Invoke(this, null);
		}
	}
}