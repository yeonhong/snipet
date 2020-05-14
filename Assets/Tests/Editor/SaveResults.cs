using NUnit.Framework.Interfaces;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.TestRunner;

[assembly: TestRunCallback(typeof(ResultSerializer))]
/// <summary>
/// 마지막 테스트 결과를 파일에 저장.
/// </summary>
public class ResultSerializer : ITestRunCallback
{
	private const string TestResultFileName = "testresults.xml";
	public void RunStarted(ITest testsToRun) { }
	public void TestStarted(ITest test) { }
	public void TestFinished(ITestResult result) { }
	public void RunFinished(ITestResult testResults) {
		var path = Path.Combine(Application.persistentDataPath, TestResultFileName);
		using (var xw = XmlWriter.Create(path, new XmlWriterSettings { Indent = true })) {
			testResults.ToXml(true).WriteTo(xw);
		}

		System.Console.WriteLine($"***\n\nTEST RESULTS WRITTEN TO\n\n\t{path}\n\n***");
		Application.Quit(testResults.FailCount > 0 ? 1 : 0);
	}
}