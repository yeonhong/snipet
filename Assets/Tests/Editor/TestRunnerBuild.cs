using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.TestTools;
using UnityEngine.TestTools;

// 빌드에서 test runner를 실행하고 결과를 얻는다. (playmode test)
[assembly: TestPlayerBuildModifier(typeof(SetUpPlaymodeTestPlayer))]
public class SetUpPlaymodeTestPlayer : ITestPlayerBuildModifier, IPostBuildCleanup
{
	private const string TestBuildPath = "TestRunnerBuild";
	private static bool s_RunningPlayerTests;

	public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions) {
		//playerOptions.options &= ~(BuildOptions.AutoRunPlayer | BuildOptions.ConnectToHost);
		playerOptions.options &= ~(BuildOptions.ConnectToHost);

		var buildLocaion = Path.GetFullPath(TestBuildPath);
		var fileName = Path.GetFileName(playerOptions.locationPathName);

		if (!string.IsNullOrEmpty(fileName)) {
			buildLocaion = Path.Combine(buildLocaion, fileName);
		}

		playerOptions.locationPathName = buildLocaion;

		s_RunningPlayerTests = true;
		return playerOptions;
	}

	public void Cleanup() {
		if (s_RunningPlayerTests && IsRunningTestsFromCommandLine()) {
			// Exit the Editor on the next update, allowing for other PostBuildCleanup steps to run.
			EditorApplication.update += () => { EditorApplication.Exit(0); };
		}
	}

	private static bool IsRunningTestsFromCommandLine() {
		var commandLineArgs = Environment.GetCommandLineArgs();
		return commandLineArgs.Any(value => value == "-runTests");
	}
}
