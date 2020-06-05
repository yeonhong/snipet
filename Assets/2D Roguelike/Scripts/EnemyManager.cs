﻿using System;
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

		public void Add(Enemy enemy) => Enemies.Add(enemy);

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