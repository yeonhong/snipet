using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

// https://nsubstitute.github.io
namespace NSubstituteExample
{
	[TestFixture]
	public class NSubstituteExampleTests
	{
		public interface ICalculator
		{
			int Add(int v1, int v2);
			string Mode { get; set; }
		}

		public interface ICommand
		{
			void Execute();
			event EventHandler Executed;

			void Run(IConnection connection);
		}

		public interface IConnection
		{
			Action SomethingHappened { get; set; }

			void Open();
			void Close();
		}

		public class Controller
		{
			private IConnection connection;
			private ICommand command;

			public Controller(IConnection connection, ICommand command) {
				this.connection = connection;
				this.command = command;
			}

			public void DoStuff() {
				connection.Open();
				command.Run(connection);
				connection.Close();
			}
		}

		public class SomethingThatNeedsACommand
		{
			private ICommand command;
			public SomethingThatNeedsACommand(ICommand command) {
				this.command = command;
			}
			public void DoSomething() { command.Execute(); }
			public void DontDoAnything() { }
		}

		public interface ISomething
		{
			void Something();
			event Action<int> responseAction;
			event EventHandler responseEvent;
		}

		private ICalculator calculator;

		[SetUp]
		public void BeforeTests() {
			calculator = Substitute.For<ICalculator>();
		}

		[Test]
		public void NSubstitute의_연습() {
			//Check received calls:
			calculator.Add(1, 2);

			calculator.Received().Add(1, Arg.Any<int>());
			calculator.DidNotReceive().Add(2, 2);

			// 해당 인스턴스가 포함되어 있는지.
			Assert.IsInstanceOf<ICalculator>(calculator);
		}

		[Test]
		public void Substitutiing_for_delegates() {
			var func = Substitute.For<Func<string>>();

			func().Returns("hello");
			Assert.AreEqual("hello", func());
		}

		[Test]
		public void Return_Value_Methods() {
			calculator.Add(1, 2).Returns(3);

			Assert.AreEqual(3, calculator.Add(1, 2));
			Assert.AreNotEqual(calculator.Add(3, 6), 3);
		}

		[Test]
		public void Return_Value_Properties() {
			calculator.Mode.Returns("DEC");
			Assert.AreEqual(calculator.Mode, "DEC");

			calculator.Mode = "HEX";
			Assert.AreEqual(calculator.Mode, "HEX");
		}

		[Test]
		public void Return_for_Specific_Args() {
			//Return when first arg is anything and second arg is 5:
			calculator.Add(Arg.Any<int>(), 5).Returns(10);
			Assert.AreEqual(10, calculator.Add(123, 5));
			Assert.AreEqual(10, calculator.Add(-9, 5));
			Assert.AreNotEqual(10, calculator.Add(-9, -9));

			//Return when first arg is 1 and second arg less than 0:
			calculator.Add(1, Arg.Is<int>(x => x < 0)).Returns(345);
			Assert.AreEqual(345, calculator.Add(1, -2));
			Assert.AreNotEqual(345, calculator.Add(1, 2));

			//Return when both args equal to 0:
			calculator.Add(Arg.Is(0), Arg.Is(0)).Returns(99);
			Assert.AreEqual(99, calculator.Add(0, 0));

			calculator.Add(default, default).ReturnsForAnyArgs(100);
			Assert.AreEqual(100, calculator.Add(1, 2));
			Assert.AreEqual(100, calculator.Add(-7, 15));
		}

		[Test]
		public void Reutrn_from_function() {
			calculator
				.Add(Arg.Any<int>(), Arg.Any<int>())
				.Returns(x => (int)x[0] + (int)x[1]);

			Assert.That(calculator.Add(1, 1), Is.EqualTo(2));
			Assert.That(calculator.Add(20, 30), Is.EqualTo(50));
			Assert.That(calculator.Add(-73, 9348), Is.EqualTo(9275));

			// callback
			var counter = 0;
			calculator
				.Add(default, default)
				.ReturnsForAnyArgs(x => {
					counter++;
					return 0;
				});

			calculator.Add(7, 3);
			calculator.Add(2, 2);
			calculator.Add(11, -3);
			Assert.AreEqual(counter, 3);
		}

		[Test]
		public void Multipe_Return_Values() {
			calculator.Mode.Returns("DEC", "HEX", "BIN");
			Assert.AreEqual("DEC", calculator.Mode);
			Assert.AreEqual("HEX", calculator.Mode);
			Assert.AreEqual("BIN", calculator.Mode);

			calculator.Mode.Returns(x => "DEC", x => "HEX", x => { throw new Exception(); });
			Assert.AreEqual("DEC", calculator.Mode);
			Assert.AreEqual("HEX", calculator.Mode);
			Assert.Throws<Exception>(() => { var result = calculator.Mode; });
		}

		[Test]
		public void 해당함수가_호출되었는지_그리고_몇번인지_체크() {
			//Arrange
			var command = Substitute.For<ICommand>();
			var something = new SomethingThatNeedsACommand(command);
			//Act
			something.DoSomething();
			something.DoSomething();
			//Assert
			command.Received().Execute();
			//count call.
			command.Received(2).Execute();
		}

		[Test]
		public void 해당함수_호출내용_정리() {
			//Arrange
			var command = Substitute.For<ICommand>();
			var something = new SomethingThatNeedsACommand(command);
			//Act
			something.DoSomething();
			something.DoSomething();
			//Assert
			command.Received().Execute();

			//Forget previous calls to command
			command.ClearReceivedCalls();

			//count call.
			command.DidNotReceive().Execute();
		}

		[Test]
		public void 해당함수가_호출된적이_있었는지_체크() {
			//Arrange
			var command = Substitute.For<ICommand>();
			var something = new SomethingThatNeedsACommand(command);
			//Act
			something.DontDoAnything();
			//Assert
			command.DidNotReceive().Execute();
		}

		[Test]
		public void 해당프로퍼티가_사용되었었는지_체크() {
			// arrage
			var mode = calculator.Mode;
			calculator.Mode = "TEST";

			//수신 된 getter 속성 호출을 확인하십시오. 결과를 유지하려면 변수에 결과를 할당해야합니다.
			var temp = calculator.Received().Mode;

			//Check received call to property setter with arg of "TEST"
			calculator.Received().Mode = "TEST";
		}

		[Test]
		public void 딕셔너리의_값을_시뮬레이션() {
			var dictionary = Substitute.For<IDictionary<string, int>>();
			dictionary["test"] = 1;

			dictionary.Received()["test"] = 1;
			dictionary.Received()["test"] = Arg.Is<int>(x => x < 5);
		}

		[Test]
		public void 매개변수_조건_매칭하기() {
			calculator.Add(1, -10);

			//Received call with first arg 1 and second arg less than 0:
			calculator.Received().Add(1, Arg.Is<int>(x => x < 0));
			//Received call with first arg 1 and second arg of -2, -5, or -10:
			calculator
				.Received()
				.Add(1, Arg.Is<int>(x => new[] { -2, -5, -10 }.Contains(x)));
			//Did not receive call with first arg greater than 10:
			calculator.DidNotReceive().Add(Arg.Is<int>(x => x > 10), -10);

			// 특정 매개변수 매칭
			calculator.Add(0, 42);
			calculator.Received().Add(Arg.Is(0), Arg.Any<int>());
		}

		[Test]
		public void 콜백의_복합조합_사용예제() {
			var sub = Substitute.For<ISomething>();

			var calls = new List<string>();
			var counter = 0;

			sub
			  .When(x => x.Something())
			  .Do(
				Callback.First(x => calls.Add("1"))
					.Then(x => calls.Add("2"))
					.Then(x => calls.Add("3"))
					.ThenKeepDoing(x => calls.Add("+"))
					.AndAlways(x => counter++)
			  );

			for (int i = 0; i < 5; i++) {
				sub.Something();
			}
			Assert.That(string.Concat(calls), Is.EqualTo("123++"));
			Assert.That(counter, Is.EqualTo(5));
		}

		[Test]
		public void 예외발생하기_Throwing_Exceptions() {
			//For non-voids:
			calculator.Add(-1, -1).Returns(x => { throw new Exception(); });

			//For voids and non-voids:
			calculator
				.When(x => x.Add(-2, -2))
				.Do(x => { throw new Exception(); });

			//Both calls will now throw.
			Assert.Throws<Exception>(() => calculator.Add(-1, -1));
			Assert.Throws<Exception>(() => calculator.Add(-2, -2));
		}

		[Test]
		public void 이벤트와_액션의_발생에_대한_테스트() {
			var something = Substitute.For<ISomething>();

			var wasCalled = false;
			something.responseEvent += (sender, args) => wasCalled = true;
			//Tell the substitute to raise the event with a sender and EventArgs:
			//something.Idling += Raise.EventWith(new object(), new EventArgs());
			something.responseEvent += Raise.Event();
			Assert.True(wasCalled);

			int revvedAt = 0;
			something.responseAction += rpm => revvedAt = rpm;
			something.responseAction += Raise.Event<Action<int>>(123);
			Assert.AreEqual(123, revvedAt);
		}

		public interface ILookup
		{
			bool TryLookup(string v, out string value);
		}

		[Test]
		public void Out_Ref_매개변수의_테스트() {
			//Arrange
			var lookup = Substitute.For<ILookup>();
			lookup
				.TryLookup("hello", out string val)
				.Returns(ret => {
					ret[1] = "world!";
					return true;
				});

			//Act
			var result = lookup.TryLookup("hello", out var value);

			//Assert
			Assert.True(result);
			Assert.AreEqual(value, "world!");
		}

		[Test]
		public void 호출순서를_테스트() {
			// arrage
			var connection = Substitute.For<IConnection>();
			var command = Substitute.For<ICommand>();
			var subject = new Controller(connection, command);

			// act
			subject.DoStuff();

			// assert
			Received.InOrder(() => {
				connection.Open();
				command.Run(connection);
				connection.Close();
			});
		}

		[Test]
		public void SubscribeToEventBeforeOpeningConnection() {
			var connection = Substitute.For<IConnection>();
			connection.SomethingHappened += () => { };
			connection.Open();

			Received.InOrder(() => {
				connection.SomethingHappened += Arg.Any<Action>();
				connection.Open();
			});
		}

		#region 부분적으로 모의동작처리

		public class SummingReader
		{
			public virtual int Read(string path) {
				var s = ReadFile(path);
				return s.Split(',').Select(int.Parse).Sum();
			}
			public virtual string ReadFile(string path) { return "the result of reading the file here"; }
		}

		[Test]
		public void ShouldSumAllNumbersInFile_부분모의동작테스트() {
			var reader = Substitute.ForPartsOf<SummingReader>();
			reader.ReadFile("foo.txt").Returns("1,2,3,4,5"); // CAUTION: real code warning!

			var result = reader.Read("foo.txt");

			Assert.That(result, Is.EqualTo(15));
		}
		#endregion


	}


}
