namespace ProgrammingPattern
{
	// PrototypePattern
	// 원형객체를 활용하여 객체를 복사하거나 파생객체를 생성하는 패턴.

	class Monster
	{
		public virtual Monster Clone()
		{
			return new Monster();
		}
	}

	class Ghost : Monster
	{
		private int speed;

		public Ghost(int speed)
		{
			this.speed = speed;
		}

		public override Monster Clone()
		{
			return new Ghost(speed);
		}
	}

	class Spawner<T> where T : Monster
	{
		private T protoType;

		public Spawner(T protoType)
		{
			this.protoType = protoType;
		}

		public Monster Spawn()
		{
			return protoType.Clone();
		}
	}

	class SpawnTest
	{
		Spawner<Monster> s;

		void Test()
		{
			s = new Spawner<Monster>(new Ghost(3));
			s.Spawn();
		}
	}

}
