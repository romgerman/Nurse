using System;
using System.Collections.Generic;

namespace Nurse
{
	public class IniSection
	{
		public Dictionary<string, string> Options { get; protected set; }
		public string Raw { get; internal set; }
		public bool Exists { get; internal set; }

		public string this[string key]
		{
			get
			{
				return GetValue<string>(key);
			}
		}

		public IniSection()
		{
			this.Options = new Dictionary<string, string>();
		}

		public T GetValue<T>(string key)
		{
			string value;

			if (Options.TryGetValue(key, out value))
				return value.ToObject<T>();

			return default(T);
		}
	}
}
