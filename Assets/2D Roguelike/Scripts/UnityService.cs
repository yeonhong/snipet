using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnityService
{
	float GetAxisRaw(string v);
}

public class UnityService : IUnityService
{
	public float GetAxisRaw(string arg) {
		return Input.GetAxisRaw(arg);
	}
}
