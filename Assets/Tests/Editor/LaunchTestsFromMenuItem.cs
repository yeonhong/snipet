﻿using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

/// <summary>
/// 특정 테스트를 간편히 실행할 수 있도록 메뉴로 추출하기.
/// </summary>
public class RunTestsFromMenu : ScriptableObject, ICallbacks
{
	[MenuItem("Tools/Prefab Validation Tests")]
	public static void DoRunTests() {
		CreateInstance<RunTestsFromMenu>().StartTestRun();
	}
	private void StartTestRun() {
		hideFlags = HideFlags.HideAndDontSave;
		
		// ExecutionSetting.
		// https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/reference-filter.html
		var filter = new Filter() {
			testMode = TestMode.EditMode,
			categoryNames = new[] { "Validation" }
			//testMode = TestMode.EditMode
			//https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/extension-run-tests.html
		};

		CreateInstance<TestRunnerApi>().Execute(new ExecutionSettings(filter));
	}
	public void OnEnable() { CreateInstance<TestRunnerApi>().RegisterCallbacks(this); }
	public void OnDisable() { CreateInstance<TestRunnerApi>().UnregisterCallbacks(this); }

	public void RunStarted(ITestAdaptor testsToRun) {
	}

	public void TestStarted(ITestAdaptor test) {
	}

	public void TestFinished(ITestResultAdaptor result) {
	}

	public void RunFinished(ITestResultAdaptor result) {
		DestroyImmediate(this);
	}
}