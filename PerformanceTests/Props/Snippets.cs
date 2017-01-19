using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests.Props
{
	internal class Snippets
	{
		public static string ConsoleApp => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTest
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

}
";

		internal static int GetCaretPositionInConsoleApp(Location completionLocation)
		{
			switch (completionLocation)
			{
				case Location.OutsideNamespace:
					return 0;
				case Location.WithinClass:
					return 175;
				case Location.WithinMethod:
					return 224;
				case Location.AfterClass:
					return 244;
			}
			return 0;
		}

		public static string ExtraCode => @"
namespace PerfTest
{
    class ExtraCode
    {
		public static T Find<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			if (match == null)
			{
				throw new ArgumentNullException(""match"");
			}

			for (int i = 0; i < array.Length; i++)
			{
				if (match(array[i]))
				{
					return array[i];
				}
			}
			return default(T);
		}

		public static T[] FindAll<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			if (match == null)
			{
				throw new ArgumentNullException(""match"");
			}

			List<T> list = new List<T>();
			for (int i = 0; i < array.Length; i++)
			{
				if (match(array[i]))
				{
					list.Add(array[i]);
				}
			}
			return list.ToArray();
		}

		public static int FindIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			return FindIndex(array, 0, array.Length, match);
		}

		public static int FindIndex<T>(T[] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			return FindIndex(array, startIndex, array.Length - startIndex, match);
		}

		public static int FindIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			if (startIndex < 0 || startIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException(""startIndex"");
			}

			if (count < 0 || startIndex > array.Length - count)
			{
				throw new ArgumentOutOfRangeException(""count"");
			}

			if (match == null)
			{
				throw new ArgumentNullException(""match"");
			}

			int endIndex = startIndex + count;
			for (int i = startIndex; i < endIndex; i++)
			{
				if (match(array[i])) return i;
			}
			return -1;
		}

		public static T FindLast<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			if (match == null)
			{
				throw new ArgumentNullException(""match"");
			}

			for (int i = array.Length - 1; i >= 0; i--)
			{
				if (match(array[i]))
				{
					return array[i];
				}
			}
			return default(T);
		}

		public static int FindLastIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			return FindLastIndex(array, array.Length - 1, array.Length, match);
		}

		public static int FindLastIndex<T>(T[] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			return FindLastIndex(array, startIndex, startIndex + 1, match);
		}

		public static int FindLastIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			if (match == null)
			{
				throw new ArgumentNullException(""match"");
			}

			if (array.Length == 0)
			{
				// Special case for 0 length List
				if (startIndex != -1)
				{
					throw new ArgumentOutOfRangeException(""startIndex"");
				}
			}
			else
			{
				// Make sure we're not out of range            
				if (startIndex < 0 || startIndex >= array.Length)
				{
					throw new ArgumentOutOfRangeException(""startIndex"");
				}
			}

			// 2nd have of this also catches when startIndex == MAXINT, so MAXINT - 0 + 1 == -1, which is < 0.
			if (count < 0 || startIndex - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException(""count"");
			}

			int endIndex = startIndex - count;
			for (int i = startIndex; i > endIndex; i--)
			{
				if (match(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public static void ForEach<T>(T[] array, Action<T> action)
		{
			if (array == null)
			{
				throw new ArgumentNullException(""array"");
			}

			if (action == null)
			{
				throw new ArgumentNullException(""action"");
			}

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}
	}
}
";

	}
}
