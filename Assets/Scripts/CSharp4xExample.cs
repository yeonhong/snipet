using System;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Mathf;

// 아래 링크의 4.x 문법에 대해 테스트 한다.
// https://docs.microsoft.com/ko-kr/visualstudio/cross-platform/unity-scripting-upgrade?view=vs-2019

public class CSharp4xExample : MonoBehaviour
{
	// .NET 4.x // auto 속성 이니셜라이저.
	public int Health { get; set; } = 100;

	// .NET 4.x
	public string PlayerHealthUiText => $"Player health: {Health}";
	// public string PlayerHealthUiText { get { return $"Player health: {Health}"; } }

	#region TAP(작업 기반 비동기 패턴)
	private async void Start() {

		Debug.Log("Start");
		await WaitSecondAsync(1);
		Debug.Log("Start End");

		/*
		 * Task.Run 및 Task.ConfigureAwait(false)와 같은 메서드를 사용하여 백그라운드 스레드에서 작업을 실행할 수 있습니다. 이 기술은 성능 향상을 위해 주 스레드에서 비용이 많이 드는 작업을 오프로딩하는 데 유용합니다. 그러나 백그라운드 스레드를 사용하면 경합 조건과 같이 디버그하기 어려운 문제가 발생할 수 있습니다.
		*/

		NameOfTest();

		// .NET 4.x
		Max(10, 20); // using static UnityEngine.Mathf;
	}

	private async Task WaitSecondAsync(double sec = 1) {
		Debug.Log(":: Wait Start");
		await Task.Delay(TimeSpan.FromSeconds(sec));
		Debug.Log(":: Wait End");
	}
	#endregion
	
	#region NameOf
	enum MyEnum
	{
		One,
		Two,
		Three
	};

	private void NameOfTest() {
		Debug.Log(nameof(MyEnum.One));
	} 
	#endregion

	private void TokenInterpolated() {
		// .NET 4.x // 문자열 보간. $를 붙여서 바로 넣을 수 있도록.
		Debug.Log($"Player health: {Health}");
	}

	// .NET 4.x // 람다식으로 본문을 간결하게 작성.
	private int TakeDamage(int amount) => Health -= amount;
	/*
	private int TakeDamage(int amount) {
		return Health -= amount;
	}
	*/
}
