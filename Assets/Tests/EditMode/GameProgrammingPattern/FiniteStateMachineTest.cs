using NUnit.Framework;
using ProgrammingPattern;
using UnityEngine;

namespace Tests.ProgrammingPattern
{
	public class ExampleStateManager : FiniteStateMachine<ExampleStateManager>
	{
		public enum eState : int
		{
			None = 0,
			Test1 = 1,
		}

		public eState State { get { return (eState)Current.GetID(); } }

		protected override void Initialize() {
			AddState(new ExampleState(eState.None));
			AddState(new ExampleState(eState.Test1));
		}

		public void ChangeState(eState nextID)
		{
			base.ChangeState((int)nextID);
		}

		public class ExampleState : FiniteState
		{
			public ExampleState(eState id) : base((int)id) {	}

			public override void OnEnter()
			{
				// Debug.Log("<< endter " + ID);
			}

			public override void OnLeave()
			{
				// Debug.Log(">> leave " + ID);
			}
		}
	}

	public class FiniteStateMachineTest
	{
		ExampleStateManager stateManager = null;

		[SetUp]
		public void Setup()
		{
			stateManager = new ExampleStateManager();
		}

		[Test]
		public void ChangeStateTest()
		{
			stateManager.ChangeState(ExampleStateManager.eState.Test1);
			Assert.IsTrue(stateManager.State == ExampleStateManager.eState.Test1);
		}


		//// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
		//// `yield return null;` to skip a frame.
		//[UnityTest]
		//public IEnumerator FSMTestsWithEnumeratorPasses()
		//{
		//    // Use the Assert class to test conditions.
		//    // Use yield to skip a frame.
		//    yield return null;
		//}
	}
}
