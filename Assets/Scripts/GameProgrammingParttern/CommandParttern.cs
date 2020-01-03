using UnityEngine;

namespace ProgrammingPattern
{
    public class Actor
    {
        public Vector3 position;
    }

    // CommandParttern
    public class Command
    {
        public virtual void Excute(Actor actor) { }
    }

    public class JumpCommand : Command
    {
        public override void Excute(Actor actor)
        {
            actor.position.y--;
        }
    }
}