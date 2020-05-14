using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

/// <summary>
/// 특정 테스트를 간편히 실행할 수 있도록 메뉴로 추출하기.
/// </summary>

// todo : 프리팹의 정합성 테스트를 해본다. (like 게임씬의 정합성 테스트)
public class RunTestsFromMenu : ScriptableObject, ICallbacks
{
	[MenuItem("Tools/Run useful tests")]
	public static void DoRunTests() {
		CreateInstance<RunTestsFromMenu>().StartTestRun();
	}
	private void StartTestRun() {
		hideFlags = HideFlags.HideAndDontSave;
		CreateInstance<TestRunnerApi>().Execute(new ExecutionSettings {
			filters = new[] { new Filter { categoryNames = new[] { "UsefulTests" } } }
		});
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