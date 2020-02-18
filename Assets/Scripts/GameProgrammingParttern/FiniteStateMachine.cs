using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

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

		public FiniteStateMachine()
		{
			Initialize();
		}

		protected abstract void Initialize();

		public virtual FiniteState AddState(FiniteState state)
		{
			var duplicate = listStates_.Find(o => o.GetID() == state.GetID());
			if (duplicate == null) {
				listStates_.Add(state);
				return state;
			} else {
				return null;
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
}
