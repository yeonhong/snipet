﻿using Newtonsoft.Json;
using UnityEngine;

public class JSONTest : MonoBehaviour
{
	private class Enemy
	{
		public string Name { get; set; }
		public int AttackDamage { get; set; }
		public int MaxHealth { get; set; }
	}

	private void Start() {
		string json = @"{
            'Name': 'Ninja',
            'AttackDamage': '40'
            }";

		var enemy = JsonConvert.DeserializeObject<Enemy>(json);

		Debug.Log($"{enemy.Name} deals {enemy.AttackDamage} damage.");
		// Output:
		// Ninja deals 40 damage.
	}
}