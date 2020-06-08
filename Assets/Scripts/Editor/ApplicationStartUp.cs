using UnityEditor;
using UnityEngine;

namespace Project.Editor
{
	[InitializeOnLoad]
	public class ApplicationStartUp : ScriptableObject
	{
		static ApplicationStartUp _instance = null;

		static ApplicationStartUp() {
			EditorApplication.update += OnInit;
		}

		static void OnInit() {
			EditorApplication.update -= OnInit;

			_instance = FindObjectOfType<ApplicationStartUp>();
			if (_instance == null) {
				_instance = CreateInstance<ApplicationStartUp>();

				OnApplicationLoaded();
			}
		}

		static void OnApplicationLoaded() {
			Debug.Log("OnApplicationLoaded");
		}
	}
}