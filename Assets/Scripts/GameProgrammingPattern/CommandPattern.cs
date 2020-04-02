using UnityEngine;

namespace ProgrammingPattern
{
	// CommandParttern
	public class Command<T>
	{
		public virtual void Excute(T actor) { }
	}
}
