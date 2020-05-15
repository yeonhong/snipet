using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class RunSceneValidationTestsFromContextMenu : ScriptableObject, ICallbacks
{
	static RunSceneValidationTestsFromContextMenu() {
		SceneHierarchyHooks.addItemsToSceneHeaderContextMenu += (menu, scene) => {
			menu.AddItem(new GUIContent("Run Validate Tests"), false, DoRunTests, scene);
		};
	}

	private static void DoRunTests(object userData) {
		CreateInstance<RunSceneValidationTestsFromContextMenu>().StartTestRun((Scene)userData);
	}

	private static TestRunnerApi _runnerApi;
	private static TestRunnerApi RunnerApi => _runnerApi ? _runnerApi : _runnerApi = CreateInstance<TestRunnerApi>();

	private void StartTestRun(Scene scene) {
		hideFlags = HideFlags.HideAndDontSave;

		RunnerApi.Execute(new ExecutionSettings {
			filters = new[] {
				new Filter {
					groupNames = new [] {$"SceneValidationTests\\(\"{scene.path}\"\\)\\."},
					//categoryNames = new [] {"Validation"},
					testMode = TestMode.EditMode
				}
			}
		});
	}

	public void OnEnable() {
		RunnerApi.RegisterCallbacks(this);
	}

	public void OnDisable() {
		RunnerApi.UnregisterCallbacks(this);
	}

	public void RunFinished(ITestResultAdaptor result) {
		if (!result.HasChildren) {
			EditorUtility.DisplayDialog("알림", "해당씬은 등록된 테스트Case가 없습니다.", "OK");
		}
		else if (result.FailCount == 0) {
			EditorUtility.DisplayDialog("알림", "테스트가 모두 성공했습니다.", "OK");
		}
		else {
			IEnumerable<string> GetFailedTestNames(ITestResultAdaptor test) {
				if (test.HasChildren) {
					return test.Children.SelectMany(GetFailedTestNames);
				}

				return test.TestStatus == TestStatus.Failed ? new[] { test.Name } : Array.Empty<string>();
			}

			string failedTestNames = string.Join("\n", GetFailedTestNames(result));
			EditorUtility.DisplayDialog("알림", 
				$"실패하였습니다. ({result.FailCount} 개)\n" +
				$"{failedTestNames}", 
				"OK");
			EditorApplication.ExecuteMenuItem("Window/General/Test Runner");
		}

		DestroyImmediate(this);
	}

	public void RunStarted(ITestAdaptor testsToRun) {
	}

	public void TestFinished(ITestResultAdaptor result) {
	}

	public void TestStarted(ITestAdaptor test) {
	}
}
