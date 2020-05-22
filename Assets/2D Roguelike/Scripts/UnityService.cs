using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnityService
{
	float GetAxisRaw(string v);
	float GetDeltaTime();
}

public class UnityService : IUnityService
{
	public float GetAxisRaw(string arg) {
		return Input.GetAxisRaw(arg);
	}

	public float GetDeltaTime() {
		return Time.deltaTime;
	}
}
