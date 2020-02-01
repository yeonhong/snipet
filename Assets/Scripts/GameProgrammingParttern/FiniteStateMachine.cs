using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Linq;

namespace ProgrammingPattern
{
	public abstract class FiniteStateMachine<T>
	{
		public interface IState
		{
			T GetID();
			void OnEnter();
			void OnLeave();
		}

		private List<IState> listStates_ = new List<IState>();
		public IState Current { get; protected set; }

		public abstract void Initialize();

		protected virtual void AddState(IState state)
		{
			Assert.IsFalse(listStates_.Contains(state));
			listStates_.Add(state);
		}

		public virtual void ChangeState(T nextID)
		{
			var nextState = listStates_.Where(o => o.GetID().Equals(nextID)).SingleOrDefault();
			Assert.IsTrue(nextState != null);

			Current?.OnLeave();
			Current = nextState;
			Current?.OnEnter();
		}
	}
}
