using System.IO;
using UnityEditor;
using UnityEditor.TestTools;

// 빌드에서 test runner를 실행하고 결과를 얻는다. (playmode test)
[assembly: TestPlayerBuildModifier(typeof(SetUpPlaymodeTestPlayer))]
public class SetUpPlaymodeTestPlayer : ITestPlayerBuildModifier
{
	private const string TestBuildPath = "TestRunnerBuild";

	public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions) {
		playerOptions.options &= ~(BuildOptions.AutoRunPlayer | BuildOptions.ConnectToHost);

		var buildLocaion = Path.GetFullPath(TestBuildPath);
		var fileName = Path.GetFileName(playerOptions.locationPathName);

		if (!string.IsNullOrEmpty(fileName)) {
			buildLocaion = Path.Combine(buildLocaion, fileName);
		}

		playerOptions.locationPathName = buildLocaion;

		return playerOptions;
	}
}
