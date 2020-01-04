using System;
using UnityEngine.Events;

namespace ProgrammingPattern
{
	// observer.

	#region 전통적인 구조?
	public class SampleEventArgs
	{
		public SampleEventArgs(string s) { Text = s; }
		public String Text { get; } // readonly
	}

	public class Publisher
	{
		// Declare the delegate (if using non-generic pattern).
		public delegate void SampleEventHandler(object sender, SampleEventArgs e);

		// Declare the event.
		public event SampleEventHandler SampleEvent;

		// Wrap the event in a protected virtual method
		// to enable derived classes to raise the event.
		protected virtual void RaiseSampleEvent()
		{
			// Raise the event in a thread-safe manner using the ?. operator.
			SampleEvent?.Invoke(this, new SampleEventArgs("Hello"));
		}
	}

	public class ObserverTest
	{
		Publisher pub_ = new Publisher();

		ObserverTest()
		{
			pub_.SampleEvent += ScbscribeMethod;
		}

		~ObserverTest()
		{
			pub_.SampleEvent -= ScbscribeMethod;
		}

		private void ScbscribeMethod(object sender, SampleEventArgs e)
		{
			throw new NotImplementedException();
		}
	} 
	#endregion

	public class ObserverUnity
	{
		UnityEvent OnDummyEvent = new UnityEvent();

		void Invoking()
		{
			OnDummyEvent.Invoke();
		}

		void Add()
		{
			OnDummyEvent.AddListener(OnListen);
		}

		private void OnListen()
		{
			throw new NotImplementedException();
		}

		void Remove()
		{
			OnDummyEvent.RemoveListener(OnListen);
		}
	}
}