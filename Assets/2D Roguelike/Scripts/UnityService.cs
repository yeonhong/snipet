using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnityService
{
	float GetAxisRaw(string v);
	float GetDeltaTime();
	Vector3 GetMousePosition();
	bool IsMouseButtonDown();
	bool IsMouseButtonUp();
}

public class UnityService : IUnityService
{
	public float GetAxisRaw(string arg) {
		return Input.GetAxisRaw(arg);
	}

	public float GetDeltaTime() {
		return Time.deltaTime;
	}

	public bool IsMouseButtonDown() {
		return Input.GetMouseButtonDown(0);
	}

	public bool IsMouseButtonUp() {
		return Input.GetMouseButtonUp(0);
	}

	public Vector3 GetMousePosition() {
		return Input.mousePosition;
	}
}
