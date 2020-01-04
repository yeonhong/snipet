using UnityEngine;

namespace ProgrammingPattern
{
	public class Actor
	{
		public Vector3 position;
	}

	// CommandParttern
	public class Command<T>
	{
		public virtual void Excute(T actor) { }
	}

	public class JumpCommand : Command<Actor>
	{
		public override void Excute(Actor actor)
		{
			actor.position.y--;
		}
	}

	public class MoveCommand : Command<Actor>
	{
		public override void Excute(Actor actor)
		{
			actor.position.y--;
			actor.position.x--;
		}
	}
}
