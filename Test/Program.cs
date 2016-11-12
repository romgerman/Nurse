using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nurse;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			IniFile ini = new IniFile();
			ini.Open("B:\\something.ini");

			Console.WriteLine(ini["database7"]["server1"]);

			foreach(IniSection section in ini.Sections)
			{
				Console.WriteLine(section.Name);

				foreach (KeyValuePair<string, string> s in section.Properties)
				{
					Console.WriteLine(s.Key + "=" + s.Value);
				}
			}

			Console.ReadKey();
		}
	}
}
