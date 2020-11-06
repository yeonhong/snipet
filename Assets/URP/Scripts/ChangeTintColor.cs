using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTintColor : MonoBehaviour
{
	public Color TintColor = Color.white;

	private void Start() {
		GetComponent<MeshRenderer>().material.SetColor("_TintColor", TintColor);
	}
}
