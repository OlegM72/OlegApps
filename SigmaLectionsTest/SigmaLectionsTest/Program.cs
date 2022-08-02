using System; // Console, Math, DateTime etc
using System.IO; // File, StreamReader etc
using System.Text; // StringBuilder
using System.Collections; // неузагальнені класи та інтерфейси колекцій
using System.Collections.Specialized; // строго типізовані неузагальнені класи та інтерфейси колекцій 
using System.Collections.Generic; // узагальнені класи та інтерфейси колекцій
using System.Collections.ObjectModel; // проксі та базові класи для спеціальних колекцій
using System.Collections.Concurrent; // потокобезпечні колекції
using System.Linq;
using System.Collections.Immutable;

namespace SigmaLectionsTest
{
	class Lecture_3_1
	{
		// Зчислення, що визначає дні тижня 
		enum Days { Mon, Tue, Wed, Thi, Fri, Sat, Sun };

		public static void Lecture31()
		{
			// 1. Оголосити екземпляр зчислення 
			Days d;
			// 2. Встановити день тижня вівторок 
			d = Days.Tue;
			try
			{
				// 3. Отримати назву дня тижня у вигляді рядка 
				string strDay = Enum.GetName(typeof(Days), d);
				string strDay2 = d.ToString();
				// 4. Вивести назву дня тижня 
				Console.WriteLine("strDay = {0}", strDay); // strDay = Tue 
				Console.WriteLine("strDay2 = {0}", strDay2); // strDay2 = Tue 
			}
			catch (System.ArgumentNullException e)
			{
				Console.WriteLine(e.Message);
			}
			catch (System.ArgumentException e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}

	class Lecture_3_2
	{
		// Оголошення типу зчислення Months, яке описує місяці року 
		// всередині класу.
		// Jan = 1, Feb = 2, Mar = 3, Apr = 4 , ..., Dec = 12
		enum Months { Jan = 1, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec };
		public static void Lecture32()
		{
			// Вивести кількість днів у місяці,
			// використати змінну типу Enum 
			Console.WriteLine("Enter number of month (1..12):");
			int month = Int32.Parse(Console.ReadLine());
			// перевірка, чи коректно введений місяць 
			if ((month < 1) || (month > 12))
			{
				Console.WriteLine("Wrong input."); return;
			}
			Months MN; // змінна типу Months
					   // Ініціалізувати MN значенням на основі змінної month.
					   // Метод Enum.GetValues() повертає клас System.Array.
					   // У класі System.Array є метод GetValue(), який повертає значення об'єкту за індексом (0..11). 
			MN = (Months)Enum.GetValues(typeof(Months)).GetValue(month - 1);
			switch (MN)
			{
				case Months.Feb:
					Console.WriteLine("28 (29) days");
					break;
				case Months.Apr:
				case Months.Jun:
				case Months.Sep:
				case Months.Nov:
					Console.WriteLine("30 days");
					break;
				default:
					Console.WriteLine("31 days");
					break;
			}
		}
	}

	class Person
	{
		protected string name;
		protected int age;

		public Person(string name)
		{
			this.name = name;
			Console.WriteLine($"Person({name})");
		}
		public Person(string name, int age) : this(name)
		{
			this.age = age;
			Console.WriteLine($"Person({name}, {age})");
		}
	}
	class Employee : Person
	{
		string company;

		public Employee(string name, int age, string company) : base(name, age)
		{
			this.company = company;
			Console.WriteLine($"Employee({name}, {age}, {company})");
		}

		public void Method()
		{
			// protected or public fields of base class are also reachable
			Console.WriteLine($"Method: {name}, {age}, {company}");
		}
	}

	// The Point class is derived from System.Object.
	class Point
	{
		public int x, y;

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public override bool Equals(object obj)
		{
			// If this and obj do not refer to the same type, then they are not equal.
			if (obj.GetType() != this.GetType()) return false;

			// Return true if x and y fields match.
			var other = (Point)obj;
			return (this.x == other.x) && (this.y == other.y);
		}

		// Return the XOR of the x and y fields.
		public override int GetHashCode()
		{
			return x ^ y;
		}

		// Return the point's value as a string.
		public override string ToString()
		{
			return $"({x}, {y})";
		}

		// Return a copy of this point object by making a simple field copy.
		public Point Copy()
		{
			return (Point)this.MemberwiseClone();
		}

		public static void Lecture33()
		{
			// Construct a Point object.
			var p1 = new Point(1, 2);
			// Make another Point object that is a copy of the first.
			var p2 = p1.Copy();
			// Make another variable that references the first Point object.
			var p3 = p1;
			// The line below displays false because p1 and p2 refer to two different objects.
			Console.WriteLine(Object.ReferenceEquals(p1, p2));
			// The line below displays true because p1 and p2 refer to two different objects that have the same value.
			Console.WriteLine(Object.Equals(p1, p2));
			// The line below displays true because p1 and p3 refer to one object.
			Console.WriteLine(Object.ReferenceEquals(p1, p3));
			// The line below displays: p1's value is: (1, 2)
			Console.WriteLine($"p1's value is: {p1.ToString()}");
			var a = p1.MemberwiseClone(); // copy of object!
										  // The line below displays: a's value is: (1, 2)
			Console.WriteLine($"a's value is: {a.ToString()}");
		}
	}

	interface I1
	{
		void Foo();
	}
	interface I2
	{
		int Foo();
	}
	public class Widget : I1, I2
	{
		public void Foo()
		{
			Console.WriteLine("Widget’s implementation of I1.Foo");
		}
		int I2.Foo()
		{
			Console.WriteLine("Widget's implementation of I2.Foo");
			return 42;
		}
	}

	public interface IUndoable { void Undo() { } }
	public class TextBox : IUndoable
	{
		public virtual void Undo() => Console.WriteLine("TextBox.Undo");
	}
	public class RichTextBox : TextBox
	{
		public override void Undo() => Console.WriteLine("RichTextBox.Undo");
	}
	public class TextBoxV : IUndoable
	{
		void IUndoable.Undo() => Console.WriteLine("TextBoxV.Undo");
	}
	public class RichTextBoxV : TextBoxV, IUndoable
	{
		public void Undo() => Console.WriteLine("RichTextBoxV.Undo");
	}

	public class Matrices
	{
		public static void Lecture41()
		{
			Console.WriteLine("Введіть кількість рядків матриці");
			int rowCount = Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("Введіть кількість стовпців матриці ");
			int colCount = Convert.ToInt32(Console.ReadLine());
			int[,] matr = new int[rowCount, colCount];
			if (rowCount == colCount && rowCount % 2 != 0)
			{
				int s = rowCount / 2;
				for (int i = 0; i < s; ++i)
				{
					for (int j = i; j <= rowCount - i - 1; j++)
					{
						matr[i, j] = 1;
					}

				}
				for (int i = s + 1; i < rowCount; ++i)
				{
					for (int j = rowCount - i - 1; j <= i; ++j)
					{
						matr[i, j] = 2;
					}

				}
				matr[s, s] = 3;

			}
			else
			{
				for (int j = 0; j < colCount; j++)
				{
					matr[0, j] = 1;
				}
				for (int i = 1; i < rowCount; ++i)
				{
					matr[i, colCount - 1] = 2;
				}
				for (int j = colCount - 2; j >= 0; --j)
				{
					matr[rowCount - 1, j] = 3;
				}
				for (int i = rowCount - 2; i > 0; --i)
				{
					matr[i, 0] = 4;
				}
			}
			for (int i = 0; i < rowCount; ++i)
			{
				for (int j = 0; j < colCount; ++j)
				{
					Console.Write(matr[i, j] + " ");
				}
				Console.WriteLine();
			}
		}

		public static void Lecture42()
		{
			int[][] matr = new int[4][];
			for (int i = 0; i < 4; i++)
			{
				matr[i] = new int[i + 1]; // create new array of the level i
				for (int j = 0; j < i + 1; j++)
				{
					matr[i][j] = j + 1;  // fill the array
				}
			}
			for (int i = 0; i < 4; ++i)
			{
				for (int j = 0; j < i + 1; ++j)
				{
					Console.Write(matr[i][j] + " ");
				}
				Console.WriteLine();
			}
		}
	}

	class TryTest
	{
		static int Calc(int x) => 10 / x;
		static public void Test1()
		{
			try
			{
				int y = Calc(0);
				Console.WriteLine(y);
			}
			catch (DivideByZeroException ex)
			{
				Console.WriteLine("x cannot be zero"); //значення x не може бути 0
			}
			Console.WriteLine("part 1 completed");
		}

		static public void Test2(params string[] args)
		{
			try
			{
				byte b = byte.Parse(args[0]);
				Console.WriteLine(b);
			}
			catch (IndexOutOfRangeException ex)
			{
				Console.WriteLine("Please provide at least one argument");
			}
			catch (FormatException ex)
			{
				Console.WriteLine("That's not a number!");
			}
			catch (OverflowException ex)
			{
				Console.WriteLine("You've given me more than a byte!");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.WriteLine("part 2 completed");
			}
		}
		public static void ReadFile()
		{
			StreamReader reader = null;
			try
			{
				reader = File.OpenText("file.txt");
				if (reader.EndOfStream) return;
				Console.WriteLine(reader.ReadToEnd());
			}
			catch (FileNotFoundException ex) { Console.WriteLine(ex.Message); }
			catch (Exception ex) { throw; }
			finally
			{
				if (reader != null) reader.Dispose();
				Console.WriteLine("Dispose");
			}
		}

		public static void Display(string name)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			Console.WriteLine(name);
		}
	}

	public readonly struct Fraction
	{
		private readonly int num;
		private readonly int den;
		public Fraction(int numerator, int denominator)
		{
			if (denominator == 0)
			{
				throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
			}
			num = numerator;
			den = denominator;
		}
		public static Fraction operator +(Fraction a) => a;
		public static Fraction operator -(Fraction a) => new Fraction(-a.num, a.den);
		public static Fraction operator +(Fraction a, Fraction b) => new Fraction(a.num * b.den + b.num * a.den, a.den * b.den);
		public static Fraction operator -(Fraction a, Fraction b) => a + (-b);
		public static Fraction operator *(Fraction a, Fraction b) => new Fraction(a.num * b.num, a.den * b.den);
		public static Fraction operator /(Fraction a, Fraction b)
		{
			if (b.num == 0)
			{
				throw new DivideByZeroException();
			}
			return new Fraction(a.num * b.den, a.den * b.num);
		}
		public override string ToString() => $"{num} / {den}";
	}
	class Counter
	{
		public int Seconds { get; set; }

		public static implicit operator Counter(int x)
		{
			return new Counter { Seconds = x };
		}
		public static explicit operator int(Counter counter)
		{
			return counter.Seconds;
		}
	}


	class Program
	{
		#region Auxiliary Methods
		static void PrintLine<T>(T obj)
		{
			Console.WriteLine(obj);
		}
		static void Print<T>(T obj)
		{
			Console.Write(obj);
		}
		static void PrintLine()
		{
			PrintLine("");
		}
		static void Print()
		{
			Print("");
		}
		static void PrintList<T>(IEnumerable<T> list)
		{
			foreach (T item in list) Print(item + " ");
			PrintLine();
		}
		static void PrintObjects(ICollection collection)
		{
			foreach (object item in collection)
				Print(item.ToString() + " ");
			PrintLine();
		}
		static void PrintDictionary(IDictionary dictionary)
		{
			foreach (DictionaryEntry item in dictionary)
				Print(item.Value.ToString() + " ");
			PrintLine();
		}
		static void PrintGroup<G, T>(IEnumerable<IGrouping<G, T>> list)
		{
			foreach (IGrouping<G, T> group in list)
			{
				Print(">> Group with Key = " + group.Key + ", elements: ");
				foreach (T item in group)
					Print(item + " ");
				PrintLine();
			}
		}
		#endregion

		static void Lecture3()
		{
			Lecture_3_1.Lecture31();
			Lecture_3_2.Lecture32();

			Employee e = new("Oleg", 50, "Sigma Software");
			e.Method();

			Point.Lecture33();

			Widget w = new();
			((I1)w).Foo(); // Widget's implementation of I1.Foo
			((I2)w).Foo(); // Widget's implementation of I2.Foo

			RichTextBox дужеКрасиваЗмінна = new();
			дужеКрасиваЗмінна.Undo(); // RichTextBox.Undo 
			((IUndoable)дужеКрасиваЗмінна).Undo(); // RichTextBox.Undo
			((TextBox)дужеКрасиваЗмінна).Undo(); // RichTextBox.Undo

			RichTextBoxV простоКрасиваЗмінна = new();
			простоКрасиваЗмінна.Undo(); // RichTextBox.Undo
			((IUndoable)простоКрасиваЗмінна).Undo(); // RichTextBox.Undo
		}

		static void Lecture4()
		{
			Matrices.Lecture41();
			Matrices.Lecture42();
		}

		static void Lecture10()
		{
			string a = "test";
			string b = "test";
			Console.WriteLine(a == b); // Truе
			a = "Here's a tab:\t";
			string a1 = "\\\\server\\fileshare\\helloworld.cs";
			Console.WriteLine(a1);
			string a2 = @"\\server\fileshare\helloworld.cs"; //дослівний літерал
			Console.WriteLine(a2);
			string escaped = "First Line\r\nSecond Line";
			string verbatim = @"First Line
Second Line";
			Console.WriteLine(verbatim);
			// Виводить True, якщо в IDE-середовищі використовуються розділювачі стрічок CR-LF: 
			Console.WriteLine(escaped == verbatim);
			int x = 4;
			Console.WriteLine($"А square has {x} sides"); // Виводить: A square has 4 sides
			x = 2;
			string s = $@"this spans 
{x:f2} lines";
			Console.WriteLine(s);// Виводить: this spans \r\n2.00 lines (якщо перенести внутри {}, то в одну!)

			StringBuilder sb = new();
			// (Append),вставляти (Insert), видаляти (Remove) і замінювати (Replace) підрядка
			sb.Append("First words"); // First words
			Console.WriteLine(sb);
			sb.Insert(6, "Inserted words "); // First Inserted words words
			Console.WriteLine(sb);
			sb.Remove(15, 6); // First Inserted words
			Console.WriteLine(sb);
			sb.Replace("words", "phrase"); // First Inserted phrase
			Console.WriteLine(sb);

			Console.WriteLine(new TimeSpan(2, 30, 0)); // 02:30:00
			Console.WriteLine(TimeSpan.FromHours(2.5)); // 02:30:00
			Console.WriteLine(TimeSpan.FromHours(-2.5)); //-02:30:00
			Console.WriteLine(DateTime.Now.TimeOfDay);

			TimeSpan nearlyTenDays = TimeSpan.FromDays(10) - TimeSpan.FromSeconds(1);

			Console.WriteLine(nearlyTenDays.Days); // 9 
			Console.WriteLine(nearlyTenDays.Hours); // 23
			Console.WriteLine(nearlyTenDays.Minutes); // 59
			Console.WriteLine(nearlyTenDays.Seconds); // 59 
			Console.WriteLine(nearlyTenDays.Milliseconds); // 0 

			Console.WriteLine(nearlyTenDays.TotalDays); // 9.999988425925926 
			Console.WriteLine(nearlyTenDays.TotalHours); // 239.999722222222 
			Console.WriteLine(nearlyTenDays.TotalMinutes); // 14399.9833333333 
			Console.WriteLine(nearlyTenDays.TotalSeconds); // 863999 
			Console.WriteLine(nearlyTenDays.TotalMilliseconds); // 863999000

			DateTime dt = new DateTime(2000, 2, 3, 10, 20, 30);
			TimeSpan ts = TimeSpan.FromMinutes(90);
			Console.WriteLine(dt.Add(ts)); // 03.02.2000 11:50:30
			Console.WriteLine(dt + ts); // те ж, що і вище

			DateTime thisYear = new DateTime(2015, 1, 1);
			DateTime nextYear = thisYear.AddYears(1);
			TimeSpan oneYear = nextYear - thisYear;
			Console.WriteLine(oneYear + " = " + oneYear.TotalDays + " days"); // 365.00:00:00 = 365 days

			DateTimeOffset dto = dt; // 03.02.2000 10:20:30
			Console.WriteLine(dto); // 03.02.2000 10:20:30 +02:00 (in Ukraine)
			Console.WriteLine(dt.ToShortDateString()); // 03.02.2000
			Console.WriteLine(dt.ToLongDateString()); // 3 лютого 2000 р.

			var person = (Name: "Олег", Age: 50);
			Console.WriteLine(String.Format("Ім’я: {0} Вік (c): {1:c3}", person.Name, person.Age)); // 50,000 ₴
			Console.WriteLine(String.Format("Ім’я: {0} Вік (d): {1:d3}", person.Name, person.Age)); // 050
			Console.WriteLine(String.Format("Ім’я: {0} Вік (e): {1:e3}", person.Name, person.Age)); // 5,000e+001
			Console.WriteLine(String.Format("Ім’я: {0} Вік (f): {1:f3}", person.Name, person.Age)); // 50,000
			Console.WriteLine(String.Format("Ім’я: {0} Вік (g): {1:g3}", person.Name, person.Age)); // 50
			Console.WriteLine(String.Format("Ім’я: {0} Вік (n): {1:n3}", person.Name, person.Age)); // 50,000
			Console.WriteLine(String.Format("Ім’я: {0} Вік (p): {1:p3}", person.Name, person.Age)); // 5 000,000 %
			Console.WriteLine(String.Format("Ім’я: {0} Вік (x): {1:x3}", person.Name, person.Age)); // 032

			double dnumber = 23.7;
			string result = String.Format("{0:C}", dnumber);
			Console.WriteLine(result); // $ 23.70 ₴
			string result2 = String.Format("{0:C3}", dnumber);
			Console.WriteLine(result2); // $ 23.700 ₴

			int number = 23;
			result = String.Format("{0:d}", number);
			Console.WriteLine(result); // 23
			result2 = String.Format("{0:d4}", number);
			Console.WriteLine(result2); // 0023

			result = String.Format("{0:f}", number);
			Console.WriteLine(result); // 23,00

			double number2 = 45.08;
			result2 = String.Format("{0:f4}", number2);
			Console.WriteLine(result2); // 45,0800

			double number3 = 25.07;
			string result3 = String.Format("{0:f1}", number3);
			Console.WriteLine(result3); // 25,1

			decimal dcnumber = 0.15345m;
			Console.WriteLine("{0:P1}", dcnumber);// 15.3 %

			long lnumber = 19876543210;
			result = String.Format("{0:+# (###) ###-##-##}", lnumber);
			Console.WriteLine(result); // +1 (987) 654-32-10
			Console.WriteLine($"{lnumber:+# (###) ###-##-##}"); // the same: +1 (987) 654-32-10

			TryTest.Test1(); // x cannot be zero
			TryTest.Test2(); // Please provide at least one argument
			TryTest.Test2("A"); // This is not a number!
			TryTest.Test2("21242"); // You've given me more than a byte!
			TryTest.Test2("3"); // 3

			try { int n = 0; int w = 1 / n; }
			catch (Exception ex)
				when (ex.GetType() == typeof(DivideByZeroException))
			{ Console.WriteLine("This should be printed"); }

			TryTest.ReadFile();

			try
			{
				using (StreamReader reader = File.OpenText("file.txt"))
				{
					// compiler inserts here try block and finally block:
					// finally block:
					//	if (reader != null) ((IDisposable)reader).Dispose();
				}
			}
			catch (Exception ex)
			{ Console.WriteLine(ex.Message); }

			try
			{
				TryTest.Display(null);
			}
			catch (ArgumentNullException ex)
			{
				Console.WriteLine("Caught the Null exception: " + ex.Message);
				// Caught the Null exception: Value cannot be null. (Parameter 'name')
			}

			try
			{
			}
			catch (Exception ex)
			{ Console.WriteLine(ex.Message); }

			Func<string, string> ProperCase = (value) =>
				  value == null ? throw new ArgumentException("value")
				  : value == "" ? "" : char.ToUpper(value[0]) + value.Substring(1);
			try
			{
				Console.WriteLine(ProperCase(null));
			}
			catch (Exception ex)
			{ Console.WriteLine(ex.GetType() + ": " + ex.Message); } // System.ArgumentException: value

		}

		public static void Лекция13()
		{
			Fraction fraction1 = new(1, 5);
			Console.WriteLine(fraction1); // 1 / 5
			try
			{
				Fraction fraction2 = new(2, 0); // System.ArgumentException: Denominator cannot be zero. (Parameter 'denominator'
			}
			catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

			Fraction fraction3 = new(0, 5);
			Console.WriteLine(fraction3); // 0 / 5
			Console.WriteLine(+fraction1); // 1 / 5
			Console.WriteLine(-fraction1); // -1 / 5
			Console.WriteLine(fraction1 + fraction3); // 5 / 25
			Console.WriteLine(fraction1 - fraction3); // 5 / 25
			Console.WriteLine(fraction1 * fraction3); // 0 / 25
			try
			{
				Console.WriteLine(fraction1 / fraction3); // System.DivideByZeroException: Attempted to divide by zero.
			}
			catch (Exception ex) { Console.WriteLine(ex.GetType() + ": " + ex.Message); }

			Counter counter1 = new Counter { Seconds = 23 };

			int x = (int)counter1;  // operator int returns counter.Seconds;
			Console.WriteLine(x);   // 23

			Counter counter2 = x;   // implicit operator Counter(x) creates new Counter with Seconds = x;
			Console.WriteLine(counter2.Seconds);  // 23
		}

		public static void Лекция17()
		{
			// неузагальнені класи та інтерфейси колекцій
			System.Array array; // abstract class with methods, cannot create an instance
								// Print(array.Length); - cannot use a variable without a value assigned
			System.Collections.ArrayList arrayList = new() { 1, "string", '}', 2.6, new Point(5, 10), true };
			// PrintList<object>((IEnumerable<object>)arrayList); // System.InvalidCastException: Unable to cast object of type 'System.Collections.ArrayList' to type 'System.Collections.Generic.IEnumerable`1[System.Object]'.
			PrintObjects(arrayList); // 1 string } 2,6 (5, 10) True

			ArrayList list = new();
			list.Add(2.3); // заносим в список об’ект типу double
			list.Add(55); // заносим в список об’ект типу int
			list.AddRange(new string[] { "Hello", "world" }); // заносим в список масив стрічок
			foreach (object o in list)
				Print(o + " "); // 2,3 55 Hello world
			PrintLine();

			// видаляємо перший елемент
			list.RemoveAt(0);
			// повертаємо список
			list.Reverse();
			// отримання елементу по індексу
			PrintLine(list[0]); // world
								// перебір значень
			for (int i = 0; i < list.Count; i++)
				Print(list[i] + " "); // world Hello 55
			PrintLine();

			System.Collections.BitArray bitArray = new(new byte[] { 8, 15 });
			PrintObjects(bitArray); // False False False True False False False False True True True True False False False False
			System.Collections.Queue queue = new();
			queue.Enqueue(1); queue.Enqueue("string"); queue.Enqueue('}'); queue.Enqueue(2.6); queue.Enqueue(new Point(5, 10)); queue.Enqueue(true);
			PrintObjects(queue); // 1 string } 2,6 (5, 10) True
								 // Hashtable compares keys by hashcodes and throws an exception for double entry
			System.Collections.Hashtable hashtable = new() { { 5, 1 }, { 4, "string" }, { 3, '}' }, { 2, 2.6 }, { 1, new Point(5, 10) }, { 0, true } }; //, { 5, 5 } };
			PrintDictionary(hashtable); // 1 string } 2,6 (5, 10) True
										// System.Collections.Immutable.ImmutableArray immutableArray; - static class, cannot create an instance, no understand
										// System.Collections.Immutable.ImmutableHashSet immutableHashSet; - the same
										// System.Collections.Immutable.ImmutableQueue immutableQueue; - the same
										// System.Collections.Immutable.ImmutableStack immutableStack; - the same
										// SortedList: single entry only, throws exception, sorted by key
			System.Collections.SortedList sortedList = new() { { 5, 1 }, { 4, "string" }, { 3, '}' }, { 2, 2.6 }, { 1, new Point(5, 10) }, { 0, true } };
			PrintDictionary(sortedList); // True (5, 10) 2,6 } string 1
			System.Collections.Stack stack = new();
			stack.Push(1); stack.Push("string"); stack.Push('}'); stack.Push(2.6); stack.Push(new Point(5, 10)); stack.Push(true);
			PrintObjects(stack); // True (5, 10) 2,6 } string 1
			PrintLine();

			// узагальнені класи та інтерфейси колекцій
			System.Collections.Generic.Dictionary<string, int> dictionary = new() { { "First", 1 }, { "Second", 2 }, { "Third", 3 } };
			dictionary["Fourth"] = 4;
			PrintObjects(dictionary); // [First, 1] [Second, 2] [Third, 3] [Fourth, 4]
			PrintDictionary(dictionary); // 1 2 3 4
										 // HashSet works like set, only single entry (by hash), no exception when double entry, not sorted
			System.Collections.Generic.HashSet<int> hashSet = new() { 1, 10, 40, 15, 17, 2, 10, 2 };
			PrintList(hashSet); // 1 10 40 15 17 2
			System.Collections.Immutable.ImmutableList<int> immutableList; // Does not have a constructor. Is readonly
																		   // immutableList.Add(1); - error: using variable without the value assigned. Don't understand how to use
			System.Collections.Immutable.ImmutableArray<int> immutableArrayGeneric; // the same
			System.Collections.Immutable.ImmutableDictionary<string, int> immutableDictionary; // the same
			System.Collections.Immutable.ImmutableHashSet<int> immutableHashSetGeneric; // the same
			System.Collections.Immutable.ImmutableQueue<int> immutableQueueGeneric; // the same
			System.Collections.Immutable.ImmutableSortedDictionary<string, int> immutableSortedDictionary; // the same
			System.Collections.Immutable.ImmutableStack<int> immutableStackGeneric; // the same
			System.Collections.Generic.LinkedList<int> linkedList = new() { };
			linkedList.AddFirst(1); linkedList.AddLast(2); linkedList.AddAfter(linkedList.Find(1), 3); linkedList.AddBefore(linkedList.First, 4);
			PrintList(linkedList); // 4 1 3 2
			System.Collections.Generic.List<int> list2 = new() { 1, 10, 40, 15, 17, 2, 10, 2 };
			PrintList(list2); // 1 10 40 15 17 2 10 2
			list2.ForEach(n => { n = n * 2; }); // does not change the values
			PrintList(list2); // 1 10 40 15 17 2 10 2
			list2.ForEach(n => Print(n * 2 + " ")); // 2 20 80 30 34 4 20 4
			PrintLine();
			System.Collections.Generic.Queue<int> queueGeneric = new(); // typed queue
			queueGeneric.Enqueue(1); queueGeneric.Enqueue(10); queueGeneric.Enqueue(40); queueGeneric.Enqueue(15); queueGeneric.Enqueue(17); queueGeneric.Enqueue(2);
			PrintList(queueGeneric); // 1 10 40 15 17 2
			System.Collections.Generic.SortedDictionary<string, int> sortedDictionary = new() { { "First", 1 }, { "Second", 2 }, { "Third", 3 } };
			sortedDictionary["Fourth"] = 4;
			PrintObjects(sortedDictionary); // [First, 1] [Fourth, 4] [Second, 2] [Third, 3]
			PrintDictionary(sortedDictionary); // 1 4 2 3
											   // SortedList is the same as SortedDictionary but can use Comparer instead of comparing by Key
			System.Collections.Generic.SortedList<string, int> sortedListGeneric = new() { { "First", 1 }, { "Second", 2 }, { "Third", 3 } };
			sortedListGeneric["Fourth"] = 4;
			PrintList(sortedListGeneric); // [First, 1] [Fourth, 4] [Second, 2] [Third, 3]
			PrintDictionary(sortedListGeneric); // 1 4 2 3
												// SortedSet works like set, single entry, no exception, sorted
			System.Collections.Generic.SortedSet<int> sortedSet = new() { 1, 10, 40, 15, 17, 2, 10, 2 };
			PrintList(sortedSet); // 1 2 10 15 17 40
			System.Collections.Generic.Stack<int> stackGeneric = new(); // typed stack
			stackGeneric.Push(1); stackGeneric.Push(2); stackGeneric.Push(3); stackGeneric.Push(4); stackGeneric.Push(5);
			PrintList(stackGeneric); // 5 4 3 2 1
			PrintLine();

			// строго типізовані неузагальнені класи та інтерфейси колекцій 
			System.Collections.Specialized.CollectionsUtil collectionsUtil = new(); // Creates collections that ignore the case in strings. Allows to create different HashTables and Dictionaries
			PrintLine(collectionsUtil + " - to create other collections"); // collectionsUtil - to create other collections
																		   // HybridDictionary is ListDictionary when it is small and is HashTable when large
			System.Collections.Specialized.HybridDictionary hybridDictionary = new() { { "First", 1 }, { "Second", 2 }, { "Third", 3 } };
			hybridDictionary["Fourth"] = 4;
			PrintDictionary(hybridDictionary); // 1 2 3 4
			System.Collections.Specialized.ListDictionary listDictionary = new() { { "First", 1 }, { "Second", 2 }, { "Third", 3 } };
			listDictionary["Fourth"] = 4;
			PrintDictionary(listDictionary); // 1 2 3 4
			System.Collections.Specialized.NameValueCollection nameValueCollection = new() { { "First", "1" }, { "Second", "2" }, { "Third", "3" } };
			nameValueCollection["Fourth"] = "4";
			PrintObjects(nameValueCollection); // First Second Third Fourth (type: DictionaryEntry)
			System.Collections.Specialized.OrderedDictionary orderedDictionary = new() { { "First", 1 }, { "Second", 2 }, { "Third", 3 } };
			orderedDictionary["Fourth"] = 4;
			foreach (DictionaryEntry item in orderedDictionary) Print(item.Value + " "); // 1 2 3 4
			PrintLine();
			System.Collections.Specialized.StringCollection stringCollection = new() { "Uno", "Dos", "Tres", "Cuatro" };
			PrintObjects(stringCollection); // Uno Dos Tres Cuatro
			System.Collections.Specialized.StringDictionary stringDictionary = new() { { "First", "1" }, { "Second", "2" }, { "Third", "3" } };
			stringDictionary["Fourth"] = "4";
			foreach (DictionaryEntry item in orderedDictionary) Print(item.Value + " "); // 1 2 3 4
			PrintLine();

			// проксі та базові класи для спеціальних колекцій
			System.Collections.ObjectModel.Collection<int> collection = new() { 1, 10, 40, 15, 17, 2, 10, 2 };
			PrintList(collection); // 1 10 40 15 17 2 10 2
								   // абстрактные классы:
			System.Collections.ObjectModel.KeyedCollection<string, int> keyedCollection;
			System.Collections.ObjectModel.ObservableCollection<int> observableCollection;
			System.Collections.ObjectModel.ReadOnlyCollection<int> readOnlyCollection;
			System.Collections.ObjectModel.ReadOnlyDictionary<string, int> readOnlyDictionary;
			System.Collections.ObjectModel.ReadOnlyObservableCollection<int> readOnlyObservableCollection;
			PrintLine();

			// потокобезпечні колекції
			System.Collections.Concurrent.BlockingCollection<int> blockingCollection = new() { 1, 10, 40, 15, 17, 2, 10, 2 };
			PrintList(blockingCollection); // 1 10 40 15 17 2 10 2
			System.Collections.Concurrent.ConcurrentBag<int> concurrentBag = new() { 1, 10, 40, 15, 17, 2, 10, 2 }; // like stack for streams
			PrintList(concurrentBag); // 2 10 2 17 15 40 10 1
			System.Collections.Concurrent.ConcurrentDictionary<string, int> concurrentDictionary = new();
			PrintList(concurrentDictionary); // 
			System.Collections.Concurrent.ConcurrentQueue<int> concurrentQueue = new();
			PrintList(concurrentQueue); // 
			System.Collections.Concurrent.ConcurrentStack<int> concurrentStack = new();
			PrintList(concurrentStack); // 
										// абстрактные классы:
			System.Collections.Concurrent.Partitioner<int> partitioner;
			System.Collections.Concurrent.OrderablePartitioner<string> orderablePartitioner;

			// IEnumerable<T>: визначає метод GetEnumerator, за допомогою якого можна отримувати елементи будь-якої колекції. Реалізація даного інтерфейсу дозволяє перебирати елементи колекції за допомогою циклу foreach
			// IEnumerator<T>: визначає методи, за допомогою яких потім можна отримати вміст колекції по черзі
			// ICollection<T>: подає ряд загальних властивостей і методів для всіх узагальнених колекцій(наприклад, методи CopyTo, Add, Remove, Contains, властивість Count)
			// IList<T>: надає функціонал для створення послідовних списків IComparer<T>: визначає метод Compare для порівняння двох однотипних об'єктів
			// IDictionary<TKey, TValue>: визначає поведінку колекції, при якому вона повинна зберігати об'єкти у вигляді пар ключ-значення: для кожного об'єкта визначається унікальний ключ типу, наданого в секції TKey, і цього ключа відповідає певне значення, яке має тип, вказаний в параметрі Tvalue
			// IEqualityComparer<T>: визначає методи, за допомогою яких два однотипних об'єкта порівнюються на предмет рівності 
		}

		delegate void D(int i);

		static void F(int i)
		{
			Console.Write($"F({i}) ");
		}
		void G(int i)
		{
			Console.Write($"G({i}) ");
		}

		delegate T DoubleSomething<T>(T x);

		public static int DoubleInt(int x) => x * 2;
		public static string DoubleString(string x) => x + x;

		delegate T PlusOne<T>(T x);
		static int f(int i) { return i + 1; }
		static string g(string s) { return s + "1"; }

		delegate void Message();
		private static void Hello() { Console.WriteLine("Hello"); }
		private static void HowAreYou() { Console.WriteLine("How are you?"); }

		public class AccountEventArgs : EventArgs
		{
			public string Message { get; set; }
			public int Sum { get; set; }
			public AccountEventArgs(string message, int sum) : base()
			{
				Message = message;
				Sum = sum;
			}
		}

		class Account
		{
			int _sum; // Змінна для зберігання суми
			int _percentage; // Змінна для зберігання відсотка
			public Account(int sum, int percentage) { _sum = sum; _percentage = percentage; }
			public void Withraw(int sum)
			{
				if (sum <= _sum)
					_sum -= sum;
			}
			public int CurrentSum { get { return _sum; } }
			public int Percentage { get { return _percentage; } }
			public void Put(int sum) {
				_sum += sum;
				if (Added != null)
					// Added?.Invoke(this, new AccountEventArgs("На рахунок поступило " + sum, sum));
					Added("На рахунок поступило " + sum + ", маємо " + CurrentSum);
				// это то же, что и 
				// Added.Invoke("На рахунок поступило " + sum + ", маємо " + CurrentSum);
			}
			public void WithdrawWithDelegate(int sum) {
				if (sum <= _sum) {
					_sum -= sum;
					if (del != null)
						del("Сума " + sum + " знята з рахунку, маємо " + CurrentSum);
				}
				else {
					if (del != null)
						del("Недостатньо грошей на рахунку, маємо " + CurrentSum);
				}
			}
			public void WithdrawWithEvent(int sum) {
				if (sum <= _sum) {
					_sum -= sum;
					if (Withdrowed != null)
						Withdrowed("Сума " + sum + " знята з рахунку, маємо " + CurrentSum);
				}
				else {
					if (Withdrowed != null)
						Withdrowed("Недостатньо грошей на рахунку, маємо " + CurrentSum);
				}
			}

			public delegate void AccountStateHandler(string message); // Оголошуємо делегат
			public event AccountStateHandler Withdrowed; // Подія, що виникає при виведенні грошей
			public event AccountStateHandler Added; // Подія, що виникає при додаванні грошей
													// (підписка на події повинна відбуватись в іншому, "клієнтскому" класі, ніж той, де самі події)
			AccountStateHandler del; // Створюємо змінну делегата
			public void RegisterHandler(AccountStateHandler _delegate)
			{
				del = _delegate;  // Реєструємо делегат
			}

			public event EventHandler ThresholdReached;

			protected virtual void OnThresholdReached(EventArgs e)
			{
				EventHandler handler = ThresholdReached;
				handler?.Invoke(this, e);
			}

		}
		private static void Show_Message(string message) { Console.WriteLine(message); }

		static protected void OnWithdrowed(object sender, AccountEventArgs args)
		{
			Account.AccountStateHandler handler = Show_Message;
			handler?.Invoke(args.Message);
		}

		public class VariableCaptureGame
		{
			internal Action<int>? updateCapturedLocalVariable;
			internal Func<int, bool>? isEqualToCapturedLocalVariable;

			public void Run(int input)
			{
				int j = 0;

				updateCapturedLocalVariable = x =>
				{
					j = x;
					bool result = j > input;
					Console.WriteLine($"{j} is greater than {input}: {result}");
				};

				isEqualToCapturedLocalVariable = x => x == j;

				Console.WriteLine($"Local variable before lambda invocation: {j}");
				updateCapturedLocalVariable(10);
				Console.WriteLine($"Local variable after lambda invocation: {j}");
			}
		}

		private static void ShowWindowsMessage(string message)
		{
			Show_Message(message);
		}

		private static string UppercaseString(string inputString)
		{
			return inputString.ToUpper();
		}

		public delegate string ConvertMethod(string inString);

		public class HockeyTeam
		{
			private string _name;
			private int _founded;

			public HockeyTeam(string name, int year)
			{
				_name = name;
				_founded = year;
			}

			public string Name
			{
				get { return _name; }
			}

			public int Founded
			{
				get { return _founded; }
			}
		}

		public static void Лекция18()
		{
			D d = new D(F); // скорочено: D d = F;
			d += new D(new Program().G); // то же, що d += G
			d += F;
			d(0); // виклик усіх методів: F, G, F. Результат: F(0) G(0) F(0)
			Console.WriteLine();

			DoubleSomething<int> intDelegate = DoubleInt;
			DoubleSomething<string> stringDelegate = DoubleString;
			Console.WriteLine(intDelegate(5)); // 10
			Console.WriteLine(stringDelegate("5")); // 55

			PlusOne<int> d1 = f;
			PlusOne<string> d2 = g;
			Console.WriteLine(d1(5)); // 6
			Console.WriteLine(d2("5")); // 51

			Message mes1 = Hello;
			mes1 += HowAreYou;  // тепер mes1 вказує на два методи
			mes1(); // викликаються обидва метода - Hello и HowAreYou

			Message mes2 = HowAreYou;
			Message mes3 = mes1 + mes2; //об’єднуєм делегати
			mes3(); // викликаються всі методи з mes1 и mes2 - Hello, How are you?, How are you?

			mes1 = Hello;
			mes1 += HowAreYou;
			mes1(); // викликаються всі методи з mes1 - Hello и HowAreYou
			mes1 -= HowAreYou;  // видаляємо метод HowAreYou
			mes1(); // викликається метод Hello

			Message mes = null;
			// mes();        //  Помилка: делегат рівний null = System.NullReferenceException: Object reference not set to an instance of an object.
			mes?.Invoke();        // помилки немає, делегат просто не викликається - так працює операція "?"

			Message mes4 = Hello;
			mes4 -= Hello;      // делегат mes4 порожній
								// mes4();       // Помилка: делегат рівний null = System.NullReferenceException: Object reference not set to an instance of an object.
			mes4?.Invoke();   // помилки немає, делегат просто не викликається - так працює операція "?"

			Account account = new Account(200, 6); // створюємо банківський рахунок
												   // Додаємо у делегат посилання на метод Show_Message
												   // а сам делегат передається як параметр методу RegisterHandler
			account.RegisterHandler(new Account.AccountStateHandler(Show_Message));
			account.WithdrawWithDelegate(100); // Сума 100 знята з рахунку, маємо 100
											   // Два рази поспіль намагаємося зняти гроші
			account.WithdrawWithDelegate(150); // /Недостатньо грошей на рахунку, маємо 100

			account.Withdrowed += Show_Message; // підписка на івенти
			account.Added += Show_Message;
			account.Put(50); // На рахунок поступило 50, маємо 150
			account.WithdrawWithEvent(200); // Недостатньо грошей на рахунку, маємо 150

			OnWithdrowed(account, new AccountEventArgs("Test message", 200)); // Test message

			Func<string, int> parse = (s) => int.Parse(s);
			Func<bool, object> choose = b => b ? 1 : "two";

			Func<int, int, int> sum = (int a, int b) => a + b;
			Func<int, int> inc = (int s) => s++;

			var game = new VariableCaptureGame();
			int gameInput = 5;
			game.Run(gameInput);
			// Local variable before lambda invocation: 0
			// 10 is greater than 5: True
			// Local variable after lambda invocation: 10
			int jTry = 10;
			bool result = game.isEqualToCapturedLocalVariable!(jTry);
			Console.WriteLine($"Captured local variable is equal to {jTry}: {result}"); // Captured local variable is equal to 10: True
			int anotherJ = 3;
			game.updateCapturedLocalVariable!(anotherJ);  // 3 is greater than 5: False
			bool equalToAnother = game.isEqualToCapturedLocalVariable(anotherJ);
			Console.WriteLine($"Another lambda observes a new value of captured variable: {equalToAnother}"); // Another lambda observes a new value of captured variable: True

			Func<int, int, int> strange1 = (_, _) => 0;
			Func<int, int, int> strange2 = (int _, int _) => 0;
			// delegate (int _, int __) { return 0; }           --- does not work

			Action<string> messageTarget;

			if (Environment.GetCommandLineArgs().Length > 1)
				messageTarget = ShowWindowsMessage;
			else
				messageTarget = Console.WriteLine;

			messageTarget("Hello, World! 1"); // Hello, World! 1

			if (Environment.GetCommandLineArgs().Length > 1)
				messageTarget = s => ShowWindowsMessage(s);
			else
				messageTarget = s => Console.WriteLine(s);

			messageTarget("Hello, World! 2"); // Hello, World! 2

			Func<string, string> selector = str => str.ToUpper();
			// Create an array of strings.
			string[] words = { "orange", "apple", "Article", "elephant" };
			// Query the array and select strings according to the selector method. 
			IEnumerable<string> aWords = words.Select(selector); // Linq method that transforms a collection
																 // Output the results to the console. 
			foreach (string word in aWords) Console.Write(word + " "); // ORANGE APPLE ARTICLE ELEPHANT
			Console.WriteLine();

			// Instantiate delegate to reference UppercaseString method
			ConvertMethod convertMeth = UppercaseString;
			string name = "Dakota";
			// Use delegate instance to call UppercaseString method
			Console.WriteLine(convertMeth(name)); // DAKOTA

			// Instantiate delegate to reference UppercaseString method
			Func<string, string> convertMethod = UppercaseString;
			string name2 = "Dakota 2";
			// Use delegate instance to call UppercaseString method
			Console.WriteLine(convertMethod(name2)); // DAKOTA 2

			Random rnd = new Random();
			List<HockeyTeam> teams = new List<HockeyTeam>();
			teams.AddRange(new HockeyTeam[] { new HockeyTeam("Detroit Red Wings", 1926),
											  new HockeyTeam("Chicago Blackhawks", 1926),
											  new HockeyTeam("San Jose Sharks", 1991),
											  new HockeyTeam("Montreal Canadiens", 1909),
											  new HockeyTeam("St. Louis Blues", 1967) });
			int[] years = { 1920, 1930, 1980, 2000 };
			int foundedBeforeYear = years[rnd.Next(0, years.Length)];
			Console.WriteLine("Teams founded before {0}:", foundedBeforeYear); // Teams founded before 1920:
			// Predicate<HockeyTeam> as a lambda. x is HockeyTeam, FindAll returns List<HockeyTeam>
			foreach (HockeyTeam team in teams.FindAll(x => x.Founded <= foundedBeforeYear))
				Console.WriteLine("{0}: {1}", team.Name, team.Founded);        // Montreal Canadiens: 1909
		}


		static void Main(string[] args)
		{
			Console.InputEncoding = Encoding.Unicode;
			Console.OutputEncoding = Encoding.Unicode;
			// Lecture3(); // enums, exceptions, Equals, ReferenceEquals
			// Lecture4(); // matrices
			// Lecture10(); // strings, StringBuilder, string formats, try-catch
			// Лекция13(); // try-catch cont.
			// Лекция17(); // collections
			// Лекция18(); // Lambdas, delegates, events

		} // Main
	} // Program class
} // namespace
