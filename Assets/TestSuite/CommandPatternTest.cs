using NUnit.Framework;
using ProgrammingPattern;
using UnityEngine;

namespace Tests.ProgrammingPattern
{
	public class Actor
	{
		public Vector3 position = Vector3.zero;
	}

	public class JumpCommand : Command<Actor>
	{
		public override void Excute(Actor actor) {
			actor.position.y--;
		}
	}

	public class MoveCommand : Command<Actor>
	{
		public override void Excute(Actor actor) {
			actor.position.y--;
			actor.position.x--;
		}
	}

	public class CommandPatternTest
	{
		Actor testActor = null;
		JumpCommand jumpCommand = null;


		[SetUp]
		public void Setup() {
			testActor = new Actor();
			jumpCommand = new JumpCommand();
		}

		[Test]
		public void ChangeStateTest() {
			var expectVal = testActor.position.y - 1;
			
			jumpCommand.Excute(testActor);

			Assert.AreEqual(expectVal, testActor.position.y);
		}
	}
}
