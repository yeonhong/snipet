using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Linq;

namespace ProgrammingPattern
{
	public abstract class FiniteState
	{
		protected int ID { get; set; }
		public FiniteState(int id) { ID = id; }
		public int GetID() { return ID; }

		public abstract void OnEnter();
		public abstract void OnLeave();
	}

	public abstract class FiniteStateMachine<T>
	{
		private List<FiniteState> listStates_ = new List<FiniteState>();
		public FiniteState Current { get; protected set; }

		public abstract void Initialize();

		public virtual void AddState(FiniteState state)
		{
			var duplicate = listStates_.Find(o => o.GetID() == state.GetID());
			if (duplicate == null) {
				listStates_.Add(state);
			} else {
				Debug.LogError("duplicate id " + state.GetID());
			}
		}

		public virtual void ChangeState(int nextID)
		{
			var nextState = listStates_.Where(o => o.GetID().Equals(nextID)).SingleOrDefault();
			Assert.IsTrue(nextState != null, string.Format("상태를 찾을 수 없습니다. {0}", nextID));

			Current?.OnLeave();
			Current = nextState;
			Current?.OnEnter();
		}
	}

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
}
