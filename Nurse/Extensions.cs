using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nurse
{
	internal static class Extensions
	{
		private static char[] validStartChars = new char[] { '.', ':', '$' };

		/// <summary>Doesn't fully check according to https://github.com/SemaiCZE/inicpp/wiki/INI-format-specification </summary>
		public static bool IsValidIdentifier(this string str)
		{
			if (char.IsLetter(str[0]) || str[0].IsAnyOf(validStartChars))
				return true;

			return false;
		}

		public static bool IsAnyOf(this char c, char[] arr)
		{
			foreach(var i in arr)
				if (c == i)
					return true;

			return false;
		}

		public static T ToObject<T>(this string str)
		{
			var type = typeof(T);
			object result = null;

			if (type.Equals(typeof(string)))
			{
				result = str;
			}
			else if (type.Equals(typeof(int)))
			{
				int o;
				if (int.TryParse(str, out o))
					result = o;
			}
			else if (type.Equals(typeof(float)))
			{
				float o;
				if (float.TryParse(str, out o))
					result = o;
			}
			else if (type.Equals(typeof(bool)))
			{
				if (str.Equals("true", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("1", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("yes", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("enabled", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("on", StringComparison.CurrentCultureIgnoreCase))
				{
					result = true;
				}

				if (str.Equals("false", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("0", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("no", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("n", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("disabled", StringComparison.CurrentCultureIgnoreCase) ||
					str.Equals("off", StringComparison.CurrentCultureIgnoreCase))
				{
					result = false;
				}
			}

			return (T)result;
		}
	}
}
