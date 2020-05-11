namespace TDD_in_Unity.Infrastructure
{
	public abstract class TestDataBuilder<T>
	{
		public abstract T Build();

		// 암시적으로 Image로 변환될때 아래 구문이 동작하는 문법, static implicit operator
		public static implicit operator T(TestDataBuilder<T> builder) {
			return builder.Build();
		}
	}
}
