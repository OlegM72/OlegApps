using System; // Console, Math, DateTime etc
using System.IO; // File, StreamReader etc
using System.Text; // StringBuilder
using System.Collections; // неузагальнені класи та інтерфейси колекцій
using System.Collections.Specialized; // строго типізовані неузагальнені класи та інтерфейси колекцій 
using System.Collections.Generic; // узагальнені класи та інтерфейси колекцій
using System.Collections.ObjectModel; // проксі та базові класи для спеціальних колекцій
using System.Collections.Concurrent; // потокобезпечні колекції
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using static System.Environment; // using a class allows to use its fields directly without class mentioning
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.Threading; // async, await, Task

// [assembly:CLSCompliant(true)]
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

	[DataContract]  // -- cannot be used for ISerializable classes
	[Serializable]
	class SerializableDictionary<TKey, TValue> //, ISerializable - it is already ISerializable
	{

		[DataMember]
		public Dictionary<TKey, TValue> dictionary;

		public SerializableDictionary() : base()
		{
			dictionary = new();
		}

		public new TValue this[TKey idx]
		{
			get => dictionary[idx];
			set => dictionary[idx] = value;
		}

		protected SerializableDictionary(SerializationInfo info, StreamingContext context)
		{
			dictionary = (Dictionary<TKey, TValue>)info.GetValue("dictionary", typeof(Dictionary<TKey, TValue>));
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("dictionary", dictionary);
		}

		public override string ToString()
		{
			string result = "";
			foreach (KeyValuePair<TKey, TValue> item in dictionary)
			{
				result += $"[{item.Key}]: {item.Value}\r\n";
			}
			return result;
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
			public void Put(int sum)
			{
				_sum += sum;
				if (Added != null)
					// Added?.Invoke(this, new AccountEventArgs("На рахунок поступило " + sum, sum));
					Added("На рахунок поступило " + sum + ", маємо " + CurrentSum);
				// это то же, что и 
				// Added.Invoke("На рахунок поступило " + sum + ", маємо " + CurrentSum);
			}
			public void WithdrawWithDelegate(int sum)
			{
				if (sum <= _sum)
				{
					_sum -= sum;
					if (del != null)
						del("Сума " + sum + " знята з рахунку, маємо " + CurrentSum);
				}
				else
				{
					if (del != null)
						del("Недостатньо грошей на рахунку, маємо " + CurrentSum);
				}
			}
			public void WithdrawWithEvent(int sum)
			{
				if (sum <= _sum)
				{
					_sum -= sum;
					if (Withdrowed != null)
						Withdrowed("Сума " + sum + " знята з рахунку, маємо " + CurrentSum);
				}
				else
				{
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

		// declaration of the delegate for reaction on the event adding an expired product
		// public delegate void ProductsExpired<ProductsExpiredArgs>(string product); // <T> is not needed if it is not used in parameters
		public delegate void ProductsExpired<ProductsExpiredArgs>(ProductsExpiredArgs args);

		// arguments type for the handler
		public class ProductsExpiredArgs : EventArgs
		{
			public string ExpiredProduct { get; }

			public ProductsExpiredArgs(string expiredProduct)
			{
				ExpiredProduct = expiredProduct;
			}
		}

		public class Storage
		{
			int s_count = 0;
			public event ProductsExpired<ProductsExpiredArgs> productsExpired; // event declaration
			public Storage() // створення порожнього складу
			{
				productsExpired += WriteProductsExpired;
			}

			public void RemoveProduct(string product) // delegate implementation #1: remove the given product from storage
			{
				if (!CheckProductsExpired(product))
				{
					s_count--;
					Console.WriteLine($"Product {product} removed");
				}
			}
			public void WriteProductsExpired(ProductsExpiredArgs args) // delegate implementation #2: list the product in the utilization file
			{
				Console.WriteLine($"Products has been expired, cannot remove {args.ExpiredProduct}");
			}

			public void AddProduct(string product) // a template for the future method of adding a product
			{
				s_count++;
				Console.WriteLine($"Product {product} added");
			}

			public bool CheckProductsExpired(string product) // checking the list and finding expired dairy products
			{
				if (s_count <= 0)
				{
					// productsExpired(product);  -- but better (no null exception):
					// productsExpired?.Invoke(product); - for (string product) instead of args in event declaration
					productsExpired?.Invoke(new ProductsExpiredArgs(product));
					return true;
				}
				return false;
			}
		}

		public class Shop
		{
			internal static void ShopOverflow(object sender)
			// event handler for the case if a cashier is overqueued and should be paused
			{
				if (sender is not Cashier)
					return;
				Console.WriteLine($"*** Shop is overflowed, cashier #{Cashier.Number}! ***");
			}
		}

		public class Cashier
		{
			public static int Number { get; set; }
			public static event ShopOverflowHandler ShopOverflow = Shop.ShopOverflow; // subscribe permanently here but can do it in constructor
			public delegate void ShopOverflowHandler(object sender);

			public Cashier()
			{
				Number++;
				// the handler is static but we could also make it public and use through "Shop owner" parameter here
				CheckOverflow();
			}

			public virtual void OnShopOverflow()
			{
				ShopOverflow?.Invoke(this);   // call the event
			}

			public void CheckOverflow()
			{
				if (Number > 2)
					OnShopOverflow();   // call the handler
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

			Storage storage = new();
			storage.AddProduct("Meat");      // Product Meat added
			storage.AddProduct("Fish");      // Product Fish added
			storage.AddProduct("Movie");     // Product Movie added
			storage.RemoveProduct("Meat");   // Product Meat removed
			storage.RemoveProduct("Fish");   // Product Fish removed
			storage.RemoveProduct("Movie");  // Product Movie removed
			storage.RemoveProduct("Table");  // Products has been expired, cannot remove Table

			Shop shop = new();
			Cashier cashier1 = new();
			Cashier cashier2 = new();
			Cashier cashier3 = new(); // ***Shop is overflowed, cashier #3! ***
		}

		public abstract class Shape
		{
			public abstract double Area();
		}


		public class CircleShape : Shape
		{
			public int Radius { get; set; }

			public CircleShape(int radius)
			{
				Radius = radius;
			}
			public override double Area()
			{
				return Radius * Radius * Math.PI;
			}
		}

		[DataContract]
		public class RectangleShape : Shape
		{
			[DataMember]
			public int Height { get; set; }
			[DataMember]
			public int Width { get; set; }

			public RectangleShape(int height, int width)
			{
				Height = height;
				Width = width;
			}

			[OnDeserialized]
			[OnDeserializing]
			[OnSerialized]
			[OnSerializing]
			public override double Area()
			{
				return Width * Height;
			}

			// this method violates SOLID (SRP principle) - use Drawer instead
			public void Draw()
			{
				Console.ForegroundColor = ConsoleColor.Red;
				for (int i = 0; i < Height; i++)
				{
					for (int j = 0; j < Width; j++)
						Console.Write("██");
					Console.WriteLine();
				}
				Console.ResetColor();
			}
		}
		public class AreaCalculator
		{
			public double TotalArea(Shape[] shapes)
			{
				double area = 0;
				foreach (var shape in shapes)
				{
					area += shape.Area();
				}
				return area;
			}
		}

		public class Drawer
		{
			public void DrawRectangle(RectangleShape rectangleShape)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				for (int i = 0; i < rectangleShape.Height; i++)
				{
					for (int j = 0; j < rectangleShape.Width; j++)
						Console.Write("██");
					Console.WriteLine();
				}
				Console.ResetColor();
			}
		}

		class Vehicle
		{
			public void startEngine()
			{
				// 	Default engine start functionality 
			}
			public void accelerate()
			{
				// Default acceleration functionality
			}
		}
		class Car : Vehicle
		{
			public void startEngine()
			{
				this.engageIgnition();
				base.startEngine();
			}
			private void engageIgnition()
			{ // Ignition procedure }
			}
		}

		#region Base Classes

		public class Component : IComponent
		{
			public string Brand
			{
				get;
				set;
			}

			public string Model
			{
				get;
				set;
			}
		}

		#endregion Base Classes

		#region Interfaces

		public interface IComponent
		{
			string Brand { get; set; }
			string Model { get; set; }
		}

		public interface IStarter : IComponent
		{
			IgnitionResult Start();
		}
		public interface IElectricStarter : IStarter
		{
			Battery Battery { get; set; }
		}

		public interface IPneumaticStarter : IStarter
		{
			AirCompressor Compressor { get; set; }
		}

		public interface IHydraulicStarter : IStarter
		{
			HydraulicPump Pump { get; set; }
		}

		#endregion Interfaces

		#region Starter Types (Services)

		public class ElectricStarter : Component, IElectricStarter
		{
			public Battery Battery
			{
				get;
				set;
			}

			public IgnitionResult Start()
			{
				//code here to initiate the electric starter
				return IgnitionResult.Success;
			}
		}

		public class PneumaticStarter : Component, IPneumaticStarter
		{
			public AirCompressor Compressor
			{
				get;
				set;
			}

			public IgnitionResult Start()
			{
				//code here to initiate the pneumatic starter
				return IgnitionResult.Success;
			}
		}

		public class HydraulicStarter : Component, IHydraulicStarter
		{
			public HydraulicPump Pump
			{
				get;
				set;
			}

			public IgnitionResult Start()
			{
				//code here to initiate the hydraulic starter
				return IgnitionResult.Success;
			}
		}
		#endregion Starter Types

		#region Starter Support Components

		public class Battery : Component
		{
			public bool IsCharged
			{
				get
				{
					/*we could write logic here to handle the
	                  validation of the battery's charge
	                  for now, we will just return true */
					return true;
				}
			}
		}

		public class AirCompressor : Component
		{
		}

		public class HydraulicPump : Component
		{
		}

		#endregion Starter Support Components

		#region Enums

		public enum IgnitionResult
		{
			Success,
			Failure
		}

		#endregion Enums

		class ElectricBus : Vehicle
		{
			public void accelerate()
			{
				this.increaseVoltage();
				this.connectIndividualEngines();
			}
			private void increaseVoltage() { } // Electric logic
			private void connectIndividualEngines() { } // Connection logic
		}

		public static void Лекция20()
		{
			RectangleShape rectangle = new(5, 10);
			Drawer drawer = new();
			drawer.DrawRectangle(rectangle);

			CircleShape circle = new(10);
			Shape[] shapes = { rectangle, circle };
			AreaCalculator areaCalc = new();
			Console.WriteLine("Total area of rectangle and circle is " + areaCalc.TotalArea(shapes));
			// Total area of rectangle and circle is 364,1592653589793
		}

		public class Base
		{
			public virtual void Print()
			{
				Console.WriteLine("Base");
			}
		}

		public class Child1 : Base
		{
			public override void Print()
			{
				Console.WriteLine("Child1");
			}
		}

		public class Child2 : Base
		{
			public new void Print()
			{
				Console.WriteLine("Child2");
			}
		}

		public class SimpleStack<T>
		{
			public T[] array;
			int size;
			int topPtr; // index of top

			public SimpleStack(int size = 1)
			{
				this.size = size;
				array = new T[size];
				topPtr = 0;
			}
			public void Push(T item) // Adds an item to the stack
			{
				if (topPtr < size)
				{
					array[topPtr] = item;
					topPtr++;
				}
				else
				{
					// increase the array size
					T[] arrayCopy = new T[size * 2];
					Array.Copy(array, arrayCopy, size);
					size = size * 2;
					array = arrayCopy;
					Push(item);
				}
			}
			public T Pop() // Returns 
			{
				topPtr--;
				if (topPtr >= 0)
				{
					return array[topPtr];
				}
				else
				{
					Console.WriteLine("Stack is empty");
					topPtr = 0;
					return default(T);
				}
			}
			public bool IsEmpty => topPtr == 0;
		}

		class Sample : IDisposable
		{
			public void Dispose()
			{
				Console.WriteLine("Dispose"); // this is called inside using {} because it contains try and finally blocks. catch is called after that
			}
		}

		class SampleClass
		{
			public int Value { get; set; }
		}

		struct SampleStruct
		{
			public int Value { get; set; }
		}

		static void IncrementValue(SampleClass sample)
		{
			sample.Value++;
		}

		static void IncrementValue(SampleStruct sample)
		{
			sample.Value++;
		}

		static void IncrementNumber(ref int number)
		{
			number++;
		}

		static void DecrementNumber(int number)
		{
			number--;
		}

		static void GetNumber(out int number)
		{
			number = 42;
		}

		public static T[] ReverseArray<T>(T[] array)
		{
			return array.Select(item => item).Reverse().ToArray();
			// или
			// Array.Reverse(array);
			// return array;
			// или
			// IEnumerable<T> list = from item in array select item;
			// return list.Reverse().ToArray();
		}

		static void PrintTest(string str)
		{
			Console.WriteLine("String");
		}

		static void PrintTest(int number)
		{
			Console.WriteLine("Int");
		}

		static void PrintTest(object obj)
		{
			Console.WriteLine("Object");
		}

		static void TestVar1()
		{
			var test = 42;
			// test = "string";  --- не удается неявно преобразовать тип string в int.
		}

		static void TestVar2()
		{
			var test = new object();
			// test.SomeProperty = 42; ---object не содержит определения SomeProperty
		}

		static void TestDynamic()
		{
			dynamic test = "some string";
			Console.WriteLine(test);
			test = 42;
			Console.WriteLine(test);
			test = 5.4;
			Console.WriteLine(test);
		}

		static void TestObject()
		{
			object test = new object();
			test = 42;
			Console.WriteLine(test);
		}

		public static void Test()
		{
			Child1 child1 = new Child1();
			Child2 child2 = new Child2();
			child1.Print();
			(child1 as Base).Print();
			child2.Print();
			(child2 as Base).Print();

			int number = 42;
			Action printNumber = () => Console.WriteLine(number);
			number++;
			printNumber(); // 43

			SimpleStack<int> simpleStack = new();
			simpleStack.Push(10);
			simpleStack.Push(20);
			Console.WriteLine(simpleStack.Pop()); // 20
			Console.WriteLine(simpleStack.Pop()); // 10
			Console.WriteLine(simpleStack.Pop()); // Stack is empty 0

			try
			{
				using (Sample sample = new Sample())
				{
					throw new Exception();
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Exception");
			}
			// Output:
			// Dispose // this is called inside using {} because it contains try and finally blocks. catch is called after that
			// Exception

			SampleClass sampleClass = new SampleClass();
			IncrementValue(sampleClass);
			Console.WriteLine(sampleClass.Value); // 1

			SampleStruct sampleStruct = new SampleStruct();
			IncrementValue(sampleStruct);
			Console.WriteLine(sampleStruct.Value); // 0

			int someNumber;
			GetNumber(out someNumber); // -> 42
			IncrementNumber(ref someNumber); // -> 43
			DecrementNumber(someNumber); // = 43
			Console.WriteLine(someNumber); // 43

			int[] array = { 1, 2, 3, 4, 5, 6, 7 };
			PrintList(ReverseArray(array)); // 7 6 5 4 3 2 1

			PrintTest(true); // Object
			PrintTest(42);   // Int
			PrintTest("test"); // String

			TestObject(); // 42
			TestDynamic(); // 42, some string, 5.4
		}

		// testing override and new modificators for methods
		// from https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/knowing-when-to-use-override-and-new-keywords

		class BaseClass
		{
			public virtual void Method1()
			{
				Console.WriteLine("Base - Method1");
			}
			public void Method2()
			{ // can be virtual, but is not overriden -> DerivedClass will use Base method
				Console.WriteLine("Base - Method2");
			}
		}

		class DerivedClass : BaseClass
		{
			public override void Method1()
			{
				Console.WriteLine("Derived - Method1");
			}

			// public void Method2() { // CS0108  "Program.DerivedClass.Method2()" скрывает наследуемый член "Program.BaseClass.Method2()".
			// Если скрытие было намеренным, используйте ключевое слово new.
			// (также можно переименовать один из методов, но это часто не является практичным)
			public new void Method2()
			{ // adding NEW suppresses the warning
				Console.WriteLine("Derived - Method2");
			}
		}

		public static void AfterTest()
		{
			// from https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/knowing-when-to-use-override-and-new-keywords

			BaseClass bc = new BaseClass(); // bc is of type BaseClass, and its value is of type BaseClass.
			DerivedClass dc = new DerivedClass(); // dc is of type DerivedClass, and its value is of type DerivedClass.
			BaseClass bcdc = new DerivedClass(); // bcdc is of type BaseClass, and its value is of type DerivedClass created by its constructor
												 // DerivedClass dcbc = new BaseClass(); // this cannot be done since BaseClass is not inherited from DerivedClass,
												 // we cannot just use its Base constructor
												 // without virtual in the BaseClass.Method1 and override in DerivedClass.Method1:
			bc.Method1(); // Base - Method1  (bc is BaseClass)
			dc.Method1(); // Base - Method1  (dc is of type DerivedClass but the method is not yet overriden)
			dc.Method2(); // Derived - Method2  (dc is of type DerivedClass and has Method2)
			bcdc.Method1(); // Base - Method1  (bcdc is of type BaseClass, it can directly access Method1)
			Console.WriteLine();

			// Because bc and bcdc have type BaseClass, they can only directly access Method1, unless you use casting.
			// Variable dc can access both Method1 and Method2. These relationships are shown in the following code.

			// After adding BaseClass.Method2 (not overriden and not new):
			bc.Method1(); // Base - Method1  (bc is BaseClass) ==
			bc.Method2(); // Base - Method2  (bc is BaseClass -- so Base method is used)
			dc.Method1(); // Base - Method1  (dc is of type DerivedClass but the method is not yet overriden) ==
			dc.Method2(); // Derived - Method2  (dc is of type DerivedClass and has Method2) ==
			bcdc.Method1(); // Base - Method1  (bcdc is of type BaseClass, it can directly access Method1) ==
			bcdc.Method2(); // Base - Method2  (bcdc is of type BaseClass, and Base.Method2 is not overriden ->
							// DerivedClass will use Base method.
							// Adding NEW suppresses the warning but does not changes output. This means: we understand and agree that
							// DerivedClass will use the Base method (based on its Type, not value). Only DC can use the new method.
			Console.WriteLine();

			// with virtual in the BaseClass.Method1 and override in DerivedClass.Method1:
			bc.Method1(); // Base - Method1  (bc is BaseClass) ==
			bc.Method2(); // Base - Method2  (bc is BaseClass -- so Base method is used) ==
			dc.Method1(); // Derived - Method1  (dc is of type DerivedClass so its overriden method is used)
			dc.Method2(); // Derived - Method2  (dc is of type DerivedClass and has Method2) ==
			bcdc.Method1(); // Derived - Method1  (bcdc is of type BaseClass, but there are overriden method in DerivedClass)
			bcdc.Method2(); // Base - Method2  (bcdc is of type BaseClass, and Base.Method2 is not overriden -> DerivedClass uses Base method) ==
			Console.WriteLine();

			// trying to cast the types
			// (bc as DerivedClass).Method1(); // System.NullReferenceException: (bc as DerivedClass) is null
			// (bc as DerivedClass).Method2(); // System.NullReferenceException: (bc as DerivedClass) is null
			(dc as BaseClass).Method1(); // "cast is unnecessary" warning -- this means that the behaviour of dc is still DerivedClass
										 // Derived - Method1  (the cast is BaseClass but dc is of type DerivedClass so its overriden method is used)
										 // the same as bcdc.Method1()
			(dc as BaseClass).Method2(); // Base - Method2  (the cast is BaseClass, and Base.Method2 is not overriden -> using Base method)

		}

		/// <summary>
		/// This method shows that the first char is not written if Esc is pressed. The bug report was sent to MS
		/// This does not work on every computer. Maybe depends on C# or .NET version, or the keyboard used.
		/// </summary>
		static public void ПропаданиеСимвола()
		{
			do
			{
				Console.Clear();
				Console.Write("This is a text test. Press Esc to test and Enter to exit");

				ConsoleKey c = Console.ReadKey().Key;
				if (c == ConsoleKey.Enter) break;
			}
			while (true);
		}
		/// <summary>
		/// Someone (MS?) said that Dictionary cannot be (de)serialized via DataContract. I show this is wrong
		/// </summary>
		static public void DictionarySerialization()
		{
			SerializableDictionary<int, string> serializableDictionary = new();
			serializableDictionary[1] = "String 1";
			serializableDictionary[2] = "String 2";
			serializableDictionary[3] = "String 3";
			serializableDictionary[4] = "String 4";
			Console.WriteLine("Original dictionary is:");
			Console.WriteLine(serializableDictionary);

			/*
			try
			{
				const string binarySerializationFileName = "../../../DictionarySerialization.bin";
				Console.WriteLine("\r\nBinary serialization of the dictionary. See " + binarySerializationFileName);
				IFormatter formatter = new BinaryFormatter();
				BinarySerialize(serializableDictionary, binarySerializationFileName, formatter);

				Console.WriteLine("\r\nBinary deserialization of the dictionary.");
				SerializableDictionary<int, string> deserializedDictionary =
							BinaryDeserialize<SerializableDictionary<int, string>>(binarySerializationFileName, formatter);
				Console.WriteLine("The deserialized dictionary is:");
				Console.WriteLine(deserializedDictionary);
			}
			catch (SerializationException serExc)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Serialization failed: " + serExc.Message);
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}

			/* An object cannot be ISerializable AND have DataContract attribute.
			   (but Dictionary is already ISerializable) */
			try
			{
				const string dataContractFileName = "../../../DictionaryDataContract.xml";
				Console.WriteLine("\r\nDataContract serialization of the dictionary. See " + dataContractFileName);
				WriteDataContractObject(serializableDictionary, dataContractFileName);

				Console.WriteLine("\r\nDataContract deserialization of the dictionary.");
				SerializableDictionary<int, string> deserializedDictionary =
					ReadDataContractObject<SerializableDictionary<int, string>>(dataContractFileName);
				Console.WriteLine("The deserialized dictionary is:");
				Console.WriteLine(deserializedDictionary);
			}
			catch (SerializationException serExc)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Serialization failed: " + serExc.Message);
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}
		}

		public static void BinarySerialize<T>(T whatToWrite, string fileName, IFormatter formatter)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Create))
				formatter.Serialize(stream, whatToWrite);
		}

		public static T BinaryDeserialize<T>(string fileName, IFormatter formatter)
		// Deserialization of an object using Binary or Soap formatter
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open))
				return (T)formatter.Deserialize(stream);
		}


		public static void WriteDataContractObject<T>(T whatToWrite, string fileName)
		// DataContract (XML) Serialization of an object with type T
		{
			using (FileStream stream = new(fileName, FileMode.Create))
			{
				DataContractSerializer serializer = new(typeof(T));
				serializer.WriteObject(stream, whatToWrite);
			}
		}

		public static T ReadDataContractObject<T>(string fileName)
		// DataContract (XML) Deserialization: returns an object of type T
		{
			using (FileStream fileReader = new FileStream(fileName, FileMode.Open))
			using (XmlDictionaryReader XMLreader =
				XmlDictionaryReader.CreateTextReader(fileReader, new XmlDictionaryReaderQuotas()))
			{
				DataContractSerializer serializer = new DataContractSerializer(typeof(T));
				return (T)serializer.ReadObject(XMLreader, true);
			}
		}

		#region Patterns
		public class Singleton
		{
			// Declaring the instance
			private static Singleton instance;
			// The constructor is private 
			private Singleton()
			{
			}
			// Initializing the instance on the first call,
			// this is not the recommended way, the code will 
			// fail in multi-threading environment
			// The Global Access Point to the instance
			public static Singleton Instance()
			{
				if (instance == null)
				{
					instance = new Singleton();
				}
				return instance;
			}
		}

		public class LazySingleton
		{
			private static readonly Lazy<LazySingleton> lazy =
			new Lazy<LazySingleton>(() => new LazySingleton());

			public string Name { get; private set; }

			private LazySingleton()
			{
				Name = System.Guid.NewGuid().ToString();
			}

			public static LazySingleton GetInstance()
			{
				LazySingleton check = lazy.Value;
				return check; // lazy.IsValueCreated = true, lazy.IsValueFaulted = false, check.Name != null
			}
		}

		abstract class AbstractFactory
		{
			public abstract AbstractProductA CreateProductA();
			public abstract AbstractProductB CreateProductB();
		}
		class ConcreteFactory1 : AbstractFactory
		{
			public override AbstractProductA CreateProductA()
			{
				return new ProductA1();
			}

			public override AbstractProductB CreateProductB()
			{
				return new ProductB1();
			}
		}
		class ConcreteFactory2 : AbstractFactory
		{
			public override AbstractProductA CreateProductA()
			{
				return new ProductA2();
			}

			public override AbstractProductB CreateProductB()
			{
				return new ProductB2();
			}
		}

		abstract class AbstractProductA
		{ }

		abstract class AbstractProductB
		{ }

		class ProductA1 : AbstractProductA
		{ }

		class ProductB1 : AbstractProductB
		{ }

		class ProductA2 : AbstractProductA
		{ }

		class ProductB2 : AbstractProductB
		{ }

		class Client
		{
			private AbstractProductA abstractProductA;
			private AbstractProductB abstractProductB;

			public Client(AbstractFactory factory)
			{
				abstractProductB = factory.CreateProductB();
				abstractProductA = factory.CreateProductA();
			}

			public AbstractProductA GetProductA() => abstractProductA;
			public AbstractProductB GetProductB() => abstractProductB;

			public void Run()
			{

			}
		}

		public class Laptop
		{
			public string MonitorResolution { get; set; }
			public string Processor { get; set; }
			public string Memory { get; set; }
			public string HDD { get; set; }
			public string Battery { get; set; }

			public override string ToString()
			{
				return MonitorResolution + ", " + Processor + ", " + Memory + ", " + HDD + ", " + Battery;
			}
		}

		abstract class LaptopBuilder
		{
			protected Laptop Laptop { get; private set; }
			public void CreateNewLaptop() { Laptop = new Laptop(); }
			// Метод, який повертає готовий ноутбук назовні
			public Laptop GetMyLaptop() { return Laptop; }
			// Кроки, необхідні щоб створити ноутбук 
			public abstract void SetMonitorResolution();
			public abstract void SetProcessor();
			public abstract void SetMemory();
			public abstract void SetHDD();
			public abstract void SetBattery();
		}

		// Таким будівельником може бути працівник, що
		// спеціалізується у складанні «геймерських» ноутів 
		class GamingLaptopBuilder : LaptopBuilder
		{
			public override void SetMonitorResolution()
			{
				Laptop.MonitorResolution = "1900X1200";
			}
			public override void SetProcessor()
			{
				Laptop.Processor = "Core 2 Duo, 3.2 GHz";
			}
			public override void SetMemory()
			{
				Laptop.Memory = "6144 Mb";
			}
			public override void SetHDD()
			{
				Laptop.HDD = "500 Gb";
			}
			public override void SetBattery()
			{
				Laptop.Battery = "6 lbs";
			}
		}

		// А ось інший «збирач» ноутів
		class TripLaptopBuilder : LaptopBuilder
		{
			public override void SetMonitorResolution()
			{
				Laptop.MonitorResolution = "1200X800";
			}
			public override void SetBattery()
			{
				//.. і так далі... 
			}

			public override void SetHDD()
			{
				//.. і так далі... 
			}

			public override void SetMemory()
			{
				//.. і так далі... 
			}

			public override void SetProcessor()
			{
				//.. і так далі... 
			}
		}

		// Ваша система може мати багато конкретних будівельників

		class BuyLaptop
		{
			private LaptopBuilder _laptopBuilder;
			public void SetLaptopBuilder(LaptopBuilder lBuilder)
			{
				_laptopBuilder = lBuilder;
			}
			// Змушує будівельника повернути цілий ноутбук
			public Laptop GetLaptop()
			{
				return _laptopBuilder.GetMyLaptop();
			}
			// Змушує будівельника додавати деталі 
			public void ConstructLaptop()
			{
				_laptopBuilder.CreateNewLaptop();
				_laptopBuilder.SetMonitorResolution();
				_laptopBuilder.SetProcessor();
				_laptopBuilder.SetMemory();
				_laptopBuilder.SetHDD();
				_laptopBuilder.SetBattery();
			}
		}

		// Factory Method

		// абстрактный клас будівельної компанії
		abstract class Developer
		{
			public string Name { get; set; }

			public Developer(string n)
			{
				Name = n;
			}
			// фабричний метод
			abstract public House Create();
		}

		// будує панельні будинки
		class PanelDeveloper : Developer
		{
			public PanelDeveloper(string n) : base(n)
			{ }

			public override House Create()
			{
				return new PanelHouse();
			}
		}
		// будує дерев’яні будинки
		class WoodDeveloper : Developer
		{
			public WoodDeveloper(string n) : base(n)
			{ }

			public override House Create()
			{
				return new WoodHouse();
			}
		}

		abstract class House
		{ }

		class PanelHouse : House
		{
			public PanelHouse()
			{
				Console.WriteLine("Панельний дім побудований");
			}
		}
		class WoodHouse : House
		{
			public WoodHouse()
			{
				Console.WriteLine("Дерев’яний дім побудований");
			}
		}

		// Prototype

		interface IFigure
		{
			IFigure Clone();
			void GetInfo();
		}

		class Rectangle : IFigure
		{
			int width;
			int height;
			public Rectangle(int w, int h)
			{
				width = w;
				height = h;
			}

			public IFigure Clone()
			{
				// return (Rectangle)this.MemberwiseClone();
				return new Rectangle(this.width, this.height);
			}
			public void GetInfo()
			{
				Console.WriteLine("Прямокутник з довжиною {0} і шириною {1}", height, width);
			}
		}

		class Circle : IFigure
		{
			int radius;
			public Circle(int r)
			{
				radius = r;
			}

			public IFigure Clone()
			{
				// return (Circle)this.MemberwiseClone();
				return new Circle(this.radius);
			}
			public void GetInfo()
			{
				Console.WriteLine("Круг з радіусом {0}", radius);
			}
		}

		// класс, к которому надо адаптировать другой класс   
		class Target
		{
			public virtual void Request() { Console.WriteLine("Target Request"); }
		}

		// Адаптер
		class Adapter : Target
		{
			private Adaptee adaptee = new Adaptee(); // Adapter knows about Adaptee and its method

			public override void Request()  // the method overrides the base method (that should be VIRTUAL)
											// or we can make a "new" Request method and call Adapter.Request
			{
				adaptee.SpecificRequest();   // and calls it when the the Target Request method is called
			}

			public void AdaptedRequest() // if the base method is not virtual we can create new with new parameters/view
			{
				adaptee.SpecificRequest(); // different parameters or in different order could be here
			}
		}

		// Адаптируемый класс
		class Adaptee
		{
			public void SpecificRequest() { Console.WriteLine("Specific Request"); }
		}

		// Decorator

		public interface ICake
		{
			public string ingredients();
			public double price();
		}
		public abstract class CakeDecorator : ICake
		{
			protected ICake cake;
			public CakeDecorator(ICake cake)
			{
				this.cake = cake;
			}
			public abstract string ingredients();
			public abstract double price();

		}
		public class SimpleCake : ICake
		{
			public string ingredients()
			{
				return "Простий торт";
			}
			public double price()
			{
				return 12.5;
			}
		}
		public class WithWhippedCream : CakeDecorator
		{
			public WithWhippedCream(ICake cake) : base(cake) { }

			public override string ingredients()
			{
				return this.cake.ingredients() + " з вершками";
			}
			public override double price()
			{
				return this.cake.price() + 2.5;
			}
		}
		public class WithSprinkles : CakeDecorator
		{
			public WithSprinkles(ICake cake) : base(cake) { }

			public override string ingredients()
			{
				return this.cake.ingredients() + " з присипкою";
			}
			public override double price()
			{
				return this.cake.price() + 1.25;
			}
		}

		// Facade

		class Compiler
		{
			public void Compile()
			{
				Console.WriteLine("Компіляція застосунку");
			}
		}
		class TextEditor
		{
			public void CreateCode()
			{
				Console.WriteLine("Створення коду");

			}
			public void Save()
			{
				Console.WriteLine("Збереження коду");

			}
		}
		class CLR
		{
			public void Execute()
			{
				Console.WriteLine("Виконання додатку");

			}
			public void Finish()
			{
				Console.WriteLine("Завершення роботи додатку");
			}
		}
		class VisualStudioFacade
		{
			TextEditor textEditor;
			Compiler compiler;
			CLR clr;
			public VisualStudioFacade(TextEditor te, Compiler compil, CLR clr)
			{
				this.textEditor = te;
				this.compiler = compil;
				this.clr = clr;
			}
			public void Start()
			{
				textEditor.CreateCode();
				textEditor.Save();
				compiler.Compile();
				clr.Execute();
			}
			public void Stop()
			{
				clr.Finish();
			}
		}

		class Programmer
		{
			public void CreateApplication(VisualStudioFacade facade)
			{
				facade.Start();
				facade.Stop();
			}
		}

		// Proxy

		abstract class Subject
		{
			public abstract void Request();
		}

		class RealSubject : Subject
		{
			public override void Request() { Console.WriteLine("RealSubject Request"); }
		}
		class Proxy : Subject
		{
			RealSubject realSubject;
			public override void Request()
			{
				Console.WriteLine("Proxy Request start");
				if (realSubject == null)
					realSubject = new RealSubject();
				realSubject.Request();
				Console.WriteLine("Proxy Request end");
			}
		}

		class Page
		{
			public int Id { get; set; }
			public int Number { get; set; }
			public string Text { get; set; }

			public Page(int id, int num, string text)
			{
				Id = id;
				Number = num;
				Text = text;
			}
		}
		class PageContext // : DbContext
		{
			// public DbSet<Page> Pages { get; set; }
			public List<Page> Pages { get; set; }
			public PageContext(List<Page> pages)
			{
				Pages = pages;
			}
			public void Dispose() { }
		}
		interface IBook : IDisposable
		{
			Page GetPage(int number);
		}

		class BookStore : IBook
		{
			PageContext db;
			public BookStore()
			{
				List<Page> pages = new();
				pages.Add(new Page(1001, 1, "Page 1"));
				pages.Add(new Page(1002, 2, "Page 2"));
				pages.Add(new Page(1003, 3, "Page 3"));
				db = new PageContext(pages);
			}
			public Page GetPage(int number)
			{
				return db.Pages.FirstOrDefault(p => p.Number == number);
			}

			public void Dispose()
			{
				db.Dispose();
			}
		}

		class BookStoreProxy : IBook
		{
			List<Page> pages;
			BookStore bookStore;
			public BookStoreProxy()
			{
				pages = new List<Page>();
			}
			public Page GetPage(int number)
			{
				Page page = pages.FirstOrDefault(p => p.Number == number);
				if (page == null)
				{
					if (bookStore == null)
						bookStore = new BookStore();
					page = bookStore.GetPage(number);
					pages.Add(page);
					Console.WriteLine("Page " + number + " added to proxy list");
				}
				else
					Console.WriteLine("Page " + number + " taken from proxy list");
				return page;
			}

			public void Dispose()
			{
				if (bookStore != null)
					bookStore.Dispose();
			}
		}

		// Visitor

		interface IVisitor
		{
			void VisitPersonAcc(Person acc);
			void VisitCompanyAc(Company acc);
		}

		// серіалізатор в HTML
		class HtmlVisitor : IVisitor
		{
			public void VisitPersonAcc(Person acc)
			{
				string result = "<table><tr><td>Властивість<td><td>Значення</td></tr>";
				result += "<tr><td>Name<td><td>" + acc.Name + "</td></tr>";
				result += "<tr><td>Number<td><td>" + acc.Number + "</td></tr></table>";
				Console.WriteLine(result);
			}

			public void VisitCompanyAc(Company acc)
			{
				string result = "<table><tr><td>Властивість<td><td>Значення</td></tr>";
				result += "<tr><td>Name<td><td>" + acc.Name + "</td></tr>";
				result += "<tr><td>RegNumber<td><td>" + acc.RegNumber + "</td></tr>";
				result += "<tr><td>Number<td><td>" + acc.Number + "</td></tr></table>";
				Console.WriteLine(result);
			}
		}

		// серіалізатор в XML
		class XmlVisitor : IVisitor
		{
			public void VisitPersonAcc(Person acc)
			{
				string result = "<Person><Name>" + acc.Name + "</Name>" +
				"<Number>" + acc.Number + "</Number></Person>";
				Console.WriteLine(result);
			}

			public void VisitCompanyAc(Company acc)
			{
				string result = "<Company><Name>" + acc.Name + "</Name>" +
				"<RegNumber>" + acc.RegNumber + "</RegNumber>" +
				"<Number>" + acc.Number + "</Number></Company>";
				Console.WriteLine(result);
			}
		}

		class Bank
		{
			List<IAccount> accounts = new List<IAccount>();
			public void Add(IAccount acc)
			{
				accounts.Add(acc);
			}
			public void Remove(IAccount acc)
			{
				accounts.Remove(acc);
			}
			public void Accept(IVisitor visitor)
			{
				foreach (IAccount acc in accounts)
					acc.Accept(visitor);
			}
		}

		interface IAccount
		{
			void Accept(IVisitor visitor);
		}

		class Person : IAccount
		{
			public string Name { get; set; }
			public string Number { get; set; }

			public void Accept(IVisitor visitor)
			{
				visitor.VisitPersonAcc(this);
			}
		}

		class Company : IAccount
		{
			public string Name { get; set; }
			public string RegNumber { get; set; }
			public string Number { get; set; }

			public void Accept(IVisitor visitor)
			{
				visitor.VisitCompanyAc(this);
			}
		}

		// Mediator

		abstract class Mediator
		{
			public abstract void Send(string msg, Colleague colleague);
		}

		abstract class Colleague
		{
			protected Mediator mediator;

			public Colleague(Mediator mediator)
			{
				this.mediator = mediator;
			}

			public virtual void Send(string message)
			{
				mediator.Send(message, this);
			}
			public abstract void Notify(string message);
		}

		// класс замовника
		class CustomerColleague : Colleague
		{
			public CustomerColleague(Mediator mediator)
			: base(mediator)
			{ }

			public override void Notify(string message)
			{
				Console.WriteLine("Повідомлення замовнику: " + message);
			}
		}
		// клас програміста
		class ProgrammerColleague : Colleague
		{
			public ProgrammerColleague(Mediator mediator)
			: base(mediator)
			{ }

			public override void Notify(string message)
			{
				Console.WriteLine("Повідомлення програмісту: " + message);
			}
		}
		// клас тестера
		class TesterColleague : Colleague
		{
			public TesterColleague(Mediator mediator)
			: base(mediator)
			{ }

			public override void Notify(string message)
			{
				Console.WriteLine("Повідомлення тестеру: " + message);
			}
		}

		class ManagerMediator : Mediator
		{
			public Colleague Customer { get; set; }
			public Colleague Programmer { get; set; }
			public Colleague Tester { get; set; }
			public override void Send(string msg, Colleague colleague)
			{
				// якщо відпрвник - замовник, значить є нове змовлення
				// відправляємо повідомлення програмісту – виконати замовлення 
				if (Customer == colleague)
					Programmer.Notify(msg);
				// якщо відправник- програміст, то можно приступати до тестування 
				// відправляємо повідомлення тестеру
				else if (Programmer == colleague)
					Tester.Notify(msg);
				// якщо відправник - тестер,то продукт готовий
				// відправляємо повідомлення замовнику.
				else if (Tester == colleague)
					Customer.Notify(msg);
			}
		}

		// Strategy

		interface IMovable
		{
			void Move();
		}

		class PetrolMove : IMovable
		{
			public void Move()
			{
				Console.WriteLine("Переміщення на бензині");
			}
		}

		class ElectricMove : IMovable
		{
			public void Move()
			{
				Console.WriteLine("Переміщення на електриці");
			}
		}
		class Auto
		{
			protected int passengers; // кількість пасажирів
			protected string model; // модель автомобіля
			public IMovable Movable { private get; set; } // тип переміщення

			public Auto(int num, string model, IMovable mov)
			{
				this.passengers = num;
				this.model = model;
				Movable = mov;
			}
			public void Move()
			{
				Movable.Move();
			}
		}

		// Chain of Responsibility

		class Receiver
		{
			// банківські переведення
			public bool BankTransfer { get; set; }
			// грошові переведення- WesternUnion, Unistream
			public bool MoneyTransfer { get; set; }
			// переведення через PayPal
			public bool PayPalTransfer { get; set; }
			public Receiver(bool bt, bool mt, bool ppt)
			{
				BankTransfer = bt;
				MoneyTransfer = mt;
				PayPalTransfer = ppt;
			}
		}
		abstract class PaymentHandler
		{
			public PaymentHandler Successor { get; set; }
			public abstract void Handle(Receiver receiver);
		}

		class BankPaymentHandler : PaymentHandler
		{
			public override void Handle(Receiver receiver)
			{
				if (receiver.BankTransfer == true)
					Console.WriteLine("Виконуємо банківське переведення");
				else if (Successor != null)
					Successor.Handle(receiver);
			}
		}

		class PayPalPaymentHandler : PaymentHandler
		{
			public override void Handle(Receiver receiver)
			{
				if (receiver.PayPalTransfer == true)
					Console.WriteLine("Виконуємо переведення через PayPal");
				else if (Successor != null)
					Successor.Handle(receiver);
			}
		}
		// переведення з допомою грошових систем 
		class MoneyPaymentHandler : PaymentHandler
		{
			public override void Handle(Receiver receiver)
			{
				if (receiver.MoneyTransfer == true)
					Console.WriteLine("Виконуємо переведення через системи грошових переводів");
				else if (Successor != null)
					Successor.Handle(receiver);
			}
		}

		// NEW (BEHAVIOURAL)
		// Observer (Publisher - Subscriber)
		interface IObservable
		{
			void RegisterObserver(IObserver o);
			void RemoveObserver(IObserver o);
			void NotifyObservers();
		}
		class Stock : IObservable
		{
			StockInfo sInfo; // информация о торгах
			private List<IObserver> observers;
			public Stock()
			{
				observers = new List<IObserver>();
				sInfo = new StockInfo();
			}
			public void RegisterObserver(IObserver o)
			{
				observers.Add(o);
			}

			public void RemoveObserver(IObserver o)
			{
				observers.Remove(o);
			}

			public void NotifyObservers()
			{
				foreach (IObserver observer in observers)
					observer.Update(sInfo);
			}
			public void Market()
			{
				Random rnd = new Random();
				sInfo.USD = rnd.Next(20, 40);
				sInfo.Euro = rnd.Next(30, 50);
				NotifyObservers();
			}
		}
		class StockInfo
		{
			public int USD { get; set; }
			public int Euro { get; set; }
		}
		interface IObserver
		{
			void Update(Object ob);
		}
		class BrokerObserver : IObserver
		{
			public string Name { get; set; }
			IObservable stock;
			public BrokerObserver(string name, IObservable obs)
			{
				this.Name = name;
				stock = obs;
				stock.RegisterObserver(this);
			}
			public void Update(object ob)
			{
				StockInfo sInfo = (StockInfo)ob;

				if (sInfo.USD > 30)
					Console.WriteLine("Брокер {0} продает доллары;  Курс доллара: {1}", this.Name, sInfo.USD);
				else
					Console.WriteLine("Брокер {0} покупает доллары;  Курс доллара: {1}", this.Name, sInfo.USD);
			}
			public void StopTrade()
			{
				stock.RemoveObserver(this);
				stock = null;
			}
		}
		class BankObserver : IObserver
		{
			public string Name { get; set; }
			IObservable stock;
			public BankObserver(string name, IObservable obs)
			{
				this.Name = name;
				stock = obs;
				stock.RegisterObserver(this);
			}
			public void Update(object ob)
			{
				StockInfo sInfo = (StockInfo)ob;

				if (sInfo.Euro > 40)
					Console.WriteLine("Банк {0} продает евро;  Курс евро: {1}", this.Name, sInfo.Euro);
				else
					Console.WriteLine("Банк {0} покупает евро;  Курс евро: {1}", this.Name, sInfo.Euro);
			}
		}

		// Template Method

		abstract class Education
		{
			public void Learn()
			{
				Enter();
				Study();
				PassExams();
				GetDocument();
			}
			public abstract void Enter();
			public abstract void Study();
			public virtual void PassExams()
			{
				Console.WriteLine("Сдаем выпускные экзамены");
			}
			public abstract void GetDocument();
		}

		class School : Education
		{
			// public new void Learn() { Console.WriteLine("Не хочу учиться"); }

			public override void Enter()
			{
				Console.WriteLine("Идем в первый класс");
			}

			public override void Study()
			{
				Console.WriteLine("Посещаем уроки, делаем домашние задания");
			}

			public override void GetDocument()
			{
				Console.WriteLine("Получаем аттестат о среднем образовании");
			}
		}

		class University : Education
		{
			public override void Enter()
			{
				Console.WriteLine("Сдаем вступительные экзамены и поступаем в ВУЗ");
			}

			public override void Study()
			{
				Console.WriteLine("Посещаем лекции");
				Console.WriteLine("Проходим практику");
			}

			public override void PassExams()
			{
				Console.WriteLine("Сдаем экзамен по специальности");
			}

			public override void GetDocument()
			{
				Console.WriteLine("Получаем диплом о высшем образовании");
			}
		}


		// Iterator

		class Reader
		{
			public void SeeBooks(Library library)
			{
				IBookIterator iterator = library.CreateNumerator();
				while (iterator.HasNext())
				{
					Book book = iterator.Next();
					Console.WriteLine(book.Name);
				}
			}
		}

		interface IBookIterator
		{
			bool HasNext();
			Book Next();
		}
		interface IBookNumerable
		{
			IBookIterator CreateNumerator();
			int Count { get; }
			Book this[int index] { get; }
		}
		class Book
		{
			public string Name { get; set; }
		}

		class Library : IBookNumerable
		{
			private Book[] books;
			public Library()
			{
				books = new Book[]
				{
			new Book{Name="Война и мир"},
			new Book {Name="Отцы и дети"},
			new Book {Name="Вишневый сад"}
				};
			}
			public int Count
			{
				get { return books.Length; }
			}

			public Book this[int index]
			{
				get { return books[index]; }
			}
			public IBookIterator CreateNumerator()
			{
				return new LibraryNumerator(this);
			}
		}
		class LibraryNumerator : IBookIterator
		{
			IBookNumerable aggregate;
			int index = 0;
			public LibraryNumerator(IBookNumerable a)
			{
				aggregate = a;
			}
			public bool HasNext()
			{
				return index < aggregate.Count;
			}

			public Book Next()
			{
				return aggregate[index++];
			}
		}

		// State

		class Water
		{
			public IWaterState State { get; set; }
			public Water(IWaterState ws)
			{
				State = ws;
			}

			public void Heat()
			{
				State.Heat(this);
			}
			public void Frost()
			{
				State.Frost(this);
			}
		}

		interface IWaterState
		{
			void Heat(Water water);
			void Frost(Water water);
		}

		class SolidWaterState : IWaterState
		{
			public void Heat(Water water)
			{
				Console.WriteLine("Превращаем лед в жидкость");
				water.State = new LiquidWaterState();
			}

			public void Frost(Water water)
			{
				Console.WriteLine("Продолжаем заморозку льда");
			}
		}
		class LiquidWaterState : IWaterState
		{
			public void Heat(Water water)
			{
				Console.WriteLine("Превращаем жидкость в пар");
				water.State = new GasWaterState();
			}

			public void Frost(Water water)
			{
				Console.WriteLine("Превращаем жидкость в лед");
				water.State = new SolidWaterState();
			}
		}
		class GasWaterState : IWaterState
		{
			public void Heat(Water water)
			{
				Console.WriteLine("Повышаем температуру водяного пара");
			}

			public void Frost(Water water)
			{
				Console.WriteLine("Превращаем водяной пар в жидкость");
				water.State = new LiquidWaterState();
			}
		}

		// Memento

		// Originator
		class Hero
		{
			private int patrons = 10; // кол-во патронов
			private int lives = 5; // кол-во жизней

			public void Shoot()
			{
				if (patrons > 0)
				{
					patrons--;
					Console.WriteLine("Производим выстрел. Осталось {0} патронов", patrons);
				}
				else
					Console.WriteLine("Патронов больше нет");
			}
			// сохранение состояния
			public HeroMemento SaveState()
			{
				Console.WriteLine("Сохранение игры. Параметры: {0} патронов, {1} жизней", patrons, lives);
				return new HeroMemento(patrons, lives);
			}

			// восстановление состояния
			public void RestoreState(HeroMemento memento)
			{
				this.patrons = memento.Patrons;
				this.lives = memento.Lives;
				Console.WriteLine("Восстановление игры. Параметры: {0} патронов, {1} жизней", patrons, lives);
			}
		}
		// Memento
		class HeroMemento
		{
			public int Patrons { get; private set; }
			public int Lives { get; private set; }

			public HeroMemento(int patrons, int lives)
			{
				this.Patrons = patrons;
				this.Lives = lives;
			}
		}

		// Caretaker
		class GameHistory
		{
			public Stack<HeroMemento> History { get; private set; }
			public GameHistory()
			{
				History = new Stack<HeroMemento>();
			}
		}

		// NEW STRUCTURAL

		// Composite

		abstract class Component2
		{
			protected string name;

			public Component2(string name)
			{
				this.name = name;
			}

			public virtual void Add(Component2 component) { }

			public virtual void Remove(Component2 component) { }

			public virtual void Print()
			{
				Console.WriteLine(name);
			}
		}
		class Directory : Component2
		{
			private List<Component2> components = new();

			public Directory(string name)
				: base(name)
			{
			}

			public override void Add(Component2 component)
			{
				components.Add(component);
			}

			public override void Remove(Component2 component)
			{
				components.Remove(component);
			}

			public override void Print()
			{
				Console.WriteLine("Узел " + name);
				Console.WriteLine("Подузлы:");
				for (int i = 0; i < components.Count; i++)
				{
					Console.Write("\t");
					components[i].Print();
				}
			}
		}

		class FileComponent : Component2
		{
			public FileComponent(string name)
					: base(name)
			{ }
		}

		// Bridge

		interface ILanguage // Implementor
		{
			void Build();
			void Execute();
		}

		class CPPLanguage : ILanguage
		{
			public void Build()
			{
				Console.WriteLine("С помощью компилятора C++ компилируем программу в бинарный код");
			}

			public void Execute()
			{
				Console.WriteLine("Запускаем исполняемый файл программы");
			}
		}

		class CSharpLanguage : ILanguage
		{
			public void Build()
			{
				Console.WriteLine("С помощью компилятора Roslyn компилируем исходный код в файл exe");
			}

			public void Execute()
			{
				Console.WriteLine("JIT компилирует программу в бинарный код");
				Console.WriteLine("CLR выполняет скомпилированный бинарный код");
			}
		}

		abstract class Programmer2 // Abstraction
		{
			protected ILanguage language;
			public ILanguage Language
			{
				set { language = value; }
			}
			public Programmer2(ILanguage lang)
			{
				language = lang;
			}
			public virtual void DoWork()
			{
				language.Build();
				language.Execute();
			}
			public abstract void EarnMoney();
		}

		class FreelanceProgrammer : Programmer2
		{
			public FreelanceProgrammer(ILanguage lang) : base(lang)
			{
			}
			public override void EarnMoney()
			{
				Console.WriteLine("Получаем оплату за выполненный заказ");
			}
		}
		class CorporateProgrammer : Programmer2
		{
			public CorporateProgrammer(ILanguage lang)
				: base(lang)
			{
			}
			public override void EarnMoney()
			{
				Console.WriteLine("Получаем в конце месяца зарплату");
			}
		}

		// Flyweight

		abstract class House2
		{
			protected int stages; // количество этажей

			public abstract void Build(double longitude, double latitude);
		}

		class PanelHouse2 : House2
		{
			public PanelHouse2()
			{
				stages = 16;
			}

			public override void Build(double longitude, double latitude)
			{
				Console.WriteLine("Построен панельный дом из {0} этажей; координаты: {1:n2} широты и {2:n2} долготы",
					stages, latitude, longitude);
			}
		}
		class BrickHouse : House2
		{
			public BrickHouse()
			{
				stages = 5;
			}

			public override void Build(double longitude, double latitude)
			{
				Console.WriteLine("Построен кирпичный дом из {0} этажей; координаты: {1:n2} широты и {2:n2} долготы",
					stages, latitude, longitude);
			}
		}

		class HouseFactory
		{
			Dictionary<string, House2> houses = new Dictionary<string, House2>();
			public HouseFactory()
			{
				houses.Add("Panel", new PanelHouse2());
				houses.Add("Brick", new BrickHouse());
			}

			public House2 GetHouse(string key)
			{
				if (houses.ContainsKey(key))
					return houses[key];
				else
					return null;
			}
		}

		// ADDITIONAL: Fluent Builder

		public class User
		{
			public string Name { get; set; }        // имя
			public string Company { get; set; }     // компания
			public int Age { get; set; }            // возраст
			public bool IsMarried { get; set; }      // женат/замужем

			public static UserBuilder CreateBuilder()
			{
				return new UserBuilder();
			}
		}

		public class UserBuilder
		{
			private User user;
			public UserBuilder()
			{
				user = new User();
			}
			public UserBuilder SetName(string name)
			{
				user.Name = name;
				return this;
			}
			public UserBuilder SetCompany(string company)
			{
				user.Company = company;
				return this;
			}
			public UserBuilder SetAge(int age)
			{
				user.Age = age > 0 ? age : 0;
				return this;
			}
			public UserBuilder IsMarried
			{
				get
				{
					user.IsMarried = true;
					return this;
				}
			}
			public User Build()
			{
				return user;
			}

			public static implicit operator User(UserBuilder builder) // operator of casting to type User
			{
				return builder.user;
			}
		}

		/// <summary>
		/// Studying different design patterns
		/// </summary>
		public static void Patterns()
		{
			// CREATIONAL

			// Singleton

			Singleton firstInstance = Singleton.Instance();
			Singleton secondInstance = Singleton.Instance();
			if (firstInstance == secondInstance)
				Console.WriteLine("Great, We are same instances");
			else
				Console.WriteLine("Singleton failed me");

			LazySingleton lazySingleton = LazySingleton.GetInstance();
			LazySingleton secondLazy = LazySingleton.GetInstance();
			if (lazySingleton == secondLazy)
				Console.WriteLine("Great, We are same lazy instances, GUID: " + lazySingleton.Name);
			else
				Console.WriteLine("LazySingleton failed me");

			// Abstract Factory

			ConcreteFactory1 factory1 = new();
			Client client = new(factory1);
			AbstractProductA productA = client.GetProductA();
			AbstractProductB productB = client.GetProductB();
			Console.WriteLine("Factory1 has created products with types: " + productA.GetType() + ", " + productB.GetType());

			ConcreteFactory2 factory2 = new();
			client = new(factory2);
			productA = client.GetProductA();
			productB = client.GetProductB();
			Console.WriteLine("Factory2 has created products with types: " + productA.GetType() + ", " + productB.GetType());

			// Builder

			var tripBuilder = new TripLaptopBuilder();
			var gamingBuilder = new GamingLaptopBuilder();
			var shopForYou = new BuyLaptop(); //Директор
											  //Покупець каже, що хоче грати ігри
			shopForYou.SetLaptopBuilder(gamingBuilder);
			shopForYou.ConstructLaptop();
			// Ну то нехай бере що хоче! 
			Laptop laptop = shopForYou.GetLaptop();
			Console.WriteLine(laptop.ToString());
			// Вивід: 1900X1200, Core 2 Duo, 3.2 GHz, 6144 Mb, 500 Gb, 6 lbs

			// Factory Method

			Developer dev = new PanelDeveloper("ТЗОВ ЦеглаБуд");
			House house2 = dev.Create(); // Панельний дім побудований

			dev = new WoodDeveloper("Приватний забудовник");
			House house = dev.Create(); // Дерев’яний дім побудований

			// Prototype 

			IFigure figure = new Rectangle(30, 40);
			IFigure clonedFigure = figure.Clone();
			figure.GetInfo(); // Прямокутник з довжиною 40 і шириною 30
			clonedFigure.GetInfo(); // Прямокутник з довжиною 40 і шириною 30

			figure = new Circle(30);
			clonedFigure = figure.Clone();
			figure.GetInfo(); // Круг з радіусом 30
			clonedFigure.GetInfo(); // Круг з радіусом 30

			// STRUCTURAL

			// Adapter (class)

			Target target = new();
			target.Request(); // Target Request
			target = new Adapter();
			target.Request(); // Specific Request

			// Adapter (object)

			Adapter target2 = new();  // Adapter has the Adaptee inside
			target2.AdaptedRequest(); // Specific Request

			// Decorator

			ICake cake = new SimpleCake(); // Простий торт 
			ICake cake1 = new WithWhippedCream(cake); // з вершками 
			ICake cake2 = new WithSprinkles(cake1); // з присипкою
			Console.WriteLine(cake2.ingredients()); // Простий торт з вершками з присипкою
			Console.WriteLine(cake2.price()); // 16.25

			// або можна ланцюжок
			ICake cake4 =
				new WithSprinkles(    // з присипкою
				new WithWhippedCream( // з вершками 
				new SimpleCake()));   // Простий торт
			Console.WriteLine(cake4.ingredients()); // Простий торт з вершками з присипкою
			Console.WriteLine(cake4.price()); // 16.25

			// Facade

			TextEditor textEditor = new TextEditor();
			Compiler compiler = new Compiler();
			CLR clr = new CLR();

			VisualStudioFacade ide = new VisualStudioFacade(textEditor, compiler, clr);

			Programmer programmer = new Programmer();
			programmer.CreateApplication(ide);
			// Output:
			// Створення коду
			// Збереження коду
			// Компіляція застосунку
			// Виконання додатку
			// Завершення роботи додатку

			// Proxy

			Subject subject = new Proxy();
			subject.Request();
			// Output:
			// Proxy Request start
			// RealSubject Request
			// Proxy Request end

			using (IBook book = new BookStoreProxy())
			{
				// читаем первую страницу
				Page page = book.GetPage(1);
				Console.WriteLine(page.Text);
				// Page 1 added to proxy list
				// Page 1

				// читаем вторую страницу
				page = book.GetPage(2);
				Console.WriteLine(page.Text);
				// Page 2 added to proxy list
				// Page 2

				// возвращаемся на первую страницу    
				page = book.GetPage(1);
				Console.WriteLine(page.Text);
				// Page 1 taken from proxy list
				// Page 1
			}

			// BEHAVIOURAL

			// Visitor

			var structure = new Bank();
			structure.Add(new Person
			{
				Name = "Дяконюк Лілія",
				Number = "+380002184931"
			});
			structure.Add(new Company { Name = "Microsoft", RegNumber = "MS32141324", Number = "+13424131445" });
			structure.Accept(new HtmlVisitor());
			// <table><tr><td>Властивість<td><td>Значення</td></tr><tr><td>Name<td><td>Дяконюк Лілія</td></tr><tr><td>Number<td><td>+380002184931</td></tr></table>
			// <table><tr><td>Властивість<td><td>Значення</td></tr><tr><td>Name<td><td>Microsoft</td></tr><tr><td>RegNumber<td><td>MS32141324</td></tr><tr><td>Number<td><td>+13424131445</td></tr></table>
			structure.Accept(new XmlVisitor());
			// <Person><Name>Дяконюк Лілія</Name><Number>+380002184931</Number></Person>
			// <Company><Name>Microsoft</Name><RegNumber>MS32141324</RegNumber><Number>+13424131445</Number></Company>

			// Mediator

			ManagerMediator mediator = new ManagerMediator();
			Colleague customer = new CustomerColleague(mediator);
			Colleague coder = new ProgrammerColleague(mediator);
			Colleague tester = new TesterColleague(mediator);
			mediator.Customer = customer;
			mediator.Programmer = coder;
			mediator.Tester = tester;
			customer.Send("Є замовлення, потрібно зробити програму"); // Повідомлення програмісту: Є замовлення, потрібно зробити програму
			coder.Send("Програма готова, треба протестувати"); // Повідомлення тестеру: Програма готова, треба протестувати
			tester.Send("Програма протестована і готова до продажу"); // Повідомлення замовнику: Програма протестована і готова до продажу

			// Strategy

			Auto auto = new Auto(4, "Volvo", new PetrolMove());
			auto.Move(); // Переміщення на бензині
			auto.Movable = new ElectricMove();
			auto.Move(); // Переміщення на електриці

			// Chain of Responsibility

			Receiver receiver = new Receiver(false, true, true); // PayPalPayment OR MoneyPayment 
																 // false, false, false -> nothing written

			PaymentHandler bankPaymentHandler = new BankPaymentHandler();
			PaymentHandler moneyPaymentHadler = new MoneyPaymentHandler();
			PaymentHandler paypalPaymentHandler = new PayPalPaymentHandler();
			bankPaymentHandler.Successor = paypalPaymentHandler;
			paypalPaymentHandler.Successor = moneyPaymentHadler;

			bankPaymentHandler.Handle(receiver); // Виконуємо переведення через PayPal

			// NEW (BEHAVIOURAL):
			// Observer (Publisher - Subscriber)

			Stock stock = new Stock();
			BankObserver bank = new("ЮнитБанк", stock);
			BrokerObserver broker = new BrokerObserver("Иван Иваныч", stock);
			// имитация торгов
			stock.Market(); // Банк ЮнитБанк продает евро; Курс евро: 44
							// Брокер Иван Иваныч продает доллары; Курс доллара: 38
							// брокер прекращает наблюдать за торгами
			broker.StopTrade();
			// имитация торгов
			stock.Market(); // Банк ЮнитБанк покупает евро; Курс евро: 30

			// Template Method

			School school = new School();
			University university = new University();

			school.Learn();  // Output:
							 // Идем в первый класс
							 // Посещаем уроки, делаем домашние задания
							 // Сдаем выпускные экзамены
							 // Получаем аттестат о среднем образовании
			university.Learn(); // Output:
								// Сдаем вступительные экзамены и поступаем в ВУЗ
								// Посещаем лекции
								// Проходим практику
								// Сдаем экзамен по специальности
								// Получаем диплом о высшем образовании

			// Iterator

			Library library = new Library();
			Reader reader = new Reader();
			reader.SeeBooks(library); // Output:
									  // Война и мир
									  // Отцы и дети
									  // Вишневый сад

			// State

			Water water = new Water(new LiquidWaterState());
			water.Heat();
			water.Frost();
			water.Frost(); // Output:
						   // Превращаем жидкость в пар
						   // Превращаем водяной пар в жидкость
						   // Превращаем жидкость в лед

			// Memento

			Hero hero = new Hero();
			hero.Shoot(); // Производим выстрел. Осталось 9 патронов
			GameHistory game = new GameHistory();
			game.History.Push(hero.SaveState()); // Сохранение игры. Параметры: 9 патронов, 5 жизней
			hero.Shoot(); // Производим выстрел. Осталось 8 патронов
			hero.RestoreState(game.History.Pop()); // Восстановление игры. Параметры: 9 патронов, 5 жизней
			hero.Shoot(); // Производим выстрел. Осталось 8 патронов


			// NEW STRUCTURAL

			// Composite

			Component2 fileSystem = new Directory("Файловая система");
			// определяем новый диск
			Component2 diskC = new Directory("Диск С");
			// новые файлы
			Component2 pngFile = new FileComponent("12345.png");
			Component2 docxFile = new FileComponent("Document.docx");
			// добавляем файлы на диск С
			diskC.Add(pngFile);
			diskC.Add(docxFile);
			// добавляем диск С в файловую систему
			fileSystem.Add(diskC);
			// выводим все данные
			fileSystem.Print();
			// Узел Файловая система
			// Подузлы:
			//			Узел Диск С
			// Подузлы:
			//			12345.png
			//			Document.docx
			Console.WriteLine();
			// удаляем с диска С файл
			diskC.Remove(pngFile);
			// создаем новую папку
			Component2 docsFolder = new Directory("Мои Документы");
			// добавляем в нее файлы
			Component2 txtFile = new FileComponent("readme.txt");
			Component2 csFile = new FileComponent("Program.cs");
			docsFolder.Add(txtFile);
			docsFolder.Add(csFile);
			diskC.Add(docsFolder);
			fileSystem.Print();
			// Узел Файловая система
			// Подузлы:
			//			Узел Диск С
			// Подузлы:
			//			Document.docx
			//			Узел Мои Документы
			// Подузлы:
			// 			readme.txt
			// 			Program.cs

			// Bridge

			// создаем нового программиста, он работает с с++
			Programmer2 freelancer = new FreelanceProgrammer(new CPPLanguage());
			freelancer.DoWork();
			freelancer.EarnMoney(); // Output:
									// С помощью компилятора C++ компилируем программу в бинарный код
									// Запускаем исполняемый файл программы
									// Получаем оплату за выполненный заказ

			// пришел новый заказ, но теперь нужен c#
			freelancer.Language = new CSharpLanguage();
			freelancer.DoWork();
			freelancer.EarnMoney(); // Output:
									// С помощью компилятора Roslyn компилируем исходный код в файл exe
									// JIT компилирует программу в бинарный код
									// CLR выполняет скомпилированный бинарный код
									// Получаем оплату за выполненный заказ

			// Flyweight

			double longitude = 37.61;
			double latitude = 55.74;

			HouseFactory houseFactory = new HouseFactory();
			for (int i = 0; i < 5; i++)
			{
				House2 panelHouse = houseFactory.GetHouse("Panel");
				if (panelHouse != null)
					panelHouse.Build(longitude, latitude);
				longitude += 0.1;
				latitude += 0.1;
			}

			// Output:
			// Построен панельный дом из 16 этажей; координаты: 55,74 широты и 37,61 долготы
			// Построен панельный дом из 16 этажей; координаты: 55,84 широты и 37,71 долготы
			// Построен панельный дом из 16 этажей; координаты: 55,94 широты и 37,81 долготы
			// Построен панельный дом из 16 этажей; координаты: 56,04 широты и 37,91 долготы
			// Построен панельный дом из 16 этажей; координаты: 56,14 широты и 38,01 долготы
			for (int i = 0; i < 5; i++)
			{
				House2 brickHouse = houseFactory.GetHouse("Brick");
				if (brickHouse != null)
					brickHouse.Build(longitude, latitude);
				longitude += 0.1;
				latitude += 0.1;
			}

			// Output:
			// Построен кирпичный дом из 5 этажей; координаты: 56,24 широты и 38,11 долготы
			// Построен кирпичный дом из 5 этажей; координаты: 56,34 широты и 38,21 долготы
			// Построен кирпичный дом из 5 этажей; координаты: 56,44 широты и 38,31 долготы
			// Построен кирпичный дом из 5 этажей; координаты: 56,54 широты и 38,41 долготы
			// Построен кирпичный дом из 5 этажей; координаты: 56,64 широты и 38,51 долготы

			// ADDITIONAL: Fluent Builder (used in Commandos)

			User tom = new UserBuilder().SetName("Tom").SetCompany("Microsoft").SetAge(23).Build();
			User alice = User.CreateBuilder().SetName("Alice").IsMarried.SetAge(25).Build();

			// using implicit cast operator User
			User tom2 = new UserBuilder().SetName("Tom").SetCompany("Microsoft").SetAge(23);
			User alice2 = User.CreateBuilder().SetName("Alice").IsMarried.SetAge(25);
		}
		#endregion Patterns

		#region AdditionalTests
		static class AdditionalTests
		{
			class Person
			{
				public string Name { get; set; }
			}
			class People
			{
				Person[] data;
				public People()
				{
					data = new Person[5];
				}
				public Person this[int index]
				{
					get
					{
						return data[index];
					}
					set
					{
						data[index] = value;
					}
				}

				class Matrix
				{
					private int[,] numbers = new int[,] {
						{ 1, 2, 4 },
						{ 2, 3, 6 },
						{ 3, 4, 8 }
					};
					public int this[int i, int j]
					{
						get
						{
							return numbers[i, j];
						}
						set
						{
							numbers[i, j] = value;
						}
					}
				}
            }
            public class SimpleClass
            {
                // Static variable that must be initialized at run time
                public static readonly long StaticTicks;

                // Non static variable
                public readonly long NonStaticTicks;

                // Static constructor is called at most one time, before any
                // instance constructor is invoked or member is accessed.
                static SimpleClass()
                {
                    StaticTicks = DateTime.Now.Ticks;
                }

                public SimpleClass()
                {
                    // StaticTicks = DateTime.Now.Ticks;    // error: Fields of static readonly field
                    // cannot be assigned to (except in a static
                    // constructor or a variable initializer)
                    NonStaticTicks = DateTime.Now.Ticks;
                }
            }
            public class PathInfo
            {
                public string DirectoryName { get; }
                public string FileName { get; }
                public string Extension { get; }

                public PathInfo(string path)
                {

                }
                public void Deconstruct(out string directoryName, out string fileName, out string extension)
                {
                    directoryName = DirectoryName; fileName = FileName;
                    extension = Extension;
                }

                ~PathInfo() { }

                // protected override void Finalize()
                // {
                //     base.Finalize();
                // }

                private void SayName(object obj)
                {
                    if (obj as TextBox != null)
                    {
                        Console.WriteLine(((TextBox)obj));//.Name);
                    }
                }
                private void SayName2(object obj)
                {
                    TextBox tmp = obj as TextBox;
                    if (tmp != null)
                    {
                        Console.WriteLine(tmp);//.Name);
                    }
                }

                public Point Copy()
                {
                    return (Point)this.MemberwiseClone();
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
            public static void AdditionalLecturesTests()
			{
				char[] vowels = new char[10];
				char[] vowels1 = new char[] { 'a', 'e', 'i', 'о', 'u' };
				int[,] matrix = new int[3, 3];
				int[,] matrix1 = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
				// Спрощені форми
				char[] vowels2 = { 'а', 'е', 'i', 'о', 'u' };
				int[,] rectangularMatrix = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
				int[][] jaggedMatrix = { new int[] { 0, 1, 2 }, new int[] { 3, 4, 5 }, new int[] { 6, 7, 8 } };
				// Неявне задання типу
				var rectMatrix = new int[,] // rectMatrix неявно отримує тип int[,]
				{
 					{0,1,2}, {3,4,5}, {6,7,8}
				};
				var jaggedMat = new int[][] // jaggedMat неявно отримує тип int[][]
				{
				new int [ ] {0,1,2}, new int [] {3,4,5}, new int[] {6,7,8}
				};
				// Повне розпізнавання
				var vowels3 = new[] { 'а', 'е', 'i', 'о', 'u' }; // Компилятор виводить тип char[]

                People people = new People();
				people[0] = new Person { Name = "Tom" };
				people[1] = new Person { Name = "Bob" };

				Person tom = people[0];
				Console.WriteLine(tom?.Name);

				Widget w = new();
				w.Foo(); // Widget’s implementation of I1.Foo
                ((I1)w).Foo(); // Widget’s implementation of I1.Foo
                ((I2)w).Foo(); // Widget’s implementation of I2.Foo
            }
        }
        #endregion AdditionalTests

        /// <summary>
        /// Using all properties from System.Environment namespace
        /// </summary>
        private static void EnvironmentTest()
		{
			if (Environment.HasShutdownStarted)
				Console.WriteLine("Current application domain is being unloaded" + Environment.NewLine +
								  "or the common language runtime (CLR) is shutting down.");  // (false)
            if (Is64BitOperatingSystem) Console.WriteLine("This is 64 bit OS"); // (true)
            if (Is64BitProcess) Console.WriteLine("This is 64 bit process / address space"); // (true)
			Console.WriteLine("A unique identifier for this managed thread: " + CurrentManagedThreadId); // 1
            // !!!
			Console.WriteLine("The fully qualified path of the current working directory: " + CurrentDirectory);
				// C:\Users\Admin\source\repos\OlegApps\SigmaLectionsTest\SigmaLectionsTest\bin\Debug\net5.0
            Console.WriteLine("Command line arguments: " + CommandLine);
				// C: \Users\Admin\source\repos\OlegApps\SigmaLectionsTest\SigmaLectionsTest\bin\Debug\net5.0\SigmaLectionsTest.dll
            Console.WriteLine("The platform identifier and version number: " + OSVersion); // Microsoft Windows NT 10.0.19044.0
            Console.WriteLine("The unique identifier for the current process: " + ProcessId); // 8736
            Console.WriteLine("The number of processors on the current machine: " + ProcessorCount); // 4
            Console.WriteLine("Current stack trace information: " + Environment.StackTrace);
				// at System.Environment.get_StackTrace()
				// at SigmaLectionsTest.Program.EnvironmentTest() in C: \Users\Admin\source\repos\OlegApps\SigmaLectionsTest\SigmaLectionsTest\Program.cs:line 4022
				// at SigmaLectionsTest.Program.Main(String[] args) in C: \Users\Admin\source\repos\OlegApps\SigmaLectionsTest\SigmaLectionsTest\Program.cs:line 4002
            Console.WriteLine("The fully qualified path of the system directory: " + SystemDirectory); // C:\WINDOWS\system32
            Console.WriteLine("The number of bytes in the system memory page: " + SystemPageSize); // 4096
            Console.WriteLine("The number of milliseconds elapsed since the system started: " + TickCount); // 442968796
            Console.WriteLine("The number of milliseconds elapsed since the system started (long): " + TickCount64); // 442968796
				// 17.5 seconds: 443067046 - 443049484 = 17562 "ticks" -> Milliseconds
            Console.WriteLine("DateTime.Now.Ticks = " + DateTime.Now.Ticks); // 637961835302583735
				// 20.5 seconds: 637961837545352417 - 637961837358166084 = 187,186,304 (~9,131,039 per second)
            Console.WriteLine("The network domain name associated with the current user: " + UserDomainName); // OLEG
            if (UserInteractive) Console.WriteLine("The current process is running in user interactive mode."); // (true)
            Console.WriteLine("The user name of the person who is associated with the current thread: " + UserName); // Admin
            Console.WriteLine("The version of the common language runtime: " + Environment.Version); // 5.0.17
            Console.WriteLine("The amount of physical memory mapped to the process context: " + WorkingSet); // 19992576
            Console.WriteLine("The NetBIOS name of this local computer: " + MachineName); // OLEG
            Console.WriteLine("A string with Path environment variable replaced by its value: " +
                ExpandEnvironmentVariables("Path")); // Path
            // FailFast(string? message[, Exception? exception]); // do not use - this closes App and sends report to MS
            Console.WriteLine("The command-line arguments for the current process:");
            foreach (string s in GetCommandLineArgs()) Console.Write(s + "; ");
	            // C:\Users\Admin\source\repos\OlegApps\SigmaLectionsTest\SigmaLectionsTest\bin\Debug\net5.0\SigmaLectionsTest.dll;
            Console.WriteLine();
            Console.WriteLine("The value of Path environment variable for the current Machine: " + 
				GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine));
	            // C:\WINDOWS\system32;C:\WINDOWS;C:\WINDOWS\System32\Wbem;C:\WINDOWS\System32\WindowsPowerShell\v1.0\;C:\WINDOWS\System32\OpenSSH\;C:\Program Files (x86)\Delphi 10 Lite\bin;C:\Program Files\Microsoft SQL Server\150\Tools\Binn\;C:\Program Files\dotnet\;C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\
            Console.WriteLine();
            Console.WriteLine("All environment variable names for this machine: ");
            foreach (DictionaryEntry item in GetEnvironmentVariables(EnvironmentVariableTarget.Machine))
				Console.WriteLine(item.Key + " = " + item.Value);
            /*
			 * PATHEXT = .COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.JSE;.WSF;.WSH;.MSC
			 * Path = C:\WINDOWS\system32;C:\WINDOWS;C:\WINDOWS\System32\Wbem;C:\WINDOWS\System32\WindowsPowerShell\v1.0\;C:\WINDOWS\System32\OpenSSH\;C:\Program Files (x86)\Delphi 10 Lite\bin;C:\Program Files\Microsoft SQL Server\150\Tools\Binn\;C:\Program Files\dotnet\;C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\
			 * OS = Windows_NT
			 * PSModulePath = C:\Program Files\WindowsPowerShell\Modules;C:\WINDOWS\system32\WindowsPowerShell\v1.0\Modules
			 * ComSpec = C:\WINDOWS\system32\cmd.exe
			 * windir = C:\WINDOWS
			 * NUMBER_OF_PROCESSORS = 4
			 * PROCESSOR_ARCHITECTURE = AMD64
			 * PROCESSOR_LEVEL = 6
			 * TMP = C:\WINDOWS\TEMP
			 * PROCESSOR_IDENTIFIER = Intel64 Family 6 Model 58 Stepping 9, GenuineIntel
			 * DriverData = C:\Windows\System32\Drivers\DriverData
			 * PROCESSOR_REVISION = 3a09
			 * USERNAME = SYSTEM
			 * TEMP = C:\WINDOWS\TEMP
			 */
            Console.WriteLine();
            Console.WriteLine("All environment variable names for this process: ");
            foreach (DictionaryEntry item in GetEnvironmentVariables())
				Console.WriteLine(item.Key + " = " + item.Value);
            /*
			 * HOMEPATH = \Users\Admin
			 * VisualStudioEdition = Microsoft Visual Studio Community 2022
			 * VSLANG = 1049
			 * HOMEDRIVE = C:
			 * ProgramData = C:\ProgramData
			 * CommonProgramW6432 = C:\Program Files\Common Files
			 * DriverData = C:\Windows\System32\Drivers\DriverData
			 * MSBuildLoadMicrosoftTargetsReadOnly = true
			 * USERDOMAIN_ROAMINGPROFILE = OLEG
			 * ComSpec = C:\WINDOWS\system32\cmd.exe
			 * PROCESSOR_ARCHITECTURE = AMD64
			 * PROCESSOR_LEVEL = 6
			 * PATHEXT = .COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.JSE;.WSF;.WSH;.MSC
			 * COMPUTERNAME = OLEG
			 * Path = C:\WINDOWS\system32;C:\WINDOWS;C:\WINDOWS\System32\Wbem;C:\WINDOWS\System32\WindowsPowerShell\v1.0\;C:\WINDOWS\System32\OpenSSH\;C:\Program Files (x86)\Delphi 10 Lite\bin;C:\Program Files\Microsoft SQL Server\150\Tools\Binn\;C:\Program Files\dotnet\;C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\;C:\Users\Admin\Documents\Borland Studio Projects\Bpl;C:\Users\Admin\AppData\Local\Microsoft\WindowsApps;C:\Users\Admin\.dotnet\tools
			 * ProgramFiles(x86) = C:\Program Files (x86)
			 * FPS_BROWSER_USER_PROFILE_STRING = Default
			 * ProgramFiles = C:\Program Files
			 * ServiceHubLogSessionKey = 2FA04B5F
			 * LOCALAPPDATA = C:\Users\Admin\AppData\Local
			 * USERPROFILE = C:\Users\Admin
			 * OneDriveConsumer = C:\Users\Admin\OneDrive
			 * NUMBER_OF_PROCESSORS = 4
			 * VSAPPIDNAME = devenv.exe
			 * windir = C:\WINDOWS
			 * VisualStudioVersion = 17.0
			 * ForceIdentityAuthenticationType = Embedded
			 * SignInWithHomeTenantOnly = False
			 * OneDrive = C:\Users\Admin\OneDrive
			 * CommonProgramFiles = C:\Program Files\Common Files
			 * LOGONSERVER = \\OLEG
			 * TMP = C:\Users\Admin\AppData\Local\Temp
			 * VisualStudioDir = C:\Users\Admin\Documents\Visual Studio 2022
			 * COMPLUS_ForceEnc = 1
			 * FPS_BROWSER_APP_PROFILE_STRING = Internet Explorer
			 * SystemDrive = C:
			 * PkgDefApplicationConfigFile = C:\Users\Admin\AppData\Local\Microsoft\VisualStudio\17.0_cf9d526f\devenv.exe.config
			 * SESSIONNAME = Console
			 * VSAPPIDDIR = C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\
			 * ALLUSERSPROFILE = C:\ProgramData
			 * VSSKUEDITION = Community
			 * OS = Windows_NT
			 * PSModulePath = C:\Program Files\WindowsPowerShell\Modules;C:\WINDOWS\system32\WindowsPowerShell\v1.0\Modules
			 * ThreadedWaitDialogDpiContext = -4
			 * USERNAME = Admin
			 * PROCESSOR_REVISION = 3a09
			 * USERDOMAIN = OLEG
			 * SystemRoot = C:\WINDOWS
			 * PUBLIC = C:\Users\Public
			 * APPDATA = C:\Users\Admin\AppData\Roaming
			 * TEMP = C:\Users\Admin\AppData\Local\Temp
			 * PROCESSOR_IDENTIFIER = Intel64 Family 6 Model 58 Stepping 9, GenuineIntel
			 * CommonProgramFiles(x86) = C:\Program Files (x86)\Common Files
			 * ProgramW6432 = C:\Program Files
			 */
            Console.WriteLine();
            Console.WriteLine("Windows folder: " + GetFolderPath(SpecialFolder.Windows, SpecialFolderOption.None)); // C:\WINDOWS
            Console.WriteLine("Common pictures folder: " + GetFolderPath(SpecialFolder.CommonPictures)); // C:\Users\Public\Pictures
            Console.WriteLine("The names of the logical drives on the current computer:");
			foreach (string item in GetLogicalDrives())
				Console.Write(item + " "); // C:\ D:\ E:\ F:\
            // SetEnvironmentVariable(string variable, string? value[, EnvironmentVariableTarget target]); // I do not use
            ExitCode = 1;
            Exit(2); // Terminates this process and returns an exit code to the operating system.
                     // SigmaLectionsTest.exe (процесс 8736) завершил работу с кодом 2.
        }

		#region AsyncTest
	    // These classes are intentionally empty for the purpose of this example. They are simply marker classes for the purpose of demonstration, contain no properties, and serve no other purpose.
	    internal class Bacon { }
        internal class Coffee { }
        internal class Egg { }
        internal class Juice { }
        internal class Toast { }

        public class AsyncTest
        {
            public static async Task Test()
            {
                Coffee cup = PourCoffee();
                Console.WriteLine("coffee is ready");

                var eggsTask = FryEggsAsync(2);
                var baconTask = FryBaconAsync(3);
                var toastTask = MakeToastWithButterAndJamAsync(2);

                var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
                while (breakfastTasks.Count > 0)
                {
                    Task finishedTask = await Task.WhenAny(breakfastTasks);
                    if (finishedTask == eggsTask)
                    {
                        Console.WriteLine("eggs are ready");
                    }
                    else if (finishedTask == baconTask)
                    {
                        Console.WriteLine("bacon is ready");
                    }
                    else if (finishedTask == toastTask)
                    {
                        Console.WriteLine("toast is ready");
                    }
                    breakfastTasks.Remove(finishedTask);
                }

                Juice oj = PourOJ();
                Console.WriteLine("oj is ready");
                Console.WriteLine("Breakfast is ready!");

             // output:
                // Pouring coffee
                // coffee is ready
                // Warming the egg pan...
                // putting 3 slices of bacon in the pan
                // cooking first side of bacon...
                // Putting a slice of bread in the toaster
                // Putting a slice of bread in the toaster
                // Start toasting...
                // https://docs.microsoft.com/ru-ru/dotnet/csharp/programming-guide/concepts/async/
				// it is not finished by some reason
            }

            static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
            {
                var toast = await ToastBreadAsync(number);
                ApplyButter(toast);
                ApplyJam(toast);

                return toast;
            }

            private static Juice PourOJ()
            {
                Console.WriteLine("Pouring orange juice");
                return new Juice();
            }

            private static void ApplyJam(Toast toast) =>
                Console.WriteLine("Putting jam on the toast");

            private static void ApplyButter(Toast toast) =>
                Console.WriteLine("Putting butter on the toast");

            private static async Task<Toast> ToastBreadAsync(int slices)
            {
                for (int slice = 0; slice < slices; slice++)
                {
                    Console.WriteLine("Putting a slice of bread in the toaster");
                }
                Console.WriteLine("Start toasting...");
                await Task.Delay(3000);
                Console.WriteLine("Remove toast from toaster");

                return new Toast();
            }

            private static async Task<Bacon> FryBaconAsync(int slices)
            {
                Console.WriteLine($"putting {slices} slices of bacon in the pan");
                Console.WriteLine("cooking first side of bacon...");
                await Task.Delay(3000);
                for (int slice = 0; slice < slices; slice++)
                {
                    Console.WriteLine("flipping a slice of bacon");
                }
                Console.WriteLine("cooking the second side of bacon...");
                await Task.Delay(3000);
                Console.WriteLine("Put bacon on plate");

                return new Bacon();
            }

            private static async Task<Egg> FryEggsAsync(int howMany)
            {
                Console.WriteLine("Warming the egg pan...");
                await Task.Delay(3000);
                Console.WriteLine($"cracking {howMany} eggs");
                Console.WriteLine("cooking the eggs ...");
                await Task.Delay(3000);
                Console.WriteLine("Put eggs on plate");

                return new Egg();
            }

            private static Coffee PourCoffee()
            {
                Console.WriteLine("Pouring coffee");
                return new Coffee();
            }
        }
        #endregion

        public static void ConsoleTest()
		{
			Console.Clear();
            Console.WriteLine("WindowTop: " + Console.WindowTop); // 0
            Console.WriteLine("WindowLeft: " + Console.WindowLeft); // 0
            Console.WriteLine("BackgroundColor: " + Console.BackgroundColor); // Black
            Console.WriteLine("ForegroundColor: " + Console.ForegroundColor); // Gray
            Console.WriteLine("BufferHeight: " + Console.BufferHeight); // 900
            Console.WriteLine("BufferWidth: " + Console.BufferWidth); // 166
            Console.WriteLine("CapsLock: " + Console.CapsLock); // False
            Console.WriteLine("CursorLeft: " + Console.CursorLeft); // 0
            Console.WriteLine("CursorTop: " + Console.CursorTop); // 9
            Console.WriteLine("CursorSize: " + Console.CursorSize); // 25  
            Console.WriteLine("CursorVisible: " + Console.CursorVisible); // True
            Console.WriteLine("GetCursorPosition: " + Console.GetCursorPosition()); // (0, 12) 
            Console.WriteLine("Error: " + Console.Error); // System.IO.TextWriter + SyncTextWriter
            Console.WriteLine("In: " + Console.In); // System.IO.SyncTextReader
            Console.WriteLine("Out: " + Console.Out); // System.IO.TextWriter + SyncTextWriter
            Console.WriteLine("InputEncoding: " + Console.InputEncoding); // System.Text.UnicodeEncoding
            Console.WriteLine("OutputEncoding: " + Console.OutputEncoding); // System.Text.UnicodeEncoding
            Console.WriteLine("IsErrorRedirected: " + Console.IsErrorRedirected); // False
            Console.WriteLine("IsInputRedirected: " + Console.IsInputRedirected); // False
            Console.WriteLine("KeyAvailable: " + Console.KeyAvailable); // False
            Console.WriteLine("LargestWindowHeight: " + Console.LargestWindowHeight); // 44
            Console.WriteLine("LargestWindowWidth: " + Console.LargestWindowWidth); // 170
            Console.WriteLine("NumberLock: " + Console.NumberLock); // True
            Console.WriteLine("Title: " + Console.Title); // C:\Users\Admin\source\repos\OlegApps\SigmaLectionsTest\SigmaLectionsTest\bin\Debug\net5.0\SigmaLectionsTest.exe
            Console.WriteLine("TreatControlCAsInput: " + Console.TreatControlCAsInput); // False
            Console.WriteLine("WindowHeight: " + Console.WindowHeight); // 42
            Console.WriteLine("WindowLeft: " + Console.WindowLeft); // 0
            Console.WriteLine("WindowTop: " + Console.WindowTop); // 0
            Console.WriteLine("WindowWidth: " + Console.WindowWidth); // 166
            Console.ReadLine();
            Console.Clear();
			int num = Enum.GetValues(typeof(ConsoleColor)).Length; // 16
            for (int i = 0; i < Console.LargestWindowHeight; i++)
			{
				for (int j = 0; j < Console.LargestWindowWidth; j++)
				{
					Console.ForegroundColor = (ConsoleColor)((i + j) % num); // * 65793 = // 10101 hex
					Console.Write((char)num); //"¤"); // ►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►►... = char(16)
				}
				Console.WriteLine();
			}
            Console.ReadLine();
            Console.ResetColor();
            Console.Clear();
            // Let's go through all Console colors and set them as foreground  
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"Foreground color set to {color}");
            }
            // Restore original colors  
            Console.ResetColor();
            Console.WriteLine("=====================================");
            Console.ForegroundColor = ConsoleColor.White;
            // Let's go through all Console colors and set them as background  
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
                
            {
                Console.BackgroundColor = color;
                Console.WriteLine($"Background color set to {color}");
            }
            // Restore original colors  
            Console.ResetColor();
            Console.WriteLine("=====================================");
            /*
			 * Foreground color set to Black
			 * Foreground color set to DarkBlue
			 * Foreground color set to DarkGreen
			 * Foreground color set to DarkCyan
			 * Foreground color set to DarkRed
			 * Foreground color set to DarkMagenta
			 * Foreground color set to DarkYellow
			 * Foreground color set to Gray
			 * Foreground color set to DarkGray
			 * Foreground color set to Blue
			 * Foreground color set to Green
			 * Foreground color set to Cyan
			 * Foreground color set to Red
			 * Foreground color set to Magenta
			 * Foreground color set to Yellow
			 * Foreground color set to White
			 * =====================================
			 * Background color set to Black
			 * Background color set to DarkBlue
			 * Background color set to DarkGreen
			 * Background color set to DarkCyan
			 * Background color set to DarkRed
			 * Background color set to DarkMagenta
			 * Background color set to DarkYellow
			 * Background color set to Gray
			 * Background color set to DarkGray
			 * Background color set to Blue
			 * Background color set to Green
			 * Background color set to Cyan
			 * Background color set to Red
			 * Background color set to Magenta
			 * Background color set to Yellow
			 * Background color set to White
			 * ===================================== */
        }

        /// <summary>Main method is the program entry point, only currently needed methods are used.</summary>
        /// <remarks>Setting console encodings and running currenly studied procedure.</remarks>
        /// <returns>Nothing 😁</returns>
        /// <param name="args">Command Line parameters for the program</param>
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
            // Лекция20(); // SOLID, (de)serialization
            // Test();
            // AfterTest();
            /// <seealso cref="ПропаданиеСимвола"/>
            // ПропаданиеСимвола(); // bug-report has been sent to Microsoft. Works only in VS, not when EXE running
			// DictionarySerialization();
			// Patterns();
			// AdditionalTests.AdditionalLecturesTests();

			// AsyncTest.Test();
            // ConsoleTest();
            // EnvironmentTest();
        } // Main
    } // Program class
} // namespace
 