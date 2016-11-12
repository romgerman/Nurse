using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

/*
	Example:
		prop1=val1
		prop2="in quotes"
		prop3 = with space (or many spaces?)

		[Section1]
		prop1=2

		[Section2]
		[SectionInSection2.Text] ;comment
		prop=?
*/

namespace Nurse
{
	public class IniSection
	{
		public string Name { get; protected set; }
		public List<KeyValuePair<string, string>> Properties { get; protected set; }

		public string this[string key]
		{
			get
			{
				return GetValue(key);
			}
		}

		public IniSection(string name)
		{
			this.Name = name;
			Properties = new List<KeyValuePair<string, string>>();
		}

		public string GetValue(string key)
		{
			return Properties.Find((KeyValuePair<string, string> prop) => { return prop.Key.Equals(key); }).Value;
		}
	}

	/// <summary>
	/// Main class
	/// </summary>
	public class IniFile
    {
		public List<IniSection> Sections { get; private set; }
		public IniSection Root { get { return root; } }

		private IniSection root;
		private IniSection currentSection;

		private IniSection emptySection = new IniSection("");

		public IniSection this[string key]
		{
			get
			{
				return GetSection(key);
			}
		}

		public IniFile()
		{
			Sections = new List<IniSection>(1);
			Sections.Add(new IniSection(string.Empty));

			root = Sections[0];
			currentSection = root;
		}

		/// <summary>
		/// Open and parse the file
		/// </summary>
		/// <param name="path">Path to ini file</param>
		public void Open(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentException("Path is empty or null", "path");

			string source = File.ReadAllText(path);

			Parse(source);
		}

		/// <summary>
		/// Parse string
		/// </summary>
		/// <param name="source">Ini data</param>
		public void Parse(string source)
		{
			if (source == null)
				throw new ArgumentNullException("source", "Source is null");

			string[] raw = source.Split('\n');

			foreach(string line in raw)
			{
				string prepLine = line.Trim();

				ParseLine(prepLine);
			}
		}

		private void ParseLine(string line)
		{
			if (string.IsNullOrEmpty(line))
				return;

			if (line.StartsWith("[") && line.EndsWith("]")) // Section
			{
				line = line.Substring(1, line.Length - 2);

				currentSection = new IniSection(line);
				Sections.Add(currentSection);

				return;
			}
			else if(line.StartsWith(";") || line.StartsWith("#")) // Comment
			{
				return;
			}
			else
			{
				if(!line.Contains("\""))
				{
					int index = line.IndexOf(';');

					if(index > 0)
						line = line.Substring(0, index);
				}

				string[] keyVal = line.Split('=');
				string key = keyVal[0];
				string value = string.Empty;

				if(keyVal.Length > 1)
				{
					if (keyVal[1].StartsWith("\""))
					{
						if(keyVal[1].EndsWith("\""))
						{
							value = keyVal[1].Substring(1, keyVal[1].Length - 2);
						}
						else
						{
							value = keyVal[1];
						}						
					}
					else
					{
						value = keyVal[1];
					}
				}

				currentSection.Properties.Add(new KeyValuePair<string, string>(key, value));
			}
		}

		public IniSection GetSection(string key)
		{
			if (string.IsNullOrEmpty(key))
				return root;

			IniSection result = Sections.Find((IniSection section) => { return section.Name.Equals(key); });

			return result != null ? result : emptySection;
		}

		public string GetValue(string section, string key)
		{
			return GetSection(section).GetValue(key);
		}

		/// <summary>
		/// Returns true if ini files has differences
		/// </summary>
		/// <param name="ini"></param>
		/// <returns></returns>
		public bool Diff(IniFile ini, StringComparison comparison = StringComparison.CurrentCulture)
		{
			if (ini.Sections.Count != Sections.Count)
				return true;

			// Make this

			return false;
		}
	}
}
