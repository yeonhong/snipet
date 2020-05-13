using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.TestTools.TestRunner.Api;
using NUnit.Framework;
using UnityEngine.TestTools;

public class ResultCollector : ICallbacks
{
	public ITestResultAdaptor Result { get; private set; }

	public void RunFinished(ITestResultAdaptor result) {
		Result = result;
	}

	public void RunStarted(ITestAdaptor testsToRun) { }
	public void TestFinished(ITestResultAdaptor result) { }
	public void TestStarted(ITestAdaptor test) { }
}

public class RunTestsBeforeBuild : IPreprocessBuildWithReport
{
	public int callbackOrder => 0;

	public void OnPreprocessBuild(BuildReport report) {
		Debug.Log("run prebuild editmode tests");

		var result = new ResultCollector();

		var api = ScriptableObject.CreateInstance<TestRunnerApi>();
		api.RegisterCallbacks(result);

		api.Execute(new ExecutionSettings {
			runSynchronously = true,
			filters = new[]{ new Filter{
				testMode = TestMode.EditMode
			}}
		});

		if (result.Result.FailCount > 0)
			throw new BuildFailedException($"{result.Result.FailCount} tests failed");

		Debug.Log($"tests passed: {result.Result.PassCount}");
	}
}