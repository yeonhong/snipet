using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.TestTools;
using UnityEngine;
using UnityEngine.TestTools;

[assembly: TestPlayerBuildModifier(typeof(SetUpPlaymodeTestPlayer))]
/// <summary>
/// 테스트모드 빌드에 대한 설정 (in test runner / playmode window)
/// </summary>
public class SetUpPlaymodeTestPlayer : ITestPlayerBuildModifier, IPostBuildCleanup
{
	private const string TestBuildPath = "TestRunnerBuild";
	private static bool s_RunningPlayerTests;
	private const string TestScenePath = "Assets/Tests/Scenes/TestScene.unity";

	public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions) {

		//playerOptions.options &= ~(BuildOptions.AutoRunPlayer | BuildOptions.ConnectToHost);
		playerOptions.options &= ~(BuildOptions.ConnectToHost);

		playerOptions = IncludeTestSceneInBuild(playerOptions);
		playerOptions = SetTestBuildLocation(playerOptions);

		s_RunningPlayerTests = true;
		return playerOptions;
	}

	private static BuildPlayerOptions SetTestBuildLocation(BuildPlayerOptions playerOptions) {
		var buildLocaion = Path.GetFullPath(TestBuildPath);
		var fileName = Path.GetFileName(playerOptions.locationPathName);

		if (!string.IsNullOrEmpty(fileName)) {
			buildLocaion = Path.Combine(buildLocaion, fileName);
		}

		playerOptions.locationPathName = buildLocaion;
		return playerOptions;
	}

	private static BuildPlayerOptions IncludeTestSceneInBuild(BuildPlayerOptions playerOptions) {
		if (!playerOptions.scenes.Contains(TestScenePath)) {
			playerOptions.scenes = playerOptions.scenes.Append(TestScenePath).ToArray();
		}

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
