using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nurse;

namespace Tests
{
	[TestClass]
	public class Tests
	{
		string simple = @"
[Section 1]
variable1 = some value
variable2 = 4532

[Section 2]
variable1 = ""Hello world""
test boolean=true
";

		[TestMethod]
		public void SimpleTest()
		{
			IniFile file = new IniFile();
			file.ReadString(simple);

			Assert.IsTrue(file["Section 1"].Exists);
			//Assert.IsTrue(file["Section 2"]["variable1"] == "Hello world");
			Assert.IsTrue(file["Section 2"].GetValue<bool>("test boolean"));
		}

		[TestMethod]
		public void EmptyStringTest()
		{
			IniFile file = new IniFile();
			file.ReadString("");

			Assert.IsTrue(file.Sections.Count == 0);
		}
	}
}
