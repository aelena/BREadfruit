using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BREadfruit.Tests.Low_level_tests
{
	[TestFixture]
	public class ActionTests
	{

		[Test]
		public void EqualsTest_UnaryAction_1 ()
		{
			Assert.IsFalse ( Grammar.VisibleUnaryActionSymbol.Equals ( null ) );
		}

		[Test]
		//[ExpectedException ( ExpectedException = typeof ( ArgumentNullException ) )]
		public void EqualsTest_UnaryAction_2 ()
		{
			Assert.IsFalse ( Grammar.VisibleUnaryActionSymbol.Equals ( "" ) );
		}

		[Test]
		public void EqualsTest_UnaryAction_3 ()
		{
			Assert.IsFalse ( Grammar.VisibleUnaryActionSymbol.Equals ( new object () ) );
		}

		[Test]
		public void EqualsTest_UnaryAction_4 ()
		{
			Assert.IsFalse ( Grammar.VisibleUnaryActionSymbol.Equals ( Grammar.DisableUnaryActionSymbol ) );
		}

		[Test]
		public void EqualsTest_UnaryAction_5 ()
		{
			Assert.IsTrue ( Grammar.VisibleUnaryActionSymbol.Equals ( Grammar.VisibleUnaryActionSymbol ) );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentNullException ) )]
		public void EqualsTest_UnaryAction_6 ()
		{
			Assert.IsFalse ( Grammar.VisibleUnaryActionSymbol == "" );
		}
	}
}
