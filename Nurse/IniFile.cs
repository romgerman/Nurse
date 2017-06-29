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

/*
	If allowInvalidFile option is enabled then:
		- File can contain invalid identifiers
		- File can contain no sections
*/

namespace Nurse
{
	/// <summary>
	/// Represents ini file
	/// </summary>
	public class IniFile
    {
		public Dictionary<string, IniSection> Sections { get; private set; }
		
		private IniSection _emptySection = new IniSection() { Exists = false };
		private IniSection _currentSection;

		private bool _allowInvalidFile = false;

		public IniSection this[string key]
		{
			get
			{
				return GetSection(key);
			}
		}

		public string this[string section, string option]
		{
			get
			{
				var s = GetSection(section);

				if (!s.Exists)
					return null;

				return s.GetValue<string>(option);
			}
		}

		public IniFile()
		{
			Sections = new Dictionary<string, IniSection>();
		}

		/// <summary>
		/// Open and parse the file
		/// </summary>
		/// <param name="path">Path to ini file</param>
		/// <param name="allowInvalidFile">Allows ini file to be a little invalid</param>
		public void ReadFile(string path, bool allowInvalidFile = false)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentException("Path is empty or null", "path");

			string source = File.ReadAllText(path);

			ReadString(source, allowInvalidFile);
		}

		/// <summary>
		/// Parse string
		/// </summary>
		/// <param name="source">Ini data</param>
		/// <param name="allowInvalidFile">Allows ini file to be a little invalid</param>
		public void ReadString(string source, bool allowInvalidFile = false)
		{
			if (source == null)
				throw new ArgumentNullException("source", "Source is null");

			if (string.IsNullOrWhiteSpace(source))
				return;

			_allowInvalidFile = allowInvalidFile;

			string[] raw = source.Split('\n');

			foreach(string line in raw)
				ParseLine(line);
		}

		private void ParseLine(string line)
		{
			var prepLine = line.TrimStart().TrimEnd();

			if (string.IsNullOrEmpty(prepLine))
				return;

			if (prepLine.StartsWith("[")) // Section
			{
				var commIndex = prepLine.IndexOf(';'); // Check if we have comment on the line
				var endIndex = prepLine.IndexOf(']');

				if (commIndex > -1) // We have comment
				{
					if (endIndex > commIndex && !_allowInvalidFile) // "]" is in comment
					{
						throw new Exception("Invalid file");
					}
				}

				var sectionName = line.Substring(1, endIndex - 1);

				if (!_allowInvalidFile && !sectionName.IsValidIdentifier())
					throw new Exception("Invalid file");

				_currentSection = new IniSection() { Exists = true };
				Sections.Add(sectionName, _currentSection);

				return;
			}
			else if(prepLine.StartsWith(";") || prepLine.StartsWith("#")) // Comment
			{
				return;
			}
			else
			{
				var eqIndex = prepLine.IndexOf('=');
				var commIndex1 = prepLine.IndexOf(';');
				var commIndex2 = prepLine.IndexOf('#');

				if (eqIndex > -1)
				{
					var identifier = prepLine.Substring(0, eqIndex).TrimEnd();

					if (!_allowInvalidFile && !identifier.IsValidIdentifier())
						throw new Exception("Invalid file");

					var value = prepLine.Substring(eqIndex + 1);

					if (commIndex1 > -1 || commIndex2 > -1)
					{
						value = value.Substring(0, value.Length - (commIndex1 > -1 ? commIndex1 : commIndex2));
					}

					_currentSection.Options.Add(identifier, value.TrimStart().TrimEnd());
				}
				else
				{
					if (!_allowInvalidFile)
						throw new Exception("Invalid file");

					if (commIndex1 > -1 || commIndex2 > -1)
					{
						_currentSection.Options.Add(prepLine.Substring(0, prepLine.Length - (commIndex1 > -1 ? commIndex1 : commIndex2)), null);
					}
					else
					{
						_currentSection.Options.Add(prepLine, null);
					}
				}

				return;
			}
		}

		public IniSection GetSection(string key)
		{
			IniSection section;

			if (Sections.TryGetValue(key, out section))
				return section;

			return _emptySection;
		}
	}
}
