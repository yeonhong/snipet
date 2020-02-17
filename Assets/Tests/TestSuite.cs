using NUnit.Framework;
using ProgrammingPattern;
using Tests.ProgrammingPattern;
using UnityEngine;

namespace Tests.ProgrammingPattern
{
	public class ExampleStateManager : FiniteStateMachine<ExampleStateManager>
	{
		public enum State : int
		{
			None = 0,
			Test1 = 1,
		}

		public override void Initialize()
		{
			AddState(new ExampleState((int)State.None));
			AddState(new ExampleState((int)State.Test1));
		}

		public void ChangeState(State nextID)
		{
			base.ChangeState((int)nextID);
		}
	}

	public class ExampleState : FiniteState
	{
		public ExampleState(int id) : base(id)
		{
		}

		public override void OnEnter()
		{
			Debug.Log("<< endter " + ID);
		}

		public override void OnLeave()
		{
			Debug.Log(">> leave " + ID);
		}
	}

	public class TestSuite
	{
		ExampleStateManager stateManager = null;

		[SetUp]
		public void Setup()
		{
			stateManager = new ExampleStateManager();
		}

		// A Test behaves as an ordinary method
		[Test]
		public void ChangeStateTest()
		{
			stateManager.ChangeState(ExampleStateManager.State.Test1);
			Assert.IsTrue(stateManager.Current.GetID() == (int)ExampleStateManager.State.Test1);
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
