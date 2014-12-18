using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using BREadfruit.Helpers;

namespace BREadfruit.Tests.Low_level_tests
{
	using System.Text.RegularExpressions;
	[TestFixture]
	public class TESTGEN_tests
	{

		[Test]
		public void TESTGEN_test_01 ()
		{
			var parser = new Parser ();
			var tgi = parser.CreateTestForEntity ( string.Join ( Environment.NewLine,
				System.IO.File.ReadAllLines ( @"..\..\sample files\single entity tests\File058 - TestGen.txt" ) ) );

			Assert.That ( tgi.Trim ().StartsWith ( "[Test]" ) );
		}


		[Test]
		public void TESTGEN_test_02 ()
		{
			var parser = new Parser ();
			var tgi = parser.CreateTestForEntity ( string.Join ( Environment.NewLine,
				System.IO.File.ReadAllLines ( @"..\..\sample files\single entity tests\File059 - TestGen No Instructions.txt" ) ) );

			Assert.IsEmpty ( tgi );
		}

		
	}
}
