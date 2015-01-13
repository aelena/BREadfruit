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

		[Test]
		public void TESTGEN_test_03 ()
		{
			var parser = new Parser ();
			var tgi = parser.CreateTestForEntity ( string.Join ( Environment.NewLine,
				System.IO.File.ReadAllLines ( @"..\..\sample files\single entity tests\File060 - TestGen - Incomplete.txt" ) ) );
			Assert.That ( tgi.Trim ().StartsWith ( "[Test]" ) );

		}

		[Test]
		public void TESTGEN_test_04 ()
		{
			var parser = new Parser ();
			var totalAutogenString = parser.CreateTestForEntities ( @"..\..\sample files\vendor-rules-simple.txt", "ExampleVendor" );

			System.IO.File.WriteAllText ( @"..\..\autogen_tests\autogen.cs", totalAutogenString );

			Assert.That ( totalAutogenString.IndexOf ( "[TestFixture]" ) > 0 );
		}

		[Test]
		public void TESTGEN_test_05 ()
		{
			var parser = new Parser ();
			var totalAutogenString = parser.CreateTestForEntities ( @"..\..\sample files\customer-rules.txt", "CustomerEntities" );

			System.IO.File.WriteAllText ( @"..\..\autogen_tests\customer-autogen-tests.cs", totalAutogenString );

			Assert.That ( totalAutogenString.IndexOf ( "[TestFixture]" ) > 0 );
		}

		[Test]
		public void TESTGEN_test_06 ()
		{
			var parser = new Parser ();
			var totalAutogenString = parser.CreateTestForEntities ( @"..\..\sample files\vendor-rules.txt", "VendorEntities" );

			System.IO.File.WriteAllText ( @"..\..\autogen_tests\vendor-autogen-tests.cs", totalAutogenString );

			Assert.That ( totalAutogenString.IndexOf ( "[TestFixture]" ) > 0 );
		}
	}
}
